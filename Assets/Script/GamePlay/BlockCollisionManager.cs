using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockCollisionManager : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 블럭이 땅에 충돌하면 플레이어 생명 감소
        if (collision.collider.tag == "Collision")
        {
            PlayerManager.instance.life += 1;
        }
    }
}