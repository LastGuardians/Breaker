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
    public int buildingType = 0;    // 건물 타입(리소스에 따라 달라진다.)
    public int score = 0;           // 블록의 점수

    public static BlockStatusManager instance = null;

    GameObject UserStat;
   
    void Start()
    {
        UserStat = GameObject.Find("Player");
    }

    void Update()
    {
        if(UserStat.GetComponent<PlayerStatusManager>().score < 700)
        {
            hp = 15;
            grade = 1;
            blockType = 0;
            score = 20;
        }
    }
}