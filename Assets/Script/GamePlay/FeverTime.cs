using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class FeverTime : MonoBehaviour
{
    public GameObject feverGauge;   // 피버 게이지
    public float block_destroy_count = 0;    // 파괴된 블럭 개수

    float fever_time = 0;

    public static FeverTime instance;

    void Start()
    {
        feverGauge = GameObject.Find("FeverGauge");
        //StartCoroutine(FeverTimeCheck());
        //Invoke("FeverTimeCheck", 0.1f);
    }

    void Update()
    {
        feverGauge.GetComponent<Slider>().value = block_destroy_count;
        if (block_destroy_count == 5)
        {
            feverGauge.GetComponent<Slider>().value = 0;
            block_destroy_count = 0;
        }
        //fever_time += Time.deltaTime;
        //if (block_destroy_count == 1)
        //    StartCoroutine(FeverTimeCheck());
    }

    void FeverTimeCheck()    // 피버타임 발동 시간 체크
    {
        Debug.Log("FeverTimeCheck 시작");
        if (fever_time > 5.0f)
        {
            Debug.Log("5.0f 초과");
            //yield break;
        }

       // yield return new WaitForSeconds(0.001f);
        Debug.Log("FeverTimeCheck 끝");
        //if (block_destroy_count == 1)
        //{
        //    fever_time += Time.deltaTime;
        //    Debug.Log("fever_time : " + fever_time);

        //    if (fever_time > 5.0f)
        //    {
        //        Debug.Log("5.0f 초과");
        //        yield break;
        //    }
        //    else if (fever_time < 5.0f)
        //    {
        //        Debug.Log("5.0f 미만");
        //        yield return new WaitForSeconds(0.001f);
        //    }
        //}
      
        //yield return null;
    }
}