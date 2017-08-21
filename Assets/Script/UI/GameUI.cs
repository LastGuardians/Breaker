using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    private Text Score;
    private Text BestScore;

    public GameObject PlayButton;
    public GameObject PauseButton;

    //public void OnMouseDown()
    //{
    //    Debug.Log("공격 버튼 down");
    //    PlayerManager.instance.attackOn = true;
    //}

    //public void OnMouseUp()
    //{
    //    Debug.Log("공격 버튼 up");
    //    PlayerManager.instance.attackOn = false;
    //}
    

    public void Pause()
    {
        Time.timeScale = 0;
        PlayButton.SetActive(true);
        PauseButton.SetActive(false);
    }

    public void Play()
    {
        Time.timeScale = 1;
        PlayButton.SetActive(false);
        PauseButton.SetActive(true);
    }
}
