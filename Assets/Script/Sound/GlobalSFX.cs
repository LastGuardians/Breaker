using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalSFX : MonoBehaviour
{
    [Header("Sound")]
    public AudioClip jumpSound; // 점프 사운드.
    public AudioClip destroySound; // 블록 파괴 사운드.
    public AudioClip weaponSwingSound; // 무기 스윙 사운드.
    public AudioClip collapseSound; // 플레이어 깔리는 사운드.
    public AudioClip shieldSound; // 방어 사운드.
    public AudioClip collisionSound; // 블럭 충돌 사운드.
    public AudioClip warningSound;  // 경고 사운드
    public AudioClip bombSound; // 폭탄 사운드
    public AudioClip objectShieldSound; // 방해사물 튕겨내는 사운드

    [Header("Button")]
    public GameObject SfxOnButton;
    public GameObject SfxOffButton;

    public AudioSource myAudio; //AudioSorce 컴포넌트 변수.
    public static GlobalSFX instance;

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

        myAudio = transform.Find("SFX").GetComponent<AudioSource>();
        StartCoroutine(StopSFXSound());
    }

    public IEnumerator StopSFXSound()
    {
        while(true)
        {
            yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Result");
            myAudio.Stop();
        }
    }
    
    public void SFXSoundOn()
    {
        Debug.Log("SFXSoundOn");
        myAudio.volume = 1;
    }

    public void SFXSoundOff()
    {
        Debug.Log("SFXSoundOff");
        myAudio.volume = 0;
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

    public void PlayCollapseSound()
    {
        myAudio.PlayOneShot(collapseSound);
    }

    public void PlayShieldSound()
    {
        myAudio.PlayOneShot(shieldSound);
    }

    public void PlayCollisionSound()
    {
        myAudio.PlayOneShot(collisionSound);
    }

    public void PlayWarningSound(bool on)
    {
        if (on)
            myAudio.PlayOneShot(warningSound);
        else
            myAudio.Stop();
    }

    public void PlayBombSound()
    {
        myAudio.PlayOneShot(bombSound);
    }

    public void PlayObjectSound()
    {
        myAudio.PlayOneShot(objectShieldSound);
    }
}