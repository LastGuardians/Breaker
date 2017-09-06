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
}
