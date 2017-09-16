using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class GlobalBGM : MonoBehaviour
{
    AudioSource playBGM; //AudioSorce 컴포넌트 변수.
    AudioSource warningBGM;
    AudioSource feverBGM;

    public AudioClip playClip;
    public AudioClip warningClip;
    public AudioClip feverClip;

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

        playBGM = this.gameObject.GetComponent<AudioSource>(); //AudioSource 오브젝트를 변수로 담습니다.              
        //playBGM.clip = playClip;
        //warningBGM = gameObject.GetComponent<AudioSource>();
        //warningBGM.clip = warningClip;
        //feverBGM = gameObject.GetComponent<AudioSource>();
        //feverBGM.clip = feverClip;

        PlayBGM();

        myAudio[0] = playBGM;
        myAudio[1] = warningBGM;
        myAudio[2] = feverBGM;
    }

    public void BGMSoundOn()
    {
        //for (int i = 0; i < 3; ++i)
        //{
        //    myAudio[i].Play();
        //}
        playBGM.Play();
    }

    public void BGMSoundOff()
    {
        //for (int i = 0; i < 3; ++i)
        //{
        //    myAudio[i].Stop();
        //}
        playBGM.Stop();
    }

    public void PlayBGM()
    {
        //playBGM.PlayOneShot(playClip);
        playBGM.Play();
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
