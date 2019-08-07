using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    Camera cam;
    Color lerpedColor = Color.white;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
    void Update()//arkaplan rengini göze hoşgelmesi amacıyla değiştirir.
    {
        lerpedColor = Color.Lerp(new Color(0.389f, 0.630f, 0.886f,0.8f), new Color(0.971f, 0.357f, 0.297f, 1f), Mathf.PingPong(Time.time, 1.3f));
        cam.backgroundColor = lerpedColor;
    }
}
