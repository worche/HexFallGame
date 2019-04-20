using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour 
{
    //yükseklik ve genişlik
    public int widht = 10; 
    public int height = 10;
    public GameObject[] hexPrefab;

    //mapin altıgenlerden oluşması için manuel bir offset değeri veriyoruz
    float xOffset = 0.957f;
    float yOffset = 0.834f;

    private void Start()
    {
        for (int x = 0; x < widht; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xPos = x*xOffset;
                if (y % 2 == 1)
                {
                    xPos += xOffset/2f;
                }
                //rastgele bir objeyi offset değerlerini kullanarak sahneye çağır
                GameObject hexGameobject = (GameObject)Instantiate(hexPrefab[Random.Range(0,hexPrefab.Length)], new Vector3(xPos,y*yOffset,0), Quaternion.identity);

                hexGameobject.name = "Hex" + x + "_" + y;
                hexGameobject.GetComponent<Hex>().x = x;
                hexGameobject.GetComponent<Hex>().y = y;

                hexGameobject.transform.SetParent(this.transform);

            }

        }
    }

}
