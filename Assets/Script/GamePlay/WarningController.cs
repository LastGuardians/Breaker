using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WarningController : MonoBehaviour
{
    float warning_time = 0;  // 게임 시작 후 경고가 발동될 시간
    float warning_continue_time = 0;    // 경고가 지속되는 시간
    bool warning_start = true; // 경고 발생 확인 변수

    public int warning_range = 0; // 경고 발동 확률 범위

    System.Random r = new System.Random();

    private void Start()
    {
        //StartCoroutine(WarningTimeCheck());
    }

    //private void Update()
    //{
    //    // 경고음 발동시간 측정 시작
    //    if (BlockGenerator.instance.game_start)
    //    {
    //        warning_time += Time.deltaTime;
    //        //Debug.Log("warning_time : " + warning_time);
    //        warning_range = r.Next(100, 10000);
    //        if (warning_time >= 10 && warning_time < 60)
    //        {
    //            if (warning_start)
    //            {
    //                if (warning_range < 1000)
    //                {
    //                    warning_start = false;
    //                    Debug.Log("warning_range < 1000 만족");
    //                    StartCoroutine(WarningStart());
    //                }
    //            }
    //        }
    //    }
    //    else
    //        warning_time = 0;
    //}

    //IEnumerator WarningTimeCheck()
    //{

    //}

    IEnumerator WarningStart()
    {
        BlockGenerator.instance.block_gravity.gravityScale *= 1.3f;

        yield return new WaitForSeconds(0.1f);
        warning_continue_time += Time.deltaTime;
        Debug.Log("warning_continue_time : " + warning_continue_time);
             
            
        
        if (warning_continue_time >= 5)
        {
            Debug.Log("warning_continue_time 5초 넘음");
            BlockGenerator.instance.block_gravity.gravityScale = 0.6f;
            warning_start = true;
            warning_continue_time = 0;
        }
    }
}