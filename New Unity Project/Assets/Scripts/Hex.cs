using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{

    public int x; // diğer hexlere göre konumu
    public int y;

    public float _x; //kordinat sistemine göre konumu
    public float _y;

    public int id;

    float centerX, centerY; // son kordinatı


    public Vector2[] corners = new Vector2[6];
    // Start is called before the first frame update
    Transform left, right, leftDown = null, leftUp = null, rightDown = null, rightUp = null;

    Transform[] cornerHex = new Transform[6]; 


     public void SetNeighbours()
    {
        centerX = transform.position.x;
        centerY = transform.position.y;

        

        try
        {
            cornerHex[0] = FindNeighbours(x - 1, y); //left

            cornerHex[3] = FindNeighbours(x + 1, y); // right


            //her altıgen etrafındaki altıgenlerin isimlerini tutuyor.
            if (y % 2 == 0)
            {
                cornerHex[4] = FindNeighbours(x , y-1); //rightDown
                cornerHex[5] = FindNeighbours(x - 1, y-1); //leftDown

                cornerHex[2] = FindNeighbours(x, y+1); //rightUp
                cornerHex[1] = FindNeighbours(x - 1, y+1); //leftUp

            }
            else if (y % 2 == 1)
            {
                cornerHex[4] = FindNeighbours(x + 1, y-1); //rightDown
                cornerHex[5] = FindNeighbours(x, y-1); //leftDown

                cornerHex[2] = FindNeighbours(x +1, y+1); //rightUp
                cornerHex[1] = FindNeighbours(x , y+1); //leftUp

            }


            //Altıgen kendi köşelerini etrafındaki diğer altıgenlere göre belirliyor. 
            //bu şekilde kesin olarak seçim aracı oraya yerleşebiliyor.
            //etrafındaki diğer iki altıgenin markezi ile kendi merkezi arasında bir üçgen belirleniyor.
            //o üçgenin aırlık merkezi kesin olarak köşe noktası oluyor.

            corners[0] = CenterOfGravity(cornerHex[0].position, cornerHex[1].position);
            corners[1] = CenterOfGravity(cornerHex[0].position, cornerHex[5].position);
            corners[2] = CenterOfGravity(cornerHex[1].position, cornerHex[2].position);
            corners[3] = CenterOfGravity(cornerHex[2].position, cornerHex[3].position);
            corners[4] = CenterOfGravity(cornerHex[3].position, cornerHex[4].position);
            corners[5] = CenterOfGravity(cornerHex[4].position, cornerHex[5].position);

        }
        catch {
            Debug.Log("catch");
        }
    }

    public void CheckNeigboursColor() //altıgenin tüm köşelerinde bulunan altıgenler ile aynı renkte mi
    {
        for (int i = 0; i < 6; i++)
        {
            try
            {
                if (i != 5)
                {
                    if (cornerHex[i].gameObject.GetComponent<Hex>().id == this.id && cornerHex[i + 1].gameObject.GetComponent<Hex>().id == this.id)
                    {
                        Destroy(cornerHex[i].gameObject);
                        Destroy(cornerHex[i + 1].gameObject);
                        Destroy(this.gameObject);
                        return;
                    }
                }
                else
                {
                    if (cornerHex[i].gameObject.GetComponent<Hex>().id == this.id && cornerHex[0].gameObject.GetComponent<Hex>().id == this.id)
                    {
                        Destroy(cornerHex[0].gameObject);
                        Destroy(cornerHex[i].gameObject);
                        Destroy(this.gameObject);

                        return;
                    }
                }
            }
            catch { }
        }
    }

    Transform FindNeighbours(int __x, int __y)
    {
        Transform temp;
        if (__x>=0 && __y>=0 && __x< Map.widht && __y < Map.height)
            temp = GameObject.Find("Hex" + __x + "_" + __y).GetComponent<Transform>();
        else
        {

            temp = GameObject.FindGameObjectWithTag("Maptool").transform;

        }
            

        return temp;
    }
    // eşkenar üçgen agırlık merkezi formülü
    private Vector2 CenterOfGravity(Vector2 h1, Vector2 h2)
    {
        float x = (h1.x + h2.x + centerX) / 3f;
        float y = (h1.y + h2.y + centerY) / 3f;
        return new Vector2(x, y);
    }
    public bool isFalling = true;

    private void Update()
    {
        if (isFalling)
        {
            if (transform.position.y - _y < 0.1f)
            {
                isFalling = false;
                transform.position = new Vector3(_x, _y, 0);
                SetNeighbours();
            }
            transform.position = Vector2.Lerp(transform.position, new Vector2(_x, _y), Time.deltaTime * 1.5f);

        }
    }



}
