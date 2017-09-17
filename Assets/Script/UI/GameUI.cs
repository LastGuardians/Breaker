using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {

    private Text Score;
    private Text BestScore;

    public GameObject PlayButton;
    public GameObject PauseButton;

    GameObject bgmObj;

    void Start()
    {
        bgmObj = GameObject.Find("GlobalBGM").GetComponent<GlobalBGM>().gameObject;
    }

    public void MainScene()
    {
        SceneManager.LoadScene("Main");
        Time.timeScale = 1;
    }

    public void PlayScene()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void Pause()
    {
        PlayButton.SetActive(true);
        PauseButton.SetActive(false);
        Time.timeScale = 0;
    }

    public void Play()
    {
        PlayButton.SetActive(false);
        PauseButton.SetActive(true);
        Time.timeScale = 1;
    }

    public void BGMOn()
    {
        //GameObject.Find("GlobalBGM").GetComponent<GlobalBGM>().bgmOnButton = true;

        GameObject.Find("GlobalBGM").GetComponent<GlobalBGM>().BGMSoundOn();
    }

    public void BGMOff()
    {
        //GameObject.Find("GlobalBGM").GetComponent<GlobalBGM>().bgmOnButton= false;

        GameObject.Find("GlobalBGM").GetComponent<GlobalBGM>().BGMSoundOff();
    }
}
