using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour 
{
    //yükseklik ve genişlik
    [SerializeField] static public int widht = 8; 
    [SerializeField] static public int height = 9;
    public GameObject[] hexPrefab;

    //mapin altıgenlerden oluşması için manuel bir offset değeri veriyoruz
    float xOffset = 1.190f;
    float yOffset = 1.056f;

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
                GameObject hexGameobject = (GameObject)Instantiate(hexPrefab[Random.Range(0,hexPrefab.Length)], new Vector3(xPos,20,0), Quaternion.identity);

                hexGameobject.name = "Hex" + x + "_" + y;
                hexGameobject.GetComponent<Hex>().x = x;
                hexGameobject.GetComponent<Hex>().y = y;
                hexGameobject.GetComponent<Hex>()._x = xPos;
                hexGameobject.GetComponent<Hex>()._y = y * yOffset;

                hexGameobject.transform.SetParent(this.transform);

            }

        }
    }

}
