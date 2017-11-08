using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WarningController : MonoBehaviour
{
    float warning_time = 0;         // 게임 시작 후 경고가 발동될 시간
    float warning_continue_time = 0;    // 경고가 지속되는 시간
    public bool warning_start = false;      // 경고 발생 확인 변수
    public int warning_range = 0;   // 경고 발동 확률 범위

    public GameObject notWarningSprite;       // 경고 상태가 아닐 때의 스프라이트
    public GameObject warningSprite;          // 경고 상태일 때 스프라이트
    public GameObject warningPanel;     // 경고 패널

    GameObject blockDestroy;

    float originGrav = 1f;        // 블록 스피드 디폴트값
    System.Random r = new System.Random();

    public static WarningController instance;

    private void Start()
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

        warning_time = 0;
        notWarningSprite.SetActive(true);

        blockDestroy = GameObject.Find("BlockManager");

        //StartCoroutine(WarningTimeCheck());
        //StartCoroutine(WarningProbCheck());
    }

    public IEnumerator WarningProbCheck()
    {
        warning_range = r.Next(100, 10000);
        Debug.Log("warning_range :" + warning_range);

        if (warning_range < 200 && !FeverTime.instance.fever_start && !BlockGenerator.instance.warningStart)
        {
            BlockGenerator.instance.warningStart = true;
            notWarningSprite.SetActive(false);
            StartCoroutine(WarningStart());
        }

        yield return null;
    }

    // 경고 지속 시간 측정
    IEnumerator WarningStart()
    {
        GlobalBGM.instance.warningBgmOn = true;
        GlobalSFX.instance.PlayWarningSound(true);
        //BlockGenerator.instance.block_gravity.gravityScale *= 1.3f;
        warningSprite.SetActive(true);
        warningPanel.SetActive(true);
        //GlobalBGM.instance.WarningBGM();

        while (true)
        {
            yield return new WaitForSeconds(1f);
            if(FeverTime.instance.fever_start)  // 피버 발동되면 즉시 중지
            {
                BlockGenerator.instance.block_gravity.gravityScale = originGrav;
                warning_continue_time = 0;
                BlockGenerator.instance.warningStart = false;
                warningSprite.SetActive(false);
                notWarningSprite.SetActive(true);
                warningPanel.SetActive(false);
                GlobalBGM.instance.warningBgmOn = false;
                GlobalSFX.instance.PlayWarningSound(false);
                yield break;
            }
            warning_continue_time += 1;

            if (warning_continue_time >= 5)
            {
                BlockGenerator.instance.block_gravity.gravityScale = originGrav;
                warning_continue_time = 0;
                //GlobalBGM.instance.PlayBGM();
                BlockGenerator.instance.warningStart = false;
                warningSprite.SetActive(false);
                notWarningSprite.SetActive(true);
                warningPanel.SetActive(false);
                GlobalSFX.instance.PlayWarningSound(false);
                GlobalBGM.instance.warningBgmOn = false;
                yield break;
            }
        }
    }
}