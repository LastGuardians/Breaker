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
    public AudioSource warningBGM;
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

        //BgmOnButton = GameObject.Find("SetupUI").transform.Find("BGMOnButton").gameObject;
        //BgmOffButton = GameObject.Find("Canvas").transform.Find("SetupUI").transform.Find("BGMOffButton").gameObject;
    }

    void Update()
    {
        //Debug.Log("bgmOnButton : " + bgmOnButton);
        if ((SceneManager.GetActiveScene().name == "Main" ||
           SceneManager.GetActiveScene().name == "Weapon" ||
           SceneManager.GetActiveScene().name == "Character" ||
           SceneManager.GetActiveScene().name == "SpecificCharacter" ||
           SceneManager.GetActiveScene().name == "Credit")
           && bgmOnButton)
        {
            //Debug.Log("메인씬");
            if(!mainBGM.isPlaying)
                mainBGM.Play();

            if(playBGM.isPlaying)
                playBGM.Stop();

            if(feverBGM.isPlaying)
                feverBGM.Stop();

            if (warningBGM.isPlaying)
                warningBGM.Stop();
        }
        //else if(SceneManager.GetActiveScene().name == "Main" &&
        //    !bgmOnButton)
        //{
        //    BgmOnButton.SetActive(true);
        //    BgmOffButton.SetActive(false);
        //}
        else if(SceneManager.GetActiveScene().name == "GamePlay"
            && bgmOnButton)
        {
            if (!playBGM.isPlaying && !feverBgmOn)
                playBGM.Play();

            if (!feverBgmOn)
                feverBGM.Stop();
            else if(feverBgmOn && !feverBGM.isPlaying)
            {
               // Debug.Log("feverBgmOn: " + feverBgmOn);
                feverBGM.Play();
                playBGM.Stop();
            }

            if (mainBGM.isPlaying)
                mainBGM.Stop();

            //if (!warningBGM.isPlaying && warningBgmOn && !feverBgmOn)
            //{
            //    playBGM.Stop();
            //    warningBGM.Play();
            //}
            //else
                warningBGM.Stop();
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
        warningBGM.Stop();
    }
    
}
