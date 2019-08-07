using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MouseManager : MonoBehaviour
{

    [SerializeField] GameObject selectTool;
    Vector2 startTouch = Vector2.zero;

    public Map map;

    bool isTouchActive = true;
    void Update()
    {
        //imlecin pozisyonunu kameranın pozisyonuna göre konumlar
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int layerMask = 1 << 8;
        // O konumdan dikey olarak ışın yolluyoruz
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, 0, layerMask);

        //oyun ekranına değerse ve herhangi bir işlem yapılmıyorsa aktif ol
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
                    if (Mathf.Abs(touch.position.magnitude - startTouch.magnitude) < 7f)
                    {
                        TouchMove(hitObject, worldPoint);//sadece dokunuş ise seçim aracını o noktaya taşınması için fonksiyona yollar.
                    }
                    else
                    {
                        Vector2 swipeDelta = touch.position - startTouch;
                        float x = swipeDelta.x;
                        float y = swipeDelta.y;
                        if (x < 0)//sağ sol swipe ayrımı için yazılmıştı fakat sağ swipe yetişmedi. ikiside sola swipe yapar.
                        {

                            StartCoroutine(SwipeLeft(true));
                            isTouchActive = false;
                        }
                        else if (x > 0)
                        {
                            StartCoroutine(SwipeLeft(true));
                            isTouchActive = false;
                        }
                    }
                }
            }
#if UNITY_EDITOR //geliştirme için deneme ınputları
            if (Input.GetMouseButtonDown(0))
            {
                TouchMove(hitObject, worldPoint);
            }
#endif
            if (Input.GetKey(KeyCode.Space))
            {
                StartCoroutine(SwipeLeft(true));
                isTouchActive = false;
            }
            
        }
    }
    public void SwipeLeftButton()
    {
        if (isTouchActive)
        {
            StartCoroutine(SwipeLeft(true));
            isTouchActive = false;
        }
    }
    void TouchMove(GameObject hitObject, Vector2 worldPoint)
    {
        selectTool.GetComponent<SelectTool>().Clear(); //seçim aracındaki secili hezleri temizler

        Hex _hex = hitObject.GetComponent<Hex>();
        float minDistance = 1000; //ilk değerin büyük olması lazım
        Vector2 minCorner = Vector2.zero;

        selectTool.transform.rotation = Quaternion.Euler(0, 0, 0);
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
        float angle = (360 / 3f);//3 aşamalı dönme olacak
        while (true)
        {
            if (left)
            {

                if (selectTool.transform.eulerAngles.z < (angle - angleOffset) && selectTool.transform.eulerAngles.z >= 0)
                {
                    selectTool.transform.rotation = Quaternion.Lerp(selectTool.transform.rotation, Quaternion.Euler(0, 0, 120), Time.deltaTime * 4f);

                }//Lerp ile zamanla yavaşlayan bir dönme hareketi sağlıyoruz.
                else if (selectTool.transform.eulerAngles.z >= (angle - angleOffset) && selectTool.transform.eulerAngles.z <= ((angle * 2) - angleOffset))
                {
                    if (selectTool.transform.eulerAngles.z <= 120) 
                    {//her aşamnın sonunda hexleri secim aracının konumuna göre spawn'layacağımız için rotation'nunu sabitliyoruz. 
                        selectTool.transform.rotation = Quaternion.Euler(0, 0, 120);
                        if (HexControl(120f))//control ediliyor.
                        {
                            Debug.Log("dur");
                            isTouchActive = true;
                            break;
                        }
                    }
                    selectTool.transform.rotation = Quaternion.Lerp(selectTool.transform.rotation, Quaternion.Euler(0, 0, 240), Time.deltaTime * 4f);
                }


                else if (selectTool.transform.eulerAngles.z > ((angle * 2) - angleOffset) && selectTool.transform.eulerAngles.z < ((angle * 3) - angleOffset))
                {

                    if (selectTool.transform.eulerAngles.z <= 240)
                    {
                        selectTool.transform.rotation = Quaternion.Euler(0, 0, 240);
                        if (HexControl(240f))
                        {
                            Debug.Log("dur");
                            isTouchActive = true;
                            break;
                        }
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
    bool  HexControl(float degre)
    {
        
        foreach (var item in selectTool.GetComponent<SelectTool>().selectedHexs)//secilen tüm hexlerin etrafını tek tek kontrol eder.
        {
            List<Hex> matchesHex = item.FindMatchHex();
            List<GameObject> newHexs = new List<GameObject>();
            if (matchesHex.Count >= 3)//bir hexin etrafındaki 3 hex aynı id değerine sahipse 
            {
                selectTool.transform.eulerAngles = new Vector3(0, 0, degre);
                
                NewHexPositions(selectTool.GetComponent<SelectTool>().selectedHexs);//hexlerin spawn anındaki konumlarını yeni konumları güncelliyoruz.

                foreach (Hex hex in matchesHex)
                {
                    //tüm eşleşen hexler rastgele yeni bir id alır.
                    int random;
                    do
                        random = UnityEngine.Random.Range(0, 6);
                    while (hex.id == random);

                    Hex selectedHex = map.HexInstantiate(hex, random,hex.transform.parent);//hangi parent'e aitse yine parent içerine yeni bir hex spawn olur.
                       selectedHex.transform.rotation = Quaternion.identity;

                    if (hex.transform.parent == selectTool.transform)//seçim aracının içerisinde ise yine seçim aracının scim listesine ekleniyor.
                    {
                        selectTool.GetComponent<SelectTool>().selectedHexs.Remove(hex);
                        selectTool.GetComponent<SelectTool>().selectedHexs.Add(selectedHex);
                    }


                    Destroy(hex);//hex scriptinin OnDestroy() çağırılıyor.
                }
               FindObjectOfType<GameManager>().Score++;
                matchesHex.Clear();//eşlenen hexler daha sonra çakışmaması adına temizleniyor.
                return true;
            }
            
        }
        return false;
    }

    void NewHexPositions(List<Hex> selectedHex)
    {
        foreach (var hex in selectedHex)
        {
            hex._x = hex.transform.position.x;
            hex._y = hex.transform.position.y;
            hex.transform.rotation = Quaternion.identity;
        }
    }

}
