using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip jumpSound; // 점프 사운드.
    public AudioClip destroySound; // 블록 파괴 사운드.
    public AudioClip weaponSwingSound; // 무기 스윙 사운드.

    AudioSource myAudio; //AudioSorce 컴포넌트 변수.
    public static SoundManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        myAudio = this.gameObject.GetComponent<AudioSource>(); //AudioSource 오브젝트를 변수로 담습니다.
    }

    public void PlayJumpSound()
    {
        myAudio.PlayOneShot(jumpSound); //소리를 한번만 재생.
    }

    public void PlayDestroySound()
    {
        myAudio.PlayOneShot(destroySound);
    }

    public void PlayWeaponSwingSound()
    {
        myAudio.PlayOneShot(weaponSwingSound);
    }
}