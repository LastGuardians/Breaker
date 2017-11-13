using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockCollisionManager : MonoBehaviour
{
    GameObject BombEffect;
    public string BombEffectName = "BombEffect";
    float bombLifeTime = 0f;

    public bool isBombCollision = false;
    bool playerCollision = false;
    bool bombCorStart = false;

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
            PlayerManager.instance.life += 1;
            GlobalSFX.instance.PlayCollapseSound();
            Handheld.Vibrate();     // 진동
        }

        else if(collision.collider.CompareTag("Player"))
        {
            playerCollision = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerCollision = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerCollision = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject newObj = collision.gameObject;
        GameObject parent;
               
        if(collision.CompareTag("bomb"))
        {
            isBombCollision = true;

            BombEffect = ObjectPool.Instance.PopFromPool(BombEffectName);
            BombEffect.transform.position = new Vector2(newObj.transform.position.x, (newObj.transform.position.y - 2));
            BombEffect.SetActive(true);

            parent = newObj.transform.parent.gameObject;
            GlobalSFX.instance.PlayBombSound();
            Destroy(newObj);
            Destroy(parent);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("bomb"))
        {
            StartCoroutine(BombLifeCheck());            
        }
    }

    public IEnumerator BombLifeCheck()
    {
        if (playerCollision)
        {
            PlayerManager.instance.life += 2;
            GlobalSFX.instance.PlayCollapseSound();
            Handheld.Vibrate();     // 진동
        }

        while (true)
        {
            yield return new WaitForSeconds(1f);
            bombLifeTime += 1;

            if (bombLifeTime >= 2)
            {
                bombLifeTime = 0;
                yield break;
            }
        }
    }
}