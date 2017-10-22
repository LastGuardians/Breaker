using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour {

    Sprite[] comboNum = new Sprite[10];
    int blockCnt = 0;
    int blockCntBefore = 0;
    int comboTime = 0;

    public GameObject ComboText;
    public GameObject ComboString;
    public Animator comboEnd;

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
        // 콤보 체크 코루틴
        StartCoroutine(ComboCheckCurr());
        //StartCoroutine(ComboCheckBefore());
    }

    // 현재 블럭 카운트 코루틴
    public IEnumerator ComboCheckCurr()
    {
        while (true)
        {
            yield return new WaitUntil(() => PlayerManager.instance.isDestroy == true);
            //yield return new WaitForSeconds(0.1f);
            //if (PlayerManager.instance.isDestroy)
            //{
                //blockCnt = PlayerManager.instance.blockCnt;
            ComboText.SetActive(true);
            ComboString.SetActive(true);
            comboEnd = ComboText.GetComponent<Animator>();
            ComboText.GetComponent<Text>().text = PlayerManager.instance.blockCnt.ToString();
            StopCoroutine("ComboTimeCheck");
            StartCoroutine("ComboTimeCheck");
            //}
            //Debug.Log("blockCnt : " + blockCnt);
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

            if (comboTime >= 2)
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
}
