using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour {
    
    public float jumpSpeed = 200f;
    Rigidbody2D playerRg;       // 플레이어 리지드바디
    public Rigidbody2D blockRg;        // 블럭 리지드바디
    //public Collider2D[] blockArrCol = new Collider2D[5];
    public Collider2D col_player;   // 플레이어의 컬라이더
    public GameObject player;       // 플레이어 오브젝트
    public bool isCollision = false;

    public Collider2D col_parent;       // 블럭의 부모 컬라이더(활성/비활성화용)

    public GameObject destroy_block;    // 파괴할 블럭 

    //public bool block_drop_min = false; // 블럭이 최소 좌표에 도달했는지 확인하는 변수

    public bool attackOn = false;    // 공격 버튼이 터치 되었는지 확인하는 변수
    public bool shieldOn = false;   // 방어 버튼이 터치 되었는지 확인

    public bool block_destroy = false;   // 블럭이 모두 파괴되었는지 확인하는 변수
    public bool ground_collsion = false;     // 플레이어가 땅과 충돌되었는지 확인하는 변수
    public bool shield_able = false;   // 방어가 가능한 상태인지 판단.

    private Touch tempTouchs;

    public int score = 0;   // test용 score
    GameObject scoreText;

    public static PlayerManager instance = null;

    void Start () {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            //잘못된 인스턴스를 가르키고 있을 경우
            Destroy(gameObject);
        }

        scoreText = GameObject.Find("Score");
        block_destroy = BlockGenerator.instance.BlockDestroy();

        this.playerRg = GetComponent<Rigidbody2D>();
        blockRg = GetComponent<Rigidbody2D>();
                
        playerRg.velocity = Vector2.zero;
        player.AddComponent<PlayerStatusManager>();
    }

    void Update()
    {
        // 뒤로가기 버튼
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Time.timeScale = 0;     // 일시정지 -> 나중에 사용
            SceneManager.LoadScene("Main");
        }

        scoreText.GetComponent<Text>().text = this.score.ToString();    // UI에 점수 갱신

        if (BlockGenerator.instance.block_ypos_min)
        {
            //Debug.Log("block_ypos_min : " + BlockGenerator.instance.block_ypos_min);
            col_player.isTrigger = true;
            playerRg.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
        else
        {
            col_player.isTrigger = false;
            playerRg.constraints = RigidbodyConstraints2D.None;
            playerRg.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerRg.constraints = RigidbodyConstraints2D.FreezePositionX;
        }


        BlockDestroy();


        if (Input.touchCount > 0)
        {    //터치가 1개 이상이면.
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (EventSystem.current.IsPointerOverGameObject(i) == false)
                {
                    tempTouchs = Input.GetTouch(i);
                    if (tempTouchs.phase == TouchPhase.Began)
                    {    //해당 터치가 시작됐다면.
                        var touchedPos = Camera.main.ScreenToWorldPoint(tempTouchs.position);
                        attackOn = true;
                        //Debug.Log("attackOn : " + attackOn);
                        break;   //한 프레임(update)에는 하나만.
                    }
                    else if (tempTouchs.phase == TouchPhase.Ended)  // 터치가 끝났다면.
                    {
                        //Debug.Log("attack 버튼 터치 End");
                        attackOn = false;
                    }
                }
            }
        }
        //else if(tempTouchs.phase == TouchPhase.Ended)
        //else  // 터치가 없으면
        //{
        //    attackOn = false;
        //    //Debug.Log("attackOn : " + attackOn);
        //}
    }

    public void Jump()
    {
        // 땅에 충돌되어있을 때만 점프.
        if(ground_collsion)
            playerRg.AddForce(new Vector2(0, jumpSpeed));
       
    }

    public void Attack()    // pc 테스트용 공격 함수
    {      
        attackOn = true;        
    }

    public void Shield()    // 방어 버튼
    {
        shieldOn = true;
        // 건물의 콜라이더 활성화
        // 블럭의 콜라이더를 건물의 콜라이더 자식으로 넣는다.
        
        //if (shield_able)
        //{
        //    col_parent.enabled = true;
        //    for (int i = 0; i < 5; ++i)
        //    {
        //        blockArrCol[i].transform.parent = col_parent.transform;
        //        blockRg.AddForce(new Vector2(0, jumpSpeed));
        //    }
        //    //col_origin.GetComponent<Collider2D>().enabled = true;
        //    //blockRg.AddForce(new Vector2(0, 100.0f));
        //    //col_origin.transform.Translate(0,
        //    //        col_origin.transform.position.y + 3, 0);
        //    ////shieldOn = false;
        //    //col_origin.GetComponent<Collider2D>().enabled = false;
        //}
        //else
        //    col_parent.enabled = false;

    }

    // 파괴할 블럭 처리
    void BlockDestroy()
    {
        if(attackOn)   // 플레이어의 트리거가 켜져있을 때 공격 버튼이 눌리면.
        {
            if (destroy_block == null)
                return;

            int destroy_block_score = 0;    // 파괴할 블럭의 점수

            if (destroy_block.tag == "block5")   // 마지막 블럭일 때, 부모 삭제
            {
                GameObject parent = destroy_block.transform.parent.gameObject;
                destroy_block_score = destroy_block.GetComponent<BlockStatusManager>().score;
                Destroy(parent);
                score += destroy_block_score;
              //  Debug.Log("score : " + score);
                //score += BlockStatusManager.instance.score;
            }
            else
            {
                destroy_block_score = destroy_block.GetComponent<BlockStatusManager>().score;
                Destroy(destroy_block);
                score += destroy_block_score;
                //score += BlockStatusManager.instance.score;
               // Debug.Log("score : " + score);
            }
            attackOn = false;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("OnCollisionEnter2D");

        // 땅과 충돌
        if (collision.collider.tag == "Collision")
            ground_collsion = true;

        // 블럭과 충돌
        if (collision.collider.tag == ("block1") || collision.collider.tag == ("block2")
            || collision.collider.tag == ("block3") || collision.collider.tag == ("block4")
            || collision.collider.tag == ("block5"))
        {
            int destroy_block_score = 0;    // 파괴할 블럭의 점수

            //shield_able = true;
            GameObject newObj = collision.collider.gameObject;
            destroy_block_score = newObj.GetComponent<BlockStatusManager>().score;
            if (attackOn)  // 공격 버튼만 눌렀을 때.
            {
                Destroy(newObj.gameObject);
                score += destroy_block_score;
               // Debug.Log("score : " + score);
                attackOn = false;
            }

        }

        //if (shieldOn)
        //{
        //    // 컬라이더 활성화
        //    col_origin.GetComponent<Collider2D>().enabled = true;
        //    col_origin.transform.Translate(0,
        //           col_origin.transform.position.y + 3, 0);
        //    shieldOn = false;
        //    col_origin.GetComponent<Collider2D>().enabled = false;
        //}

        //if (collision.collider.tag == "BlockPrefab")
        //{
        //    if (shieldOn)    // 방어 누른 채로 블록과 충돌
        //    {
        //        //Debug.Log("방어 누른 채로 블록과 충돌");
        //        collision.collider.transform.Translate(0,
        //            collision.collider.transform.position.y + 3, 0);
        //    }
        //    shieldOn = false;
        //}
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
       // Debug.Log("OnCollisionStay2D");
     
        // 땅과 충돌
        if (collision.collider.tag == "Collision")
            ground_collsion = true;

        // 블럭과 충돌
        if (collision.collider.tag == ("block1") || collision.collider.tag == ("block2")
            || collision.collider.tag == ("block3") || collision.collider.tag == ("block4")
            || collision.collider.tag == ("block5"))
        {
            //shield_able = true;
            int destroy_block_score = 0;
            GameObject newObj = collision.collider.gameObject;
            destroy_block_score = newObj.GetComponent<BlockStatusManager>().score;

            if (attackOn)  // 공격 버튼만 눌렀을 때.
            {
                Destroy(newObj.gameObject);
                score += destroy_block_score;
               // Debug.Log("score : " + score);
                attackOn = false;
            }
        }

        //if (shieldOn)
        //{
        //    // 컬라이더 활성화
        //    col_origin.GetComponent<Collider2D>().enabled = true;
        //    col_origin.transform.Translate(0,
        //           col_origin.transform.position.y + 3, 0);
        //    shieldOn = false;
        //    col_origin.GetComponent<Collider2D>().enabled = false;
        //}

        //if (collision.collider.tag == "BlockPrefab")
        //{
        //    if (shieldOn)    // 방어 누른 채로 블록과 충돌
        //    {
        //        //Debug.Log("방어 누른 채로 블록과 충돌");
        //        collision.collider.transform.Translate(0,
        //            collision.collider.transform.position.y + 3, 0);

        //    }
        //    shieldOn = false;
        //}
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log("OnCollisionExit2D");
        col_player.isTrigger = false;
        // 플레이어 y좌표 freeze 되있던 것을 초기화
        playerRg.constraints = RigidbodyConstraints2D.None;
        playerRg.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerRg.constraints = RigidbodyConstraints2D.FreezePositionX;

        if (collision.collider.tag == ("block1") || collision.collider.tag == ("block2") ||
            collision.collider.tag == ("block3") || collision.collider.tag == ("block4") ||
            collision.collider.tag == ("block5"))
        {
            shield_able = false;
            block_destroy = false;
            col_player.isTrigger = false;
            // 플레이어 y좌표 freeze 되있던 것을 초기화
            playerRg.constraints = RigidbodyConstraints2D.None;
            playerRg.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerRg.constraints = RigidbodyConstraints2D.FreezePositionX;
        }

        if (collision.collider.tag == "Collision")
            ground_collsion = false;
    }

    // 트리거 함수 자체는 플레이어의 트리거가 true일 때만 발동.
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("OnTriggerEnter2D");
        GameObject newObj = collision.gameObject;
        if (collision.tag == ("block1") || collision.tag == ("block2")
            || collision.tag == ("block3") || collision.tag == ("block4")
            || collision.tag == ("block5"))
        {
            shield_able = true;
            destroy_block = collision.gameObject;
            //if (attackOn) // 터치된 상태로, 블록과 충돌
            //{
            //    //Debug.Log("터치된 상태로 블록과 충돌, OnTriggerEnter2D");
            //    Destroy(newObj);
            //    attackOn = false;
            //}
        }
    }

    void BlockDestroy(Collider2D collision)
    {
        //Debug.Log("BlockDestroy 호출");
        GameObject newObj = collision.gameObject;
        Destroy(newObj);
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("OnTriggerStay2D");
        
        // GameObject newObj = collision.gameObject;
        //if (collision.tag == ("block1") || collision.tag == ("block2")
        //    || collision.tag == ("block3") || collision.tag == ("block4")
        //    || collision.tag == ("block5"))
        //{
        //    block_destroy = true;
        //    // Debug.Log("블록과 충돌중");
        //    if (attackOn) // 터치된 상태로, 블록과 충돌
        //    {
        //        col_new = collision;
        //        attackOn = false;
        //        //Destroy(newObj);
        //    }        
        //}

        // 방어 버튼 눌릴때만 전체 컬라이더 활성화
        //if (shieldOn)
        //{
        //    // 컬라이더 활성화
        //   // coll_origin.enabled = true;
        //    //coll_origin.transform.Translate(0,
        //    //       coll_origin.transform.position.y + 3, 0);

        //    shieldOn = false;
        //}
        //else
        //{
        //    coll_origin.enabled = false;
        //}

        //if (collision.tag == "BlockPrefab")
        //{
        //    if (shieldOn)    // 방어 누른 채로 블록과 충돌
        //    {
        //        collision.transform.Translate(0,
        //            collision.transform.position.y + 3, 0);
        //    }
        //    shieldOn = false;
        //}
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        shield_able = false;
        col_player.isTrigger = false;
        ground_collsion = false;
        playerRg.constraints = RigidbodyConstraints2D.None;
        playerRg.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerRg.constraints = RigidbodyConstraints2D.FreezePositionX;

        if (/*collision.tag == ("block1") || collision.tag == ("block2")
            || collision.tag == ("block3") || collision.tag == ("block4")
            ||*/ collision.tag == ("block5"))
        {
            col_player.isTrigger = false;
            playerRg.constraints = RigidbodyConstraints2D.None;
            playerRg.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerRg.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }
}
