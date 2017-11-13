using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour {

    Sprite[] comboNum = new Sprite[10];
    int blockCnt = 0;
    int blockCntBefore = 0;
    int comboTime = 0;

    public bool criticalStart = false;
    public GameObject ComboText;
    public GameObject ComboString;
    public Animator comboEnd;
    public Animator criticalAni;
    public GameObject CriticalText;
   

    public static ComboManager instance = null;
    // Use this for initialization
    void Start () {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            //잘못된 인스턴스를 가르키고 있을 경우
            Destroy(gameObject);
        }

        //ComboText = GameObject.Find("ComboText");
        comboEnd = ComboText.GetComponent<Animator>();
        criticalAni = CriticalText.GetComponent<Animator>();
        // 콤보 체크 코루틴
        StartCoroutine(ComboCheckCurr());
        StartCoroutine(ComboCritical());
    }

    // 현재 블럭 카운트 코루틴
    public IEnumerator ComboCheckCurr()
    {
        while (true)
        {
            yield return new WaitUntil(() => PlayerManager.instance.isDestroy == true);
            ComboText.SetActive(true);
            ComboString.SetActive(true);
            
            ComboText.GetComponent<Text>().text = PlayerManager.instance.blockCnt.ToString();
            StopCoroutine("ComboTimeCheck");
            StartCoroutine("ComboTimeCheck");
            
        }
    }

    // 콤보 시간 체크
    public IEnumerator ComboTimeCheck()
    {
        comboTime = 0;

        while (true)
        {            
            yield return new WaitForSeconds(1f);
            comboTime += 1;
            //Debug.Log("comboTime: " + comboTime);

            if (comboTime >= 3)
            {
                //ComboAniEnd();
                ComboText.SetActive(false);
                ComboString.SetActive(false);
            }

            if (comboTime >= 5)
            {
                if (!PlayerManager.instance.isDestroy)
                {                    
                    PlayerManager.instance.blockCnt = 0;
                    comboTime = 0;
                }
            }
        }
    }

    public void ComboAniEnd()
    {
        comboEnd.SetTrigger("ComboEnd");
    }

    // 콤보 크리티컬 발동
    public IEnumerator ComboCritical()
    {
        while(true)
        {
            yield return new WaitUntil(() => (PlayerManager.instance.blockCnt) % 5 == 0);
            if(PlayerManager.instance.blockCnt > 1)
            {                
                if(!criticalStart)
                {
                    StartCoroutine(CriticalDuration());
                    //Debug.Log("크리티컬 발동");
                }                  
            }
        }
    }

    // 크리티컬 유지 체크
    public IEnumerator CriticalDuration()
    {
        criticalStart = true;        
       // float originDamage = PlayerManager.instance.damage;
        //PlayerManager.instance.damage *= PlayerManager.instance.critical;

        while (true)
        {
            yield return new WaitUntil(() => PlayerManager.instance.isDestroy);
            criticalStart = false;
            //PlayerManager.instance.damage = originDamage;
            yield break;
        }
    }

    public IEnumerator CriticalTextDuration()
    {
        int time = 0;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            
            time += 1;
            if(time >= 1)
            {
                CriticalText.SetActive(false);
                yield break;
            }
        }
    }
}
