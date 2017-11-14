using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public Animator characterAni;
    public RuntimeAnimatorController catAni;
    public RuntimeAnimatorController alpacaAni;
    public RuntimeAnimatorController deerAni;
    public RuntimeAnimatorController frogAni;
    public RuntimeAnimatorController monkeyAni;
    float aniTime = 0;

    public static CharacterAnimation instance = null;

    void Awake()
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
        characterAni = gameObject.GetComponent<Animator>();
    }

    // idle 상태에서 공격했을 때 애니메이션
    public void CatAttackAniControll()
    {
        characterAni.SetTrigger("IsAttack");
    }

    // 점프 애니메이션
    public void CatJumpAniControll()
    {
        characterAni.SetTrigger("IsJump");
        //characterAni.SetTrigger("IsIdleJump");
    }

    // 점프 idle 애니메이션
    public void CatJumpIdleAniControll(float n)
    {
        characterAni.SetFloat("IsJumpIdle", n);
       
    }


    // 점프 상태에서 공격했을 때 애니
    public void CatJumpAttackAniControll()
    {
        characterAni.SetTrigger("IsJumpAttack");
    }

    public IEnumerator AniCor()
    {
        aniTime = 0;
        Debug.Log("AniCor 시작");
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            aniTime += 0.1f;
            Debug.Log("aniTime: " + aniTime);
            if(aniTime >= 0.3)
            {
                CatJumpIdleAniControll(11f);
                yield break;
            }
        }
    }
}