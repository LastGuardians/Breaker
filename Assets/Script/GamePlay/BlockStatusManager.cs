using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Diagnostics;

public class BlockStatusManager : MonoBehaviour
{
    // 블럭 스탯
    public int hp = 0;              // 블록 hp
    public int grade = 0;           // 블록 등급
    public int blockType = 0;       // 블록 타입(기본 =0 / 강화 =1)
    public int resourceType = 0;    // 블록 리소스 타입(ex. 철창, 철문)
    public int score = 0;           // 블록의 점수
    public int coin = 0;            // 블록 깼을 때 얻는 코인 수
    public int key = 0;             // 블록 깼을 때 얻는 열쇠 수
    
    System.Random r = new System.Random();
    int range = 0;
    //public int type_ratio = 0;    // 블록 타입 비율(기본 or 강화)
    public static BlockStatusManager instance = null;

    public GameObject UserStat;
   
    void Start()
    {
        UserStat = GameObject.Find("Player");
    }

    void Update()
    {

    }

    public void BlockNormal()      // 기본 블럭 셋팅
    {
        if (PlayerManager.instance.score < 700)     // 700점 미만(1단계)
        {
            hp = 15;
            score = 20;
            coin = 3;
            grade = 1;
            blockType = 0;
        }
        else if(PlayerManager.instance.score < 2500)    // 2단계
        {
            if(BlockGenerator.instance.grade_range < 30)    // 1단계 블럭 확률 30%
            {
                hp = 15;
                score = 20;
                coin = 3;
                grade = 1;
                blockType = 0;
            }
            else        // 2단계 블럭 확률 70%
            {
                hp = 40;
                score = 20;
                coin = 9;
                grade = 2;
                blockType = 0;
            }
        
        }
        else if (PlayerManager.instance.score < 7500)    // 3단계
        {
            if (BlockGenerator.instance.grade_range < 10)    // 1단계 블럭 확률 10%
            {
                hp = 15;
                score = 20;
                coin = 3;
                grade = 1;
                blockType = 0;
            }
            else if(BlockGenerator.instance.grade_range < 40)  // 2단계 블럭 확률 30%
            {
                hp = 40;
                score = 20;
                coin = 9;
                grade = 2;
                blockType = 0;
            }
            else if (BlockGenerator.instance.grade_range < 70) // 3단계 블럭 확률 30%
            {
                hp = 100;
                score = 120;
                coin = 15;
                grade = 3;
                blockType = 0;
            }
            else if (BlockGenerator.instance.grade_range < 90) // 수갑/밧줄 확률 20%
            {
                hp = 10000;
                score = 150;
            }
            else if (BlockGenerator.instance.grade_range < 100) // 폭탄 확률 10%
            {
                hp = 10000;
                score = 150;
            }
        }
        else if (PlayerManager.instance.score < 15000)    // 4단계
        {
            if (BlockGenerator.instance.grade_range < 5)    // 1단계 블럭 확률 5%
            {
                hp = 15;
                score = 20;
                coin = 3;
                grade = 1;
                blockType = 0;
            }
            else if (BlockGenerator.instance.grade_range < 10)  // 2단계 블럭 확률 5%
            {
                hp = 40;
                score = 20;
                coin = 9;
                grade = 2;
                blockType = 0;
            }
            else if (BlockGenerator.instance.grade_range < 40) // 3단계 블럭 확률 30%
            {
                hp = 100;
                score = 120;
                coin = 15;
                grade = 3;
                blockType = 0;
            }
            else if (BlockGenerator.instance.grade_range < 70) // 4단계 블럭 확률 30%
            {
                hp = 150;
                score = 180;
                coin = 30;
                grade = 4;
                blockType = 0;
            }
            else if (BlockGenerator.instance.grade_range < 90) // 수갑/밧줄 확률 20%
            {
                hp = 10000;
                score = 150;
            }
            else if (BlockGenerator.instance.grade_range < 100) // 폭탄 확률 10%
            {
                hp = 10000;
                score = 150;
            }
        }
        else if (PlayerManager.instance.score >= 15000)    // 5단계
        {
            if (BlockGenerator.instance.grade_range < 2)    // 1단계 블럭 확률 2%
            {
                hp = 15;
                score = 20;
                coin = 3;
                grade = 1;
                blockType = 0;
            }
            else if (BlockGenerator.instance.grade_range < 5)  // 2단계 블럭 확률 3%
            {
                hp = 40;
                score = 20;
                coin = 9;
                grade = 2;
                blockType = 0;
            }
            else if (BlockGenerator.instance.grade_range < 10) // 3단계 블럭 확률 5%
            {
                hp = 100;
                score = 120;
                coin = 15;
                grade = 3;
                blockType = 0;
            }
            else if (BlockGenerator.instance.grade_range < 40) // 4단계 블럭 확률 30%
            {
                hp = 150;
                score = 180;
                coin = 30;
                grade = 4;
                blockType = 0;
            }
            else if (BlockGenerator.instance.grade_range < 70) // 5단계 블럭 확률 30%
            {
                hp = 240;
                score = 280;
                coin = 50;
                grade = 5;
                blockType = 0;
            }
            else if (BlockGenerator.instance.grade_range < 90) // 수갑/밧줄 확률 20%
            {
                hp = 10000;
                score = 150;
            }
            else if (BlockGenerator.instance.grade_range < 100) // 폭탄 확률 10%
            {
                hp = 10000;
                score = 150;
            }
        }
    }

    public void BlockUpgrade()  // 강화 블럭 셋팅
    {
        if(BlockGenerator.instance.grade_range < 50)   // 50% 확률로 열쇠 드롭
        {
            key = 1;
        }
        if (PlayerManager.instance.score < 700)     // 1단계
        {
            hp = 23;
            score = 20;
            coin = 6;
            grade = 1;
            blockType = 1;
        }
        else if (PlayerManager.instance.score < 2500)   // 2단계
        {
            hp = 60;
            score = 20;
            coin = 18;
            grade = 2;
            blockType = 1;
        }
        else if (PlayerManager.instance.score < 7500)   // 3단계
        {
            hp = 150;
            score = 120;
            coin = 25;
            grade = 3;
            blockType = 1;
        }
        else if (PlayerManager.instance.score < 15000)   // 4단계
        {
            hp = 225;
            score = 120;
            coin = 45;
            grade = 4;
            blockType = 1;
        }
        else if (PlayerManager.instance.score < 15000)   // 5단계
        {
            hp = 360;
            score = 120;
            coin = 75;
            grade = 5;
            blockType = 1;
        }
    }
}