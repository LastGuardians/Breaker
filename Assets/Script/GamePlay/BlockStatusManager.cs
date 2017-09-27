
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStatusManager : MonoBehaviour
{
    // 블럭 스탯
    [Header ("BlockStatus")]
    public int hp = 0;              // 블록 hp
    public int grade = 0;           // 블록 등급
    public int blockType = 0;       // 블록 타입(기본 =0 / 강화 =1)
    //public int resourceType = 0;  // 블록 리소스 타입(ex. 철창, 철문)
    public int score = 0;           // 블록의 점수
    public int coin = 0;            // 블록 깼을 때 얻는 코인 수
    public int key = 0;             // 블록 깼을 때 얻는 열쇠 수

    public int stage = 1;   // 블럭 단계

    // 건물 리소스
    Sprite[] jail_normal = new Sprite[5];  // 교도소 내부(철창) 기본 건물 리소스
    Sprite[] jail_upgrade = new Sprite[5];  // 교도소 내부(철창) 강화 건물 리소스

    System.Random r = new System.Random();
    int range = 0;

    static Block[] BlockStatNormal = new Block[5];
    static Block[] BlockStatUpgrade = new Block[5];

    int playerScore = 0;
    int gradeRange = 0;

    public static BlockStatusManager instance = null;

    //public GameObject UserStat;
   
    void Start()
    {
        StartCoroutine(BlockStageCheck());
        //UserStat = GameObject.Find("Player");
        for (int i = 0; i < 5; ++i)
        {
            // 리소스 셋팅
            jail_normal[i] = Resources.Load<Sprite>("Building/1.Jail/jail" + (i + 1).ToString());
            jail_upgrade[i] = Resources.Load<Sprite>("Building/1.Jail/upgrade/jail_upgrade" + (i + 1).ToString());
        }
        playerScore = PlayerManager.instance.score;
        gradeRange = BlockGenerator.instance.grade_range;

        // 1단계 기본 블럭
        BlockStatNormal[0].hp = 15;
        BlockStatNormal[0].score = 20;
        BlockStatNormal[0].coin = 3;
        BlockStatNormal[0].grade = 1;
        BlockStatNormal[0].blockType = 0;
        // 2단계 기본 블럭
        BlockStatNormal[1].hp = 40;
        BlockStatNormal[1].score = 20;
        BlockStatNormal[1].coin = 9;
        BlockStatNormal[1].grade = 2;
        BlockStatNormal[1].blockType = 0;
        // 3단계 기본 블럭
        BlockStatNormal[2].hp = 100;
        BlockStatNormal[2].score = 120;
        BlockStatNormal[2].coin = 15;
        BlockStatNormal[2].grade = 3;
        BlockStatNormal[2].blockType = 0;
        // 4단계 기본 블럭
        BlockStatNormal[3].hp = 150;
        BlockStatNormal[3].score = 180;
        BlockStatNormal[3].coin = 30;
        BlockStatNormal[3].grade = 4;
        BlockStatNormal[3].blockType = 0;
        // 5단계 기본 블럭
        BlockStatNormal[4].hp = 240;
        BlockStatNormal[4].score = 280;
        BlockStatNormal[4].coin = 50;
        BlockStatNormal[4].grade = 5;
        BlockStatNormal[4].blockType = 0;

        // 1단계 강화 블럭
        BlockStatUpgrade[0].hp = 23;
        BlockStatUpgrade[0].score = 20;
        BlockStatUpgrade[0].coin = 6;
        BlockStatUpgrade[0].grade = 1;
        BlockStatUpgrade[0].blockType = 1;
        // 2단계 강화 블럭
        BlockStatUpgrade[1].hp = 60;
        BlockStatUpgrade[1].score = 20;
        BlockStatUpgrade[1].coin = 18;
        BlockStatUpgrade[1].grade = 2;
        BlockStatUpgrade[1].blockType = 1;
        // 3단계 강화 블럭
        BlockStatUpgrade[2].hp = 150;
        BlockStatUpgrade[2].score = 20;
        BlockStatUpgrade[2].coin = 25;
        BlockStatUpgrade[2].grade = 3;
        BlockStatUpgrade[2].blockType = 1;
        // 4단계 강화 블럭
        BlockStatUpgrade[3].hp = 225;
        BlockStatUpgrade[3].score = 20;
        BlockStatUpgrade[3].coin = 45;
        BlockStatUpgrade[3].grade = 4;
        BlockStatUpgrade[3].blockType = 1;
        // 5단계 강화 블럭
        BlockStatUpgrade[4].hp = 360;
        BlockStatUpgrade[4].score = 20;
        BlockStatUpgrade[4].coin = 75;
        BlockStatUpgrade[4].grade = 5;
        BlockStatUpgrade[4].blockType = 1;

        
    }

    void Update()
    {

    }

    public void SetBlockNormal(int num)
    {
        if (num == 1)
        {
            // 1단계 기본 블럭
            hp = BlockStatNormal[0].hp;
            score = BlockStatNormal[0].score;
            coin = BlockStatNormal[0].coin;
            grade = BlockStatNormal[0].grade;
            blockType = BlockStatNormal[0].blockType;
        }
        else if (num == 2)
        {
            // 2단계 기본 블럭
            hp = BlockStatNormal[1].hp;
            score = BlockStatNormal[1].score;
            coin = BlockStatNormal[1].coin;
            grade = BlockStatNormal[1].grade;
            blockType = BlockStatNormal[1].blockType;

        }
        else if (num == 3)
        {
            // 3단계 기본 블럭
            hp = BlockStatNormal[2].hp;
            score = BlockStatNormal[2].score;
            coin = BlockStatNormal[2].coin;
            grade = BlockStatNormal[2].grade;
            blockType = BlockStatNormal[2].blockType;

        }
        else if (num == 4)
        {
            // 4단계 기본 블럭
            hp = BlockStatNormal[3].hp;
            score = BlockStatNormal[3].score;
            coin = BlockStatNormal[3].coin;
            grade = BlockStatNormal[3].grade;
            blockType = BlockStatNormal[3].blockType;

        }
        else if (num == 5)
        {
            // 5단계 기본 블럭
            hp = BlockStatNormal[4].hp;
            score = BlockStatNormal[4].score;
            coin = BlockStatNormal[4].coin;
            grade = BlockStatNormal[4].grade;
            blockType = BlockStatNormal[4].blockType;
        }
        
    }

    // 강화 건물 세팅
    public void SetBlockUpgrade(int num)
    {
        if (num == 1)
        {
            // 1단계 강화 블럭
            hp = BlockStatUpgrade[0].hp;
            score = BlockStatUpgrade[0].score;
            coin = BlockStatUpgrade[0].coin;
            grade = BlockStatUpgrade[0].grade;
            blockType = BlockStatUpgrade[0].blockType;
        }
        else if (num == 2)
        {
            // 2단계 강화 블럭
            hp = BlockStatUpgrade[1].hp;
            score = BlockStatUpgrade[1].score;
            coin = BlockStatUpgrade[1].coin;
            grade = BlockStatUpgrade[1].grade;
            blockType = BlockStatUpgrade[1].blockType;
        }
        else if (num == 3)
        {
            // 3단계 강화 블럭
            hp = BlockStatUpgrade[2].hp;
            score = BlockStatUpgrade[2].score;
            coin = BlockStatUpgrade[2].coin;
            grade = BlockStatUpgrade[2].grade;
            blockType = BlockStatUpgrade[2].blockType;
        }
        else if (num == 4)
        {
            // 4단계 강화 블럭
            hp = BlockStatUpgrade[3].hp;
            score = BlockStatUpgrade[3].score;
            coin = BlockStatUpgrade[3].coin;
            grade = BlockStatUpgrade[3].grade;
            blockType = BlockStatUpgrade[3].blockType;
        }
        else if (num == 5)
        {
            // 5단계 강화 블럭
            hp = BlockStatUpgrade[4].hp;
            score = BlockStatUpgrade[4].score;
            coin = BlockStatUpgrade[4].coin;
            grade = BlockStatUpgrade[4].grade;
            blockType = BlockStatUpgrade[4].blockType;
        }
    }


    public void BlockNormal()      // 기본 블럭 셋팅
    {
        range = r.Next(0, 100);
        if (stage == 1)     // 700점 미만(1단계)
        {
            //Debug.Log("stage : "+ stage);
            SetBlockNormal(stage);
        }
        else if(stage == 2)    // 2단계
        {
            Debug.Log("stage : " + stage);
            if (range < 30)    // 1단계 블럭 확률 30%
            {
                //Debug.Log("1단계 블럭");
                SetBlockNormal(stage - 1);
            }
            else        // 2단계 블럭 확률 70%
            {
               // Debug.Log("2단계 블럭");
                SetBlockNormal(stage);
            }
        
        }
        else if (stage == 3)    // 3단계
        {
            if (gradeRange < 10)    // 1단계 블럭 확률 10%
            {
                SetBlockNormal(stage - 2);
            }
            else if(gradeRange < 40)  // 2단계 블럭 확률 30%
            {
                SetBlockNormal(stage - 1);
            }
            else if (gradeRange < 70) // 3단계 블럭 확률 30%
            {
                SetBlockNormal(stage);
            }
            else if (gradeRange < 90) // 수갑/밧줄 확률 20%
            {
                hp = 10000;
                score = 150;
            }
            else if (gradeRange < 100) // 폭탄 확률 10%
            {
                hp = 10000;
                score = 150;
            }
        }
        else if (stage == 4)    // 4단계
        {
            if (gradeRange < 5)    // 1단계 블럭 확률 5%
            {
                SetBlockNormal(stage - 3);
            }
            else if (gradeRange < 10)  // 2단계 블럭 확률 5%
            {
                SetBlockNormal(stage-2);
            }
            else if (gradeRange < 40) // 3단계 블럭 확률 30%
            {
                SetBlockNormal(stage-1);
            }
            else if (gradeRange < 70) // 4단계 블럭 확률 30%
            {
                SetBlockNormal(stage);
            }
            else if (gradeRange < 90) // 수갑/밧줄 확률 20%
            {
                hp = 10000;
                score = 150;
            }
            else if (gradeRange < 100) // 폭탄 확률 10%
            {
                hp = 10000;
                score = 150;
            }
        }
        else if (stage == 5)    // 5단계
        {
            if (gradeRange < 2)    // 1단계 블럭 확률 2%
            {
                SetBlockNormal(stage-4);
            }
            else if (gradeRange < 5)  // 2단계 블럭 확률 3%
            {
                SetBlockNormal(stage-3);
            }
            else if (gradeRange < 10) // 3단계 블럭 확률 5%
            {
                SetBlockNormal(stage-2);
            }
            else if (gradeRange < 40) // 4단계 블럭 확률 30%
            {
                SetBlockNormal(stage-1);
            }
            else if (gradeRange < 70) // 5단계 블럭 확률 30%
            {
                SetBlockNormal(stage);
            }
            else if (gradeRange < 90) // 수갑/밧줄 확률 20%
            {
                hp = 10000;
                score = 150;
            }
            else if (gradeRange < 100) // 폭탄 확률 10%
            {
                hp = 10000;
                score = 150;
            }
        }
    }

    public void BlockUpgrade()  // 강화 블럭 셋팅
    {
        if(gradeRange < 50)   // 50% 확률로 열쇠 드롭
        {
            key = 1;
        }

        if (stage == 1)     // 1단계
        {
            SetBlockUpgrade(stage);
        }
        else if (stage == 2)   // 2단계
        {
            SetBlockUpgrade(stage);
        }
        else if (stage == 3)   // 3단계
        {
            SetBlockUpgrade(stage);
        }
        else if (stage == 4)   // 4단계
        {
            SetBlockUpgrade(stage);
        }
        else if (stage == 5)   // 5단계
        {
            SetBlockUpgrade(stage);
        }
    }

    // 블럭 단계 설정
    IEnumerator BlockStageCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (PlayerManager.instance.score < 700)
            {
                //Debug.Log("playerScore : " + playerScore);
                stage = 1;
            }
            else if (PlayerManager.instance.score < 2500)
            {
                stage = 2;
            }
            else if (PlayerManager.instance.score < 7500)
                stage = 3;
            else if (PlayerManager.instance.score < 15000)
                stage = 4;
            else if (PlayerManager.instance.score >= 15000)
                stage = 5;
        }
    }
}