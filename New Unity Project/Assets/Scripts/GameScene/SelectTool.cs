using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTool : MonoBehaviour
{
    public List<Hex> selectedHexs = new List<Hex>();


    Vector2 pos = new Vector2(-5, -5);
    int countHex = 0;
    [HideInInspector] public bool releaseTriggers = false;

    public enum selectToolState
    {
        up,
        down,
    }
    public selectToolState state = selectToolState.up;

    private void OnTriggerStay2D(Collider2D collision) //seçim aletinin collisionunun değdiği triggerleri seçer.
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
    public void Clear() //bir sonraki seçimde seçili hexleri temizler
    {
        releaseTriggers = false;

        foreach (var hex in selectedHexs)//bırakılan hexleri map'a taşır.
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
        int[] y = new int[3];
        int i = 0;
        foreach (var hex in selectedHexs)//seçilen hexleri child'ı olarak ayarlar select layer'ına atar
        {
            hex.transform.SetParent(this.transform);
            hex.GetComponent<SpriteRenderer>().sortingLayerName = "select";
            y[i] = hex.y;
            i++;
        }


    }

}
