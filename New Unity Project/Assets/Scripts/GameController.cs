using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CheckHexs()
    {
        StartCoroutine(check());
    }


    IEnumerator check()
    {
        float startX = 0.595f;
        float startY = 0.420f;

        float xOffset = 1.195f;
        float yOffset = 1.4059f;

        for (int x = 0; x < Map.widht - 1; x++)
        {
            for (int y = 0; y < Map.height - 1; y++)
            {
                float xPos = startX + (x * xOffset);
                float yPos = startY + (y * yOffset);

                transform.position = new Vector3(xPos, yPos, 0);
                yield return new WaitForSeconds(1f);
                
            }
        }
    }

    List<Hex> selectedHexs = new List<Hex>();

    private void OnTriggerEnter2D(Collider2D collision)
    {


        Debug.Log(collision.name);
        

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
