using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    float gameOverTimer = 120f; // 1 dakika;
    public Text timeValue;
    public Text scoreValue;
   float timerChangeAmount=1f;

    int score = 0;

    public int Score
    {
        get
        {
            return score;
        }

        set
        {
            if (score >= 10)
            {
                timerChangeAmount = 2f;//süre daha hızlı akıyor
            }
            
            score = value;
            scoreValue.text = score.ToString();
        }
    }

    void Start()
    {
        timeValue.text =gameOverTimer + " s";
        scoreValue.text = "0";
        StartCoroutine(GameOverTimer());
    }

    IEnumerator GameOverTimer()
    {
        while (gameOverTimer > 0)
        {

            yield return new WaitForSeconds(1f);//1 saniyede süreyi bir düşür.
            gameOverTimer -= timerChangeAmount;
            timeValue.text = gameOverTimer + " s";
            if (gameOverTimer <= 0)
            {
                GameOver();
            }
        }
    }

    public void GameOver()
    {

        PlayerPrefs.SetInt("score", score);//score'u daha sonra okumak üzere kaydeder.
        GetComponent<SceneManagerSc>().LoadNextScene();
    }
}
