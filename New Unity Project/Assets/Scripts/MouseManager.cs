using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{

    [SerializeField] GameObject selectTool;
    float holdTime = 0.7f;
    float acumyime = 0;
    Vector2 startTouch = Vector2.zero;

    bool isTouchActive = true;

    private void Start()
    {

    }
    void Update()
    {
        //imlecin pozisyonunu kameranın pozisyonuna göre konumla
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int layerMask = 1 << 8;
        // O konumdan dikey olarak ışın yolluyoruz
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, 0, layerMask);

        //oyun ekranına değerse aktif ol
        if (hit && isTouchActive)
        {
            GameObject hitObject = hit.transform.gameObject;

            if (Input.touches.Length > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    startTouch = touch.position;


                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (Mathf.Abs(touch.position.magnitude - startTouch.magnitude) < 8f)
                    {
                        TouchMove(hitObject, worldPoint);
                    }
                    else
                    {
                        Vector2 swipeDelta = touch.position - startTouch;
                        float x = swipeDelta.x;
                        float y = swipeDelta.y;
                        if (x < 0)
                        {

                            StartCoroutine(SwipeLeft(true));
                            isTouchActive = false;
                        }
                        else if (x > 0)
                        {
                            StartCoroutine(SwipeLeft(false));
                            isTouchActive = false;
                        }
                    }
                }
            }
        }
    }

    void TouchMove(GameObject hitObject, Vector2 worldPoint)
    {
        selectTool.GetComponent<SelectTool>().Clear();


        Hex _hex = hitObject.GetComponent<Hex>();
        float minDistance = 1000; //ilk değerin büyük olması lazım
        Vector2 minCorner = Vector2.zero;

        for (int i = 0; i < 6; i++) //tıklanan alandaki altıgenin en yakın köşesini seçiyor.
        {
            float temp = Vector2.Distance(worldPoint, _hex.corners[i]);
            if (temp < minDistance)
            {
                minDistance = temp;
                minCorner = _hex.corners[i];

            }
        }
        selectTool.SetActive(true);
        selectTool.transform.position = new Vector3(minCorner.x, minCorner.y, 0);//seçim aracı o noktaya oturuyor.
        selectTool.GetComponent<SelectTool>().releaseTriggers = true;
    }

    IEnumerator SwipeLeft(bool left)
    {

        float angleOffset = 3f;
        float angle = (360 / 3f);
        while (true)
        {

            if (left)
            {
                if (selectTool.transform.eulerAngles.z < (angle - angleOffset) && selectTool.transform.eulerAngles.z >= 0)
                {
                    selectTool.transform.rotation = Quaternion.Lerp(selectTool.transform.rotation, Quaternion.Euler(0, 0, 120), Time.deltaTime * 4f);

                }
               
                else if (selectTool.transform.eulerAngles.z >= (angle - angleOffset) && selectTool.transform.eulerAngles.z <= ((angle * 2) - angleOffset))
                {
                    if (selectTool.transform.eulerAngles.z == 120f)
                    {
                        switchFunc = true;
                        HexControl();
                    }

                    selectTool.transform.rotation = Quaternion.Lerp(selectTool.transform.rotation, Quaternion.Euler(0, 0, 240), Time.deltaTime * 4f);

                }
                
                
                else if (selectTool.transform.eulerAngles.z > ((angle * 2) - angleOffset) && selectTool.transform.eulerAngles.z < ((angle * 3) - angleOffset))
                {

                    if (selectTool.transform.eulerAngles.z == 240f)
                    {
                        switchFunc = true;
                        HexControl();
                    }
                    selectTool.transform.rotation = Quaternion.Lerp(selectTool.transform.rotation, Quaternion.Euler(0, 0, 360), Time.deltaTime * 4f);

                }
                else if (selectTool.transform.eulerAngles.z >= ((angle * 3) - angleOffset))
                {
                    selectTool.transform.rotation = Quaternion.Euler(0, 0, 0);
                    isTouchActive = true;
                    break;
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
    bool switchFunc = true;
    void HexControl()
    {
        if (switchFunc)
        {



            //seçilen alandaki hexleri up left ve right olarak değişkene al
            {
                int xTemp = -1, yTemp = -1;
                Hex hexUpTemp = null, hexLeftTemp = null, hexRightTemp = null;
                foreach (var hex in selectTool.GetComponent<SelectTool>().selectedHexs)
                {
                    if (yTemp < hex.y)
                    {
                        hexUpTemp = hex;
                        yTemp = hexUpTemp.y;
                    }

                }
                foreach (var hex in selectTool.GetComponent<SelectTool>().selectedHexs)
                {
                    if (hex != hexUpTemp)
                    {
                        if (hex.x > xTemp)
                        {
                            hexRightTemp = hex;
                            xTemp = hexRightTemp.x;
                        }
                    }
                }

                foreach (var hex in selectTool.GetComponent<SelectTool>().selectedHexs)
                {
                    if (hex != hexUpTemp)
                        if (hex != hexRightTemp)
                            hexLeftTemp = hex;
                }
            }



            foreach (var hex in selectTool.GetComponent<SelectTool>().selectedHexs)
            {
                hex.CheckNeigboursColor();


            }



            switchFunc = false;
        }
    }
}
