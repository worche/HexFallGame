using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTool : MonoBehaviour
{
    public List<Hex> selectedHexs = new List<Hex>();
    Vector2 pos = new Vector2(0, 0);
    int countHex = 0;
    [HideInInspector] public bool releaseTriggers = false;

    public enum selectToolState
    {
        up,
        down,
    }
    public selectToolState state = selectToolState.up;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (releaseTriggers)
        {
            PositionUpdate();
            if (!selectedHexs.Contains(collision.gameObject.GetComponent<Hex>()))
            {
                selectedHexs.Add(collision.gameObject.GetComponent<Hex>());
                
            }

            if (selectedHexs.Count == 3)
            {
                Rotate();
                releaseTriggers = false;
            }
        }
        
    }
    public void Clear()
    {
        releaseTriggers = false;

        foreach (var hex in selectedHexs)
        {
            hex.transform.SetParent(GameObject.FindGameObjectWithTag("map").transform);
            hex.GetComponent<SpriteRenderer>().sortingLayerName = "map";
        }

        selectedHexs.Clear();
    }

    void PositionUpdate()
    {
        pos = new Vector2(transform.position.x, transform.position.y);
    }

    void Rotate()
    {
        Quaternion rot = new Quaternion();
        int[] y = new int[3];
        int i = 0;
        foreach (var hex in selectedHexs)
        {
            hex.transform.SetParent(this.transform);
            hex.GetComponent<SpriteRenderer>().sortingLayerName="select";
            y[i] = hex.y;
            i++;
        }

        if (isRotate(y))
        {
            rot = Quaternion.Euler(0, 0, 0);
            state = selectToolState.up;
        }
        else
        {
            rot = Quaternion.Euler(0, 0, 60);
            state = selectToolState.down;
        }
        transform.GetChild(0).transform.rotation = rot;

        
    }
    bool isRotate(int[] y)
    {
        if (y[0] == y[1])
        {
            if (y[2] > y[0])
                return true;
            return false;

        }
        else if (y[1] == y[2])
        {
            if (y[0] > y[1])
                return true;
            return false;

        }
        else if (y[0] == y[2])
        {
            if (y[0] < y[1])
                return true;
            return false;
        }
        else {
            Debug.Log("error");
            return false; }
    }
}
