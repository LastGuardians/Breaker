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

    public bool fever_start = false;
    float fever_time = 0;
    float originGrav = 0.7f;

    public static FeverTime instance;

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

        feverGauge = GameObject.Find("FeverGauge");
        //StartCoroutine(FeverTimeCheck());
        //Invoke("FeverTimeCheck", 0.1f);
    }

    void Update()
    {
        // 피버 발동 중엔 게이지가 차지 않음.
        if (!fever_start)
        {            
            feverGauge.GetComponent<Slider>().value = block_destroy_count;
            if (block_destroy_count == 20 &&
                BlockGenerator.instance.block_gravity != null)
            {
                //Debug.Log("fever 발동");
                feverGauge.GetComponent<Slider>().value = 0;
                block_destroy_count = 0;
                fever_start = true;
                StartCoroutine(FeverTimeCheck());
            }
        }
    }

    IEnumerator FeverTimeCheck()    // 피버타임 발동 시간 체크
    {
        //GlobalBGM.instance.FeverBGM();
        BlockGenerator.instance.block_gravity.gravityScale *= 1.5f;
       // Debug.Log("block_gravity: " + BlockGenerator.instance.block_gravity.gravityScale);

        while (true)
        {
            yield return new WaitForSeconds(1f);
            fever_time += 1;
            //Debug.Log("fever_time: " + fever_time);

            if (fever_time >= 5) 
            {
                fever_time = 0;
                fever_start = false;
                BlockGenerator.instance.block_gravity.gravityScale = originGrav;
                //GlobalBGM.instance.PlayBGM();
                yield break;
            }
        }
    }
}