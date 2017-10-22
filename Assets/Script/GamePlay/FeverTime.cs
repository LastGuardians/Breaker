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
    public GameObject feverStartImage;
    public float block_destroy_count = 0;    // 파괴된 블럭 개수

    public bool fever_start = false;
    float fever_time = 0;
    float originGrav = 1f;    // 디폴트 중력값
    int fever_cnt = 1;      // 피버타임 발동 횟수

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
        StartCoroutine(FeverStartCheck());
        //Invoke("FeverTimeCheck", 0.1f);
    }

    void Update()
    {
        // 피버 발동 중엔 게이지가 차지 않음.
        if (!fever_start)
        {            
            feverGauge.GetComponent<Slider>().value = block_destroy_count;
            //if (block_destroy_count == (20 * fever_cnt) &&
            //    BlockGenerator.instance.block_gravity != null)
            //{
            //    //Debug.Log("fever 발동");
            //    feverGauge.GetComponent<Slider>().value = 0;
            //    block_destroy_count = 0;
            //    fever_start = true;
            //    StartCoroutine(FeverTimeCheck());
            //}
        }
    }

    public IEnumerator FeverStartCheck()
    {
        while (true)
        {
            yield return new WaitUntil(() => block_destroy_count == (20 * fever_cnt));
            if(BlockGenerator.instance.block_gravity != null)
            {
                feverGauge.GetComponent<Slider>().value = 0;
                block_destroy_count = 0;
                fever_start = true;
                StartCoroutine(FeverTimeCheck());
            }
        }
    }

    IEnumerator FeverTimeCheck()    // 피버타임 발동 시간 체크
    {
        //Debug.Log("FeverTimeCheck");
        GameObject.Find("GlobalBGM").GetComponent<GlobalBGM>().feverBgmOn = true;
        //GameObject.Find("BlockManager").GetComponent< BlockGenerator >().block_gravity.gravityScale *= 1.5f;

        feverStartImage.SetActive(true);

        while (true)
        {
            yield return new WaitForSeconds(1f);
            fever_time += 1;

            if (fever_time >= 4)
            {
                feverStartImage.SetActive(false);
            }

            if (fever_time >= 5) 
            {
                fever_time = 0;
                fever_start = false;
                BlockGenerator.instance.block_gravity.gravityScale = originGrav;
                GameObject.Find("GlobalBGM").GetComponent<GlobalBGM>().feverBgmOn = false;
                block_destroy_count = 0;
                fever_cnt += 1;
                feverGauge.GetComponent<Slider>().maxValue = 20 * fever_cnt;                
                //Debug.Log("maxValue : " + feverGauge.GetComponent<Slider>().maxValue);
                yield break;
            }
        }
    }
}