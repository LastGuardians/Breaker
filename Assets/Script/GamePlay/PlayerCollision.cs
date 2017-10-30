using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    RaycastHit hit_0; // RaycastHit 선언.
    Vector2 playerPos;
    Vector2[] blockPos = new Vector2[5];

    public GameObject rayCollider;      // 레이로 충돌 체크 된 오브젝트

    public bool isNear = false;
    public bool isDestroy = false;

    int blockNum = 0;

    public static PlayerCollision instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            //잘못된 인스턴스를 가르키고 있을 경우
            Destroy(gameObject);
        }

        playerPos = transform.position;
        
        for(int i=0; i<5; ++i)
        {
            blockPos[i] = GameObject.Find("BlockManager").GetComponent<BlockGenerator>().blockArr[i].transform.position;
        }
    }

    void Update()
    {
        playerPos = transform.position;
               
        // raycast
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y + 1f);
        Ray ray = new Ray(transform.position, Vector2.up);
        RaycastHit2D hit = Physics2D.Raycast(currentPos, transform.TransformDirection(0, 2, 0), 2.5f);
        //Debug.Log("currentPos : " + currentPos.y);
        if (hit.collider != null)
        {
            //Debug.DrawRay(currentPos, transform.TransformDirection(0, 2, 0), Color.green);
            //Debug.Log("hit.transform.position.y = " + hit.transform.position.y);
            if (hit.collider.tag == "block1" || hit.collider.tag == "block2" ||
                hit.collider.tag == "block3" || hit.collider.tag == "block4" ||
                hit.collider.tag == "block5" || hit.collider.tag == "rope" ||
                hit.collider.tag == "bomb" || hit.collider.tag == "handcuffs" ||
                hit.collider.tag == "portion" || hit.collider.tag == "FeverBlock1" ||
                hit.collider.tag == "FeverBlock2" || hit.collider.tag == "FeverBlock3" ||
                hit.collider.tag == "FeverBlock4" || hit.collider.tag == "FeverBlock5" ||
                hit.collider.tag == "FeverBlock6" || hit.collider.tag == "FeverBlock7" ||
                hit.collider.tag == "FeverBlock8" || hit.collider.tag == "FeverBlock9" ||
                hit.collider.tag == "FeverBlock10")
            {
                rayCollider = hit.collider.gameObject;
                //if (rayCollider.GetComponent<BlockStatusManager>().hp > 0)
                //{
                //    PlayerManager.instance.isDestroy = false;
                //}
            }
            
        }
      
    }
}