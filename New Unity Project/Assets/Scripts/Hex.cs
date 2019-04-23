using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{

    public int x; // diğer hexlere göre konumu
    public int y;

    public float _x; //kordinat sistemine göre konumu
    public float _y;



    float centerX, centerY; // son kordinatı


    public Vector2[] corners = new Vector2[6];
    // Start is called before the first frame update

    private void Start()
    {


    }

    void SetNeighnours()
    {
        centerX = transform.position.x;
        centerY = transform.position.y;

        Transform left, right, leftDown = null, leftUp = null, rightDown = null, rightUp = null;

        try
        {
            left = FindNeighbours(x - 1, y);

            right = FindNeighbours(x + 1, y);


            //her altıgen etrafındaki altıgenlerin isimlerini tutuyor.
            if (y % 2 == 0)
            {
                rightDown = FindNeighbours(x , y-1);
                leftDown = FindNeighbours(x - 1, y-1);

                rightUp = FindNeighbours(x, y+1);
                leftUp = FindNeighbours(x - 1, y+1);

            }
            else if (y % 2 == 1)
            {
                rightDown = FindNeighbours(x + 1, y-1);
                leftDown = FindNeighbours(x, y-1);

                rightUp = FindNeighbours(x +1, y+1);
                leftUp = FindNeighbours(x , y+1);

            }


            //Altıgen kendi köşelerini etrafındaki diğer altıgenlere göre belirliyor. 
            //bu şekilde kesin olarak seçim aracı oraya yerleşebiliyor.
            //etrafındaki diğer iki altıgenin markezi ile kendi merkezi arasında bir üçgen belirleniyor.
            //o üçgenin aırlık merkezi kesin olarak köşe noktası oluyor.

            corners[0] = CenterOfGravity(left.position, leftUp.position);
            corners[1] = CenterOfGravity(left.position, leftDown.position);
            corners[2] = CenterOfGravity(leftUp.position, rightUp.position);
            corners[3] = CenterOfGravity(rightUp.position, right.position);
            corners[4] = CenterOfGravity(right.position, rightDown.position);
            corners[5] = CenterOfGravity(rightDown.position, leftDown.position);

        }
        catch {
            Debug.Log("catch");
        }
    }

    Transform FindNeighbours(int __x, int __y)
    {
        Transform temp;
        if (__x>=0 && __y>=0 && __x< Map.widht && __y < Map.height)
            temp = GameObject.Find("Hex" + __x + "_" + __y).GetComponent<Transform>();
        else
        {
            GameObject gameObject = new GameObject();
            gameObject.transform.position = new Vector3(200, 200, 0);
            temp = gameObject.transform;

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
    bool isFalling = true;

    private void Update()
    {
        if (isFalling)
        {
            if (transform.position.y - _y < 0.1f)
            {
                isFalling = false;
                transform.position = new Vector3(_x, _y, 0);
                SetNeighnours();
            }
            transform.position = Vector2.Lerp(transform.position, new Vector2(_x, _y), Time.deltaTime * 3f);

        }
    }



}
