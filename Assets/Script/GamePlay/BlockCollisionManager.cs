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
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 블럭이 땅에 충돌하면 플레이어 생명 감소
        if (collision.collider.tag == ("block1") || collision.collider.tag == ("block2")
          || collision.collider.tag == ("block3") || collision.collider.tag == ("block4")
          || collision.collider.tag == ("block5") || collision.collider.tag == ("FeverBlock1")
            || collision.collider.tag == ("FeverBlock2") || collision.collider.tag == ("FeverBlock3")
            || collision.collider.tag == ("FeverBlock4") || collision.collider.tag == ("FeverBlock5")
            || collision.collider.tag == ("FeverBlock6") || collision.collider.tag == ("FeverBlock7")
            || collision.collider.tag == ("FeverBlock8") || collision.collider.tag == ("FeverBlock9")
            || collision.collider.tag == ("FeverBlock10"))
        {
            PlayerManager.instance.life += 1;
            GlobalSFX.instance.PlayCollapseSound();
            Handheld.Vibrate();     // 진동
            //PlayerManager.instance.shield_able = false;
        }

        else if (collision.collider.tag == "rope" || collision.collider.tag == "handcuffs")        
        {
            PlayerManager.instance.life += 2;
            GlobalSFX.instance.PlayCollapseSound();
            Handheld.Vibrate();     // 진동
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject newObj = collision.gameObject;
        GameObject parent;

        if (collision.tag == "bomb" && collision.tag == "Player") // 폭탄이 터질때, 반경 안에 플레이어가 있으면 생명력 -2
        {
            PlayerManager.instance.life += 2;
            GlobalSFX.instance.PlayCollapseSound();
            Handheld.Vibrate();     // 진동

            parent = newObj.transform.parent.gameObject;
            Destroy(newObj);
            Destroy(parent);
        }
        else if(collision.tag == "bomb")
        {
            parent = newObj.transform.parent.gameObject;
            Destroy(newObj);
            Destroy(parent);
        }
    }
}