using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{

    [SerializeField] GameObject selectTool;

    void FixedUpdate()
    {
        //imlecin pozisyonunu kameranın pozisyonuna göre konumlandırıyoruz.
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); 

        // O konumdan dikey olarak ışın yolluyoruz
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, 0);

        //ışın çarparsa true döner
        if (hit)
        {
            GameObject hitObject = hit.transform.gameObject;
            
            if (Input.GetMouseButtonDown(0))
            {
                Hex _hex = hitObject.GetComponent<Hex>();
                float minDistance=100;
                Vector2 minCorner=Vector2.zero;
                for (int i = 0; i < 6; i++)
                {
                   
                    float temp = Vector2.Distance(worldPoint, _hex.corners[i]);
                    if (temp < minDistance)
                    {
                        minDistance = temp;
                        minCorner = _hex.corners[i];
                    }
                }
                Debug.Log(_hex.name);
                selectTool.transform.position = new Vector3(minCorner.x, minCorner.y, 0); ;



                
            }
        }

    }
}
