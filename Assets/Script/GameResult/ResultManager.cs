using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    public GameObject scoreText;
    public GameObject coinText;
    public GameObject keyText;

    public int score = 0;
    public float coin = 0;
    public float key = 0;

    public static ResultManager instance = null;

    private void Start()
    {
        //Debug.Log("result score : " + score);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            //잘못된 인스턴스를 가르키고 있을 경우
            Destroy(gameObject);
        }
        scoreText = GameObject.Find("Score");

        BlockGenerator.instance.game_start = false;
    }

    public void ResultScore(int iScore)
    {
        scoreText.GetComponent<Text>().text = iScore.ToString();
       // scoreText = GameObject.Find("Score");       
    }

    public void ResultCoin(float fCoin)
    {
        coinText.GetComponent<Text>().text = fCoin.ToString();
    }

    public void ResultKey(float fKey)
    {
        keyText.GetComponent<Text>().text = fKey.ToString();
    }

    public void MainScene()
    {
        SceneManager.LoadScene("Main");
        Time.timeScale = 1;

        //Destroy(GameManager.instance);
        //Destroy(GameManager.instance.gameObject);  // 임시방편. 수정 필요.
    }
    
    public void PlayScene()
    {
        SceneManager.LoadScene("GamePlay");
        Time.timeScale = 1;
    }
}