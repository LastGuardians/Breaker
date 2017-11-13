using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public struct Block   // 블록
{
    public float hp;              // 블록 hp
    public int grade;           // 블록 등급
    public int blockType;       // 블록 타입(기본 =0 / 강화 =1)
    public int score;           // 블록의 점수
    public float coin;            // 블록 깼을 때 얻는 코인 수
    public int key;             // 블록 깼을 때 얻는 열쇠 수
}

public struct Character     // 캐릭터
{
    public int tier;           // 캐릭터 티어
    public int weaponLevel;    // 무기 레벨
    public char[] name;        // 캐릭터 이름
    public int attack;         // 공격력
    public int jump;           // 점프력
}

public struct User
{
    public int score;       // 획득 점수
    public int best_score;  // 획득한 점수 중 최고 점수
    public int coin;        // 코인 수
    public int key;         // 열쇠 수
}