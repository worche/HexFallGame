using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{


    public int x; // diğer hexlere göre konumu
    public int y;

    public float _x; //kordinat sistemine göre konumu
    public float _y;

    public int id;//renk , random sayı bilgisi

    float centerX, centerY; // son kordinatı

    public Vector2[] corners = new Vector2[6];
    // Start is called before the first frame update
    Transform left, right, leftDown = null, leftUp = null, rightDown = null, rightUp = null;

    Transform[] cornerHex = new Transform[6];

    Vector2[] castDir = new Vector2[7];

    private void Start()
    {
        castDir[0] = new Vector2(1, 1.732050f).normalized;//30 derece , 1kök3 e tekabül kordinat
        castDir[1] = Vector2.right;// 30+60 = 90 derece
        castDir[2] = new Vector2(1, -1.732050f).normalized;//90 + 60 =150 derece
        castDir[3] = new Vector2(-1, -1.732050f).normalized;//150 + 60 =210 
        castDir[4] = -Vector2.right;//210+60 = 270
        castDir[5] = new Vector2(-1, 1.732050f).normalized;//270+60 = 330 
        castDir[6] = new Vector2(1, 1.732050f).normalized;//30 derece 
        //330 +60 = 30 derece , Hexagonların tüm kenarlarından bir raycast göndereceğiz
    }

    public void SetCorners()//seçim aracını yerleştirmek için köşe noktalarını tutar.
    {
        try
        {
            corners[0] = transform.GetChild(0).transform.position;
            corners[1] = transform.GetChild(1).transform.position;
            corners[2] = transform.GetChild(2).transform.position;
            corners[3] = transform.GetChild(3).transform.position;
            corners[4] = transform.GetChild(4).transform.position;
            corners[5] = transform.GetChild(5).transform.position;
        }
        catch
        {
            Debug.Log("catch");
        }
    }

    public List<Hex> FindMatchHex() //
    {


        GetComponent<PolygonCollider2D>().enabled = false; //kendi collider bilgisini bulmamak için bu süreçte kapatır.
        List<Hex> matchingHex = new List<Hex>();
        for (int i = 0; i < castDir.Length - 1; i++)//6 yönde ışın yollar.
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir[i]);
            if (hit.collider != null && hit.collider.GetComponent<Hex>().id == id && !matchingHex.Contains(hit.collider.GetComponent<Hex>()))
            {// ışının çarptığı hex id si bu hex ile aynıysa +60 -60 yönünde 2 ışın daha yollar bu ışınlarda bu iki id ile eşitse bu 3 hexi listeye ekler.
                RaycastHit2D hit3;
                RaycastHit2D hit2 = Physics2D.Raycast(transform.position, castDir[i + 1]);
                if (i == 0)
                { hit3 = Physics2D.Raycast(transform.position, castDir[5]); }
                else
                { hit3 = Physics2D.Raycast(transform.position, castDir[i - 1]); }

                if (hit2.collider != null && hit2.collider.GetComponent<Hex>().id == id && !matchingHex.Contains(hit2.collider.GetComponent<Hex>()))
                {
                    matchingHex.Add(hit2.collider.GetComponent<Hex>());
                    matchingHex.Add(hit.collider.GetComponent<Hex>());
                }
                else if (hit3.collider != null && hit3.collider.GetComponent<Hex>().id == id && !matchingHex.Contains(hit3.collider.GetComponent<Hex>()))
                {
                    matchingHex.Add(hit3.collider.GetComponent<Hex>());
                    matchingHex.Add(hit.collider.GetComponent<Hex>());
                }


            }
        }
        matchingHex.Add(this.GetComponent<Hex>());//kendisini eklediği kısım.
        GetComponent<PolygonCollider2D>().enabled = true;// geri colliderı aktif eder.
        return matchingHex;
    }

    public bool isFalling = true;

    private void Update()
    {
        if (isFalling)
        {
            if (transform.position.y - _y < 0.05f)
            {
                if (transform.localScale.x >= 0.9f)//asıl boyutuna ulaşmadan köşelerini belirlemiyor
                {
                    isFalling = false;
                    transform.position = new Vector3(_x, _y, 0);
                    SetCorners();
                }
                
            }
            transform.position = Vector2.Lerp(transform.position, new Vector2(_x, _y), Time.deltaTime * 1.5f);//düşme efekti

        }
    }

    public void PlayInstantiateAnim()
    {
        GetComponent<Animator>().Play("Instantiate");
    }
    public void OnDestroy()
    {
        GetComponent<Animator>().Play("Destroy");
        Destroy(gameObject);
    }

}
