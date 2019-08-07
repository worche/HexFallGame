using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerSc : MonoBehaviour
{
    public static SceneManagerSc instance;
    public void LoadNextScene()//bir sonraki sahneyi çağırır
    {
        int y = SceneManager.GetActiveScene().buildIndex;

        if (y == SceneManager.sceneCountInBuildSettings-1)//son sahneyse başa alır.
            y = -1;
        SceneManager.LoadScene(y + 1);
    }

}
