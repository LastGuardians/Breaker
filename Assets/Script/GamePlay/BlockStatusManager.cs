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
    public static BlockStatusManager instance = null;

    public GameObject UserStat;
   
    void Start()
    {
        UserStat = GameObject.Find("Player");

        //for (int i = 0; i < 5; ++i) 
        //{
        //    range = r.Next(0, i);

        //    if (range < 1)  // 강화 블럭
        //    {
        //        hp = 23;
        //        grade = 1;
        //        blockType = 1;
        //        score = 20;
        //    }
        //    else
        //    {
        //        hp = 15;
        //        grade = 1;
        //        blockType = 0;
        //        score = 20;
        //    }
        //}
    }

    void Update()
    {        
        if (UserStat.GetComponent<PlayerStatusManager>().score < 700)
        {
            //if(range < 20)  // 강화 블럭
            //{
            //    hp = 23;
            //    grade = 1;
            //    blockType = 1;
            //    score = 20;
            //}
            //else
            //{
            //    hp = 15;
            //    grade = 1;
            //    blockType = 0;
            //    score = 20;
            //}
        }
    }
}