using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockCollisionManager : MonoBehaviour
{
    public GameObject player;
    GameObject bombEffect;

    private void Start()
    {
        player = GameObject.Find("Player");
        bombEffect = player.GetComponent<PlayerManager>().BombEffect;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 블럭이 땅에 충돌하면 플레이어 생명 감소
        if (collision.collider.CompareTag ("block1") || collision.collider.CompareTag("block2")
          || collision.collider.CompareTag ("block3") || collision.collider.CompareTag ("block4")
          || collision.collider.CompareTag ("block5") || collision.collider.CompareTag ("FeverBlock1")
            || collision.collider.CompareTag ("FeverBlock2") || collision.collider.CompareTag ("FeverBlock3")
            || collision.collider.CompareTag ("FeverBlock4") || collision.collider.CompareTag ("FeverBlock5")
            || collision.collider.CompareTag ("FeverBlock6") || collision.collider.CompareTag ("FeverBlock7")
            || collision.collider.CompareTag ("FeverBlock8") || collision.collider.CompareTag ("FeverBlock9")
            || collision.collider.CompareTag ("FeverBlock10"))
        {
            PlayerManager.instance.life += 1;
            GlobalSFX.instance.PlayCollapseSound();
            Handheld.Vibrate();     // 진동
            //PlayerManager.instance.shield_able = false;
        }

        else if (collision.collider.CompareTag("rope") || collision.collider.CompareTag("handcuffs"))        
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

        if (collision.CompareTag("bomb") && collision.CompareTag("Player")) // 폭탄이 터질때, 반경 안에 플레이어가 있으면 생명력 -2
        {
            PlayerManager.instance.life += 2;
            GlobalSFX.instance.PlayCollapseSound();
            Handheld.Vibrate();     // 진동

            bombEffect.SetActive(true);
            Instantiate(bombEffect, new Vector2(newObj.transform.position.x, (newObj.transform.position.y - 2)), transform.rotation);

            parent = newObj.transform.parent.gameObject;
            GlobalSFX.instance.PlayBombSound();
            Destroy(newObj);
            Destroy(parent);
        }
        else if(collision.CompareTag("bomb"))
        {
            bombEffect.SetActive(true);
            Instantiate(bombEffect, new Vector2(newObj.transform.position.x, (newObj.transform.position.y - 2)), transform.rotation);

            parent = newObj.transform.parent.gameObject;
            GlobalSFX.instance.PlayBombSound();
            Destroy(newObj);
            Destroy(parent);
        }
        bombEffect.SetActive(false);
    }
}