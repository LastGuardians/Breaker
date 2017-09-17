using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonCoolTime : MonoBehaviour
{
    public Image shieldFilter;      // 방어버튼 쿨타임 이미지

    public float coolTime = 1.0f;   // 방어 쿨타임

    private float currentCoolTime; //남은 쿨타임을 추적 할 변수

    private bool canUseShield = true; // 방어를 사용할 수 있는지 확인하는 변수

    public static ButtonCoolTime instance = null;

    void start()
    {
        shieldFilter.fillAmount = 0; //처음에 방어 버튼을 가리지 않음

    }

    public void UseShield()
    {
        if (canUseShield)
        {
            //Debug.Log("Use Shield");
            shieldFilter.fillAmount = 1; //방어 버튼을 가림
            // 블록을 위로 튕긴다.
            //PlayerManager.instance.col_parent.enabled = true;
            //PlayerManager.instance.blockRg.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
            //PlayerManager.instance.col_parent.enabled = false;
            StartCoroutine("Cooltime");

            currentCoolTime = coolTime;

            StartCoroutine("CoolTimeCounter");

            canUseShield = false; //방어를 사용하면 사용할 수 없는 상태로 바꿈
        }
        else
        {
            Debug.Log("아직 방어를 사용할 수 없습니다.");
        }
    }

    IEnumerator Cooltime()
    {
        while (shieldFilter.fillAmount > 0)
        {
            shieldFilter.fillAmount -= 1 * Time.smoothDeltaTime / coolTime;

            yield return null;
        }

        canUseShield = true; //방어 쿨타임이 끝나면 방어를 사용할 수 있는 상태로 바꿈

        yield break;
    }

    //남은 쿨타임을 계산할 코루틴을 만든다.
    IEnumerator CoolTimeCounter()
    {
        while (currentCoolTime > 0)
        {
            yield return new WaitForSeconds(1.0f);

            currentCoolTime -= 1.0f;
        }

        yield break;
    }
}

