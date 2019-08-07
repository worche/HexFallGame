using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverSc : MonoBehaviour
{
    public Text score;
    void Start()
    {
        if(PlayerPrefs.HasKey("score"))//score kaydı varsa score Text değerine atar.
        score.text = PlayerPrefs.GetInt("score").ToString();
    }

}
