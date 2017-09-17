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

    GameObject bgmOnButtonObj;

    public bool bgmOnButton = true;

    AudioSource[] myAudio = new AudioSource[3];

    public static GlobalBGM instance;

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

        myAudio[0] = mainBGM;
        myAudio[1] = playBGM;
        myAudio[2] = feverBGM;
    }

    

    void Update()
    {
        //bgmOnButtonObj = 
        //Debug.Log("bgmOnButton : " + bgmOnButton);
        if ((SceneManager.GetActiveScene().name == "Main" ||
           SceneManager.GetActiveScene().name == "Weapon" ||
           SceneManager.GetActiveScene().name == "Character" ||
           SceneManager.GetActiveScene().name == "SpecificCharacter")
           && bgmOnButton)
        {
            //Debug.Log("메인씬");
            if(!mainBGM.isPlaying)
                mainBGM.Play();

            if(playBGM.isPlaying)
                playBGM.Stop();
            if(feverBGM.isPlaying)
                feverBGM.Stop();
        }
        else if(SceneManager.GetActiveScene().name == "GamePlay"
            && bgmOnButton)
        {
            if (!playBGM.isPlaying)
                playBGM.Play();

            if (feverBGM.isPlaying)
                feverBGM.Stop();
            if (mainBGM.isPlaying)
                mainBGM.Stop();
        }

    }

    public void BGMSoundOn()
    {
        Debug.Log("BGMSoundOn");
        bgmOnButton = true;
    }

    public void BGMSoundOff()
    {
        Debug.Log("BGMSoundOff");
        bgmOnButton = false;

        mainBGM.Stop();
        playBGM.Stop();
        feverBGM.Stop();
        //mainBGM.mute = true;
        //playBGM.mute = true;
        //feverBGM.mute = true;
    }

    public void PlayBGM()
    {
        //myAudio[1].Play();
        //playBGM.PlayOneShot(playClip);
        //playBGM.Play();
        //feverBGM.Stop();
        //warningBGM.Stop();
    }

    public void WarningBGM()
    {
        //playBGM
        //warningBGM.Play();
        //playBGM.Stop();
    }

    public void FeverBGM()
    {
        //feverBGM.Play();
        //playBGM.Stop();
        //warningBGM.Stop();
    }
}
