using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalProb : MonoBehaviour {

    public float criticalProb;  // 플레이어 크리티컬 확률
    public bool criticalStart = false;
    float originDamage;

    //public GameObject CriticalText;

    System.Random r = new System.Random();
    int criRange = 0;   // 크리티컬 확률 범위

    //public static CriticalProb instance = null;

    // Use this for initialization
    void Start () {

        //criticalProb = PlayerManager.instance.probablity;
        //Debug.Log("criticalProb : " + criticalProb);
        //originDamage = PlayerManager.instance.damage;
        StartCoroutine(CriticalProbCheck());
    }

    // 크리티컬 확률 체크
    public IEnumerator CriticalProbCheck()
    {
        while(true)
        {
            yield return new WaitForSeconds(10f);
            //yield return new WaitUntil(() => PlayerManager.instance.isDestroy);
            criRange = r.Next(0, 100);
            //Debug.Log("criRange: " + criRange);
            if(criRange <= criticalProb)
            {
                if(!criticalStart)
                {
                    //CriticalText.SetActive(true);
                    StartCoroutine(CriticalDuration());
                }
            }
        }
    }

    // 크리티컬 유지 체크
    public IEnumerator CriticalDuration()
    {
        criticalStart = true;
        //PlayerManager.instance.damage *= PlayerManager.instance.critical;
        Debug.Log("criticalStart: " + criticalStart);
        while (true)
        {            
            yield return new WaitUntil(() => PlayerManager.instance.isDestroy);
            //Debug.Log("PlayerManager.instance.damage: " + PlayerManager.instance.damage);
            criticalStart = false;
           // PlayerManager.instance.damage = originDamage;
           // CriticalText.SetActive(false);
            yield break;
        }
    }
}
