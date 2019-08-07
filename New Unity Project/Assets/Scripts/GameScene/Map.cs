using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour 
{
    //yükseklik ve genişlik
    [SerializeField] static public int widht = 8; 
    [SerializeField] static public int height = 13;
    public GameObject[] hexPrefab;

    //mapin altıgenlerden oluşması için manuel bir offset değeri veriyoruz
    float xOffset = 1.190f;
    float yOffset = 1.056f;


    private void Start()
    {
        int random = Random.Range(0, hexPrefab.Length); 
        for (int x = 0; x < widht; x++)
        {
            for (int y = 0; y < height; y++)

            {
                int lastHexRandomColor = random;

                do
                    random = Random.Range(0, hexPrefab.Length);
                while (lastHexRandomColor == random); //ilk baştaki eşleşmelerden kurtulmak için peşpeşe 2 random sayının aynı olmamasını sağlar.

                float xPos = x*xOffset;
                if (y % 2 == 1)
                {
                    xPos += xOffset/2f;
                }
                
                
                GameObject hexGameobject = (GameObject)Instantiate(hexPrefab[random], new Vector3(xPos,25,0), Quaternion.identity);
                //rastgele bir objeyi offset değerlerini kullanarak sahneye çağır


                //hexin kendi scriptine bu bilgileri daha sonra kullanmak üzere atar.
                hexGameobject.GetComponent<Hex>().id = random;
                hexGameobject.name = "Hex" + x + "_" + y;
                hexGameobject.GetComponent<Hex>().x = x;
                hexGameobject.GetComponent<Hex>().y = y;
                hexGameobject.GetComponent<Hex>()._x = xPos;
                hexGameobject.GetComponent<Hex>()._y = y * yOffset;

                hexGameobject.transform.SetParent(this.transform);
            }
        }
    }
    public Hex HexInstantiate(Hex hex,int id,Transform parent=null)//bir örnek hexin konumuna ve adına sahip yeni bir hex yaratır. ve o hexi geri döndürür.
    {
        GameObject hexGameobject = (GameObject)Instantiate(hexPrefab[id], new Vector3(hex._x, hex._y, 0), Quaternion.identity);

        hexGameobject.GetComponent<Hex>().id = id;
        hexGameobject.name = hex.name;
        hexGameobject.GetComponent<Hex>().x = hex.x;
        hexGameobject.GetComponent<Hex>().y = hex.y;
        hexGameobject.GetComponent<Hex>()._x = hex._x;
        hexGameobject.GetComponent<Hex>()._y = hex._y;
        if (parent == null)
        {
            parent = this.transform;
        }
        hexGameobject.transform.SetParent(parent);
        hexGameobject.GetComponent<Hex>().PlayInstantiateAnim();
        return hexGameobject.GetComponent<Hex>();


    }


}
