using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public struct Block   // 블록
{
    public int hp;              // 블록 hp
    public int level;           // 블록 level
    public int blockType;       // 블록 타입(기본=0 / 은=1 / 금=2)
    public int buildingType;    // 건물 타입(리소스에 따라 달라진다.)
}

public struct Character     // 캐릭터
{
    public int tier;           // 캐릭터 티어
    public int weaponLevel;    // 무기 레벨
    public char[] name;        // 캐릭터 이름
    public int attack;         // 공격력
    public int jump;           // 점프력
}