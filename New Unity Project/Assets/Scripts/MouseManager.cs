using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{

    [SerializeField] GameObject selectTool;

    void FixedUpdate()
    {
        //imlecin pozisyonunu kameranın pozisyonuna göre konumla
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int layerMask = 1 << 8;
        // O konumdan dikey olarak ışın yolluyoruz
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, 0, layerMask);
        
        //ışın çarparsa true dön
        if (hit)
        {
            GameObject hitObject = hit.transform.gameObject;

            if (Input.GetMouseButtonDown(0))
            {
                selectTool.GetComponent<SelectTool>().Clear();
                

                Hex _hex = hitObject.GetComponent<Hex>();
                float minDistance=100; //ilk değerin büyük olması lazım
                Vector2 minCorner=Vector2.zero;

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
        }

    }
}
