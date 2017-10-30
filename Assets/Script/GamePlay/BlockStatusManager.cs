
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStatusManager : MonoBehaviour
{
    // 블럭 스탯
    [Header ("BlockStatus")]
    public double hp = 0;           // 블록 hp
    public int grade = 0;           // 블록 등급
    public int blockType = 0;       // 블록 타입(기본 =0 / 강화 =1)
    public int resourceType = 0;    // 블록 리소스 타입(0 = 철창 / 1 = 철문 / 3 = 교도소 건물 / 4 = 감시탑 / 5 = 담장 /
                                    //                  6 = 밧줄 / 7 = 수갑 / 8 = 폭탄 / 9 = 포션)
    public int score = 0;           // 블록의 점수
    public double coin = 0;         // 블록 깼을 때 얻는 코인 수
    public int key = 0;             // 블록 깼을 때 얻는 열쇠 수
    public int stage = 1;   // 블럭 단계

    public GameObject DestroyParticle;

    System.Random r = new System.Random();
    int range = 0;

    static Block[] BlockStatNormal = new Block[10];
    static Block[] BlockStatUpgrade = new Block[10];

    public static BlockStatusManager instance = null;

    //public GameObject UserStat;
   
    void Awake()
    {
        StartCoroutine(BlockStageCheck());
        //StartCoroutine(BlockHPCheck());

        for (int i = 0; i < 10; ++i) 
        {
            BlockStatNormal[i].hp = 5 + 17 * ((i + 1) - 1);
            BlockStatNormal[i].score = 20 * (i + 1);
            BlockStatNormal[i].coin = 5 * (i+1);
            BlockStatNormal[i].grade = i + 1;
            BlockStatNormal[i].blockType = 0;

            BlockStatUpgrade[i].hp = (5 + 17 * ((i + 1) - 1)) * 1.5;  // 1.5배
            BlockStatUpgrade[i].score = 20 * (i + 1);
            BlockStatUpgrade[i].coin = (5 * (i + 1)) * 1.5; // 1.5배
            BlockStatUpgrade[i].grade = i + 1;
            BlockStatUpgrade[i].blockType = 1;
        }
    }

    // 기본 건물 셋팅
    public void SetBlockNormal(int num)
    {
        hp = BlockStatNormal[num-1].hp;
        //Debug.Log("hp: " + hp);
        score = BlockStatNormal[num-1].score;
        coin = BlockStatNormal[num-1].coin;
        grade = BlockStatNormal[num-1].grade;
        blockType = BlockStatNormal[num-1].blockType;
    }

    // 강화 건물 셋팅
    public void SetBlockUpgrade(int num)
    {
        hp = BlockStatUpgrade[num-1].hp;
        score = BlockStatUpgrade[num-1].score;
        coin = BlockStatUpgrade[num-1].coin;
        grade = BlockStatUpgrade[num-1].grade;
        blockType = BlockStatUpgrade[num-1].blockType;
    }

    public void SetObject(int num)
    {
        if(num == 1)    // 밧줄
        {
            hp = 10000;
            score = 100;
            coin = 0;
        }
        else if (num == 2)  // 수갑
        {
            hp = 10000;
            score = 100;
            coin = 0;
        }
        else if (num == 3)  // 폭탄
        {
            hp = 10000;
            score = 0;
            coin = 0;
        }
        else if (num == 4)  // 포션
        {
            hp = 50;
            score = 0;
            coin = 0;
        }
    }
    

    // 블럭 단계 설정
    IEnumerator BlockStageCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (PlayerManager.instance.score < 340)
            {
                //Debug.Log("playerScore : " + playerScore);
                stage = 1;
            }
            else if (PlayerManager.instance.score < 1020)
            {
                //Debug.Log("stage : " + stage);
                stage = 2;
            }
            else if (PlayerManager.instance.score < 2040)
                stage = 3;
            else if (PlayerManager.instance.score < 3400)
                stage = 4;
            else if (PlayerManager.instance.score < 5100)
                stage = 5;
            else if (PlayerManager.instance.score < 7140)
                stage = 6;
            else if (PlayerManager.instance.score < 9520)
                stage = 7;
            else if (PlayerManager.instance.score < 12240)
                stage = 8;
            else if (PlayerManager.instance.score < 15300)
                stage = 9;
            else if (PlayerManager.instance.score < 18700)
                stage = 10;
        }
    }
}