using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{

    public int x;
    public int y;

    public float centerX, centerY;
    public Vector2[] corners=new Vector2[6];
    // Start is called before the first frame update

    private void Start()
    {

        centerX = transform.position.x;
        centerY = transform.position.y;

        Transform left, right, leftDown=null, leftUp=null, rightDown=null, rightUp =null;
        try
        {
             left = GameObject.Find("Hex" + (x - 1) + "_" + y).GetComponent<Transform>();
             right = GameObject.Find("Hex" + (x + 1) + "_" + y).GetComponent<Transform>();

            if (x%2==0 && y%2==0)
            {
                rightDown = GameObject.Find("Hex" + (x ) + "_" + (y - 1)).GetComponent<Transform>();
                leftDown = GameObject.Find("Hex" + (x-1) + "_" + (y - 1)).GetComponent<Transform>();

                rightUp = GameObject.Find("Hex" + (x ) + "_" + (y + 1)).GetComponent<Transform>();
                leftUp = GameObject.Find("Hex" + (x-1) + "_" + (y + 1)).GetComponent<Transform>();

            }else if( x % 2 == 1 && y%2==1)
            {
                rightDown = GameObject.Find("Hex" + (x+1) + "_" + (y - 1)).GetComponent<Transform>();
                leftDown = GameObject.Find("Hex" + (x ) + "_" + (y - 1)).GetComponent<Transform>();

                rightUp = GameObject.Find("Hex" + (x+1) + "_" + (y + 1)).GetComponent<Transform>();
                leftUp = GameObject.Find("Hex" + (x ) + "_" + (y + 1)).GetComponent<Transform>();

            }
            else if(x%2==1 && y % 2 == 0)
            {
                rightDown = GameObject.Find("Hex" + (x) + "_" + (y - 1)).GetComponent<Transform>();
                leftDown = GameObject.Find("Hex" + (x-1) + "_" + (y - 1)).GetComponent<Transform>();

                rightUp = GameObject.Find("Hex" + (x ) + "_" + (y + 1)).GetComponent<Transform>();
                leftUp = GameObject.Find("Hex" + (x-1) + "_" + (y + 1)).GetComponent<Transform>();
            }
            else if(x % 2 == 0 && y % 2 == 1)
            {
                rightDown = GameObject.Find("Hex" + (x+1) + "_" + (y - 1)).GetComponent<Transform>();
                leftDown = GameObject.Find("Hex" + (x ) + "_" + (y - 1)).GetComponent<Transform>();
                rightUp = GameObject.Find("Hex" + (x+1) + "_" + (y + 1)).GetComponent<Transform>();
                leftUp = GameObject.Find("Hex" + (x) + "_" + (y + 1)).GetComponent<Transform>();

            }


            corners[0] = CenterOfGravity(left.position, leftUp.position);
            corners[1] = CenterOfGravity(left.position, leftDown.position);
            corners[2] = CenterOfGravity(leftUp.position, rightUp.position);
            corners[3] = CenterOfGravity(rightUp.position, right.position);
            corners[4] = CenterOfGravity(right.position, rightDown.position);
            corners[5] = CenterOfGravity(rightDown.position, leftDown.position);

        }
        catch { }
    }
    private Vector2 CenterOfGravity(Vector2 h1,Vector2 h2)
    {
        float x = (h1.x + h2.x + centerX) / 3f;
        float y= (h1.y + h2.y + centerY) / 3f;
        return new Vector2(x, y);
    }


}
