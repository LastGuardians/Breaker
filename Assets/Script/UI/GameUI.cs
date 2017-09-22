using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {

    [Header("Text")]
    private Text Score;
    private Text BestScore;

    [Header("Button")]
    public GameObject PlayButton;
    public GameObject PauseButton;
    public GameObject BgmOnButton;
    public GameObject BgmOffButton;
    public GameObject SfxOnButton;
    public GameObject SfxOffButton;

    GameObject bgmObj;

    void Start()
    {
        bgmObj = GameObject.Find("GlobalBGM").GetComponent<GlobalBGM>().gameObject;

        if(!GlobalBGM.instance.bgmOnButton) // BGM이 off 되었다면
        {
            BgmOnButton.SetActive(true);
            BgmOffButton.SetActive(false);
        }
        if(GlobalSFX.instance.myAudio.volume == 0)  // 효과음이 off 되었다면
        {
            SfxOnButton.SetActive(true);
            SfxOffButton.SetActive(false);
        }
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
        GameObject.Find("GlobalBGM").GetComponent<GlobalBGM>().BGMSoundOn();
    }

    public void BGMOff()
    {
        GameObject.Find("GlobalBGM").GetComponent<GlobalBGM>().BGMSoundOff();
    }

    public void SFXOn()
    {
        GameObject.Find("GlobalSFX").GetComponent<GlobalSFX>().SFXSoundOn();
    }

    public void SFXOff()
    {
        GameObject.Find("GlobalSFX").GetComponent<GlobalSFX>().SFXSoundOff();
    }
}
