using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalBGM : MonoBehaviour
{
    [Header("AudioSource")]
    public AudioSource mainBGM;
    public AudioSource playBGM; //AudioSorce 컴포넌트 변수.
    //public AudioSource warningBGM;
    public AudioSource feverBGM;

    [Header ("AudioClip")]
    public AudioClip mainClip;
    public AudioClip playClip;
    public AudioClip warningClip;
    public AudioClip feverClip;

    [Header("Button")]
    public GameObject BgmOnButton;
    public GameObject BgmOffButton;

    public bool bgmOnButton = true;
    public bool feverBgmOn = false;
    public bool warningBgmOn = false;
    AudioSource[] myAudio = new AudioSource[3];

    public static GlobalBGM instance = null;


    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            //잘못된 인스턴스를 가르키고 있을 경우
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
        StartCoroutine(SetBGM1());
        StartCoroutine(SetBGM2());
        StartCoroutine(SetBGM3());
    }

    public IEnumerator SetBGM1()
    {
        while (true)
        {
            yield return new WaitUntil(() => (SceneManager.GetActiveScene().name.Equals("Main") ||
            SceneManager.GetActiveScene().name.Equals("Weapon") || SceneManager.GetActiveScene().name.Equals("Character") ||
            SceneManager.GetActiveScene().name.Equals("SpecificCharacter") || SceneManager.GetActiveScene().name.Equals("Credit") ||
            SceneManager.GetActiveScene().name.Equals("Tutorial") || SceneManager.GetActiveScene().name.Equals("Loading")));
            if (bgmOnButton)
            {
                if (!mainBGM.isPlaying)
                    mainBGM.Play();

                if (playBGM.isPlaying)
                    playBGM.Stop();

                if (feverBGM.isPlaying)
                    feverBGM.Stop();
            }
        }
    }

    public IEnumerator SetBGM2()
    {
        while (true)
        {
            yield return new WaitUntil(() => SceneManager.GetActiveScene().name.Equals("GamePlay"));
            if (bgmOnButton)
            {
                if (!playBGM.isPlaying && !feverBgmOn)
                    playBGM.Play();

                if (!feverBgmOn)
                    feverBGM.Stop();
                else if (feverBgmOn && !feverBGM.isPlaying)
                {
                    // Debug.Log("feverBgmOn: " + feverBgmOn);
                    feverBGM.Play();
                    playBGM.Stop();
                }

                if (mainBGM.isPlaying)
                    mainBGM.Stop();
            }
        }
    }

    public IEnumerator SetBGM3()
    {
        while (true)
        {
            yield return new WaitUntil(() => SceneManager.GetActiveScene().name.Equals("Result"));
            if (playBGM.isPlaying)
                playBGM.Stop();

            GlobalSFX.instance.PlayWarningSound(false);
        }
    }

    public void BGMSoundOn()
    {
        //Debug.Log("BGMSoundOn");
        bgmOnButton = true;
    }

    public void BGMSoundOff()
    {
        //Debug.Log("BGMSoundOff");
        bgmOnButton = false;

        mainBGM.Stop();
        playBGM.Stop();
        feverBGM.Stop();
    }
    
}
