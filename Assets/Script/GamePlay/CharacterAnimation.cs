using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public Animator catAni;

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

        catAni = gameObject.GetComponent<Animator>();
       
    }

    // idle 상태에서 공격했을 때 애니메이션
    public void CatAttackAniControll()
    {
       // Debug.Log("attack : " + attack);
        //catAni.SetBool("IsAttack", attack);
        catAni.SetTrigger("IsAttack");
    }

    // 점프 애니메이션
    public void CatJumpAniControll()
    {
        //Debug.Log("jump : " + jump);
        catAni.SetTrigger("IsJump");
        //catAni.SetTrigger("IsIdleJump");
    }

    // 점프 상태에서 공격했을 때 애니
    public void CatJumpAttackAniControll()
    {
        catAni.SetTrigger("IsJumpAttack");
    }
}