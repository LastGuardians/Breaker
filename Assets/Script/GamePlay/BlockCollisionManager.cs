using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockCollisionManager : MonoBehaviour
{
    public GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        // 플레이어가 점프했을 때, 땅 collider도 같이 올라간다.
        //if(player.transform.position.y > 1)
        //{
        //    transform.position = new Vector3(transform.position.x,
        //    player.transform.position.y - 2);
        //}
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 블럭이 땅에 충돌하면 플레이어 생명 감소, 방어 불가능 상태
        if (collision.collider.tag == ("block1") || collision.collider.tag == ("block2")
          || collision.collider.tag == ("block3") || collision.collider.tag == ("block4")
          || collision.collider.tag == ("block5"))
        {
            PlayerManager.instance.life += 1;
            //PlayerManager.instance.shield_able = false;
        }
    }
}