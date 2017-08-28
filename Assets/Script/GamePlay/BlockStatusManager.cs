using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BlockStatusManager : MonoBehaviour
{
    // 블럭 스탯
    public int hp = 0;              // 블록 hp
    public int grade = 0;           // 블록 등급
    public int blockType = 0;       // 블록 타입(기본 =0 / 강화 =1)
    public int resourceType = 0;    // 블록 리소스 타입(ex. 철창, 철문)
    public int score = 0;           // 블록의 점수

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
        //BlockSetting();
        //if (BlockGenerator.instance.range == 1)
        //    BlockUpgrade();
        //else
        //    BlockNormal();
    }

    public void BlockNormal()      // 기본 블럭 셋팅
    {
        if (PlayerManager.instance.score < 700)     // 700점 미만(1단계)
        {
            hp = 15;
            score = 20;
            grade = 1;
            blockType = 0;
        }
        else if(PlayerManager.instance.score < 2500)    // 2단계
        {
            if(BlockGenerator.instance.grade_range < 30)    // 1단계 블럭 확률 30%
            {
                hp = 15;
                score = 20;
                grade = 1;
                blockType = 0;
            }
            else        // 2단계 블럭 확률 70%
            {
                hp = 40;
                score = 20;
                grade = 2;
                blockType = 0;
            }
        
        }
    }

    public void BlockUpgrade()  // 강화 블럭 셋팅
    {
        if (PlayerManager.instance.score < 700)     // 1단계
        {
            hp = 23;
            score = 20;
            grade = 1;
            blockType = 1;
        }
        else if (PlayerManager.instance.score < 2500)   // 2단계
        {
            hp = 60;
            score = 20;
            grade = 2;
            blockType = 1;
        }
    }
}