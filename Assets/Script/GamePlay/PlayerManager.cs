using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour {
    
    public float jumpSpeed = 200f;
    Rigidbody2D playerRg;
    public Collider2D col_player;   // 플레이어의 컬라이더
    public GameObject player;       // 플레이어 오브젝트
    public User user_data = new User(); // 유저 구조체 정보 -> 플레이어 오브젝트에 붙임.
    //public Collider2D col_temp = new Collider2D();     // 트리거 함수의 매개변수 컬라이더 
    public bool isCollision = false;

    public GameObject block_origin; // hierarchy 상에 원래 존재하던 블럭 오브젝트
    public GameObject block_new;    // 새로 생성한 블럭 오브젝트
    BoxCollider2D coll_origin;       // origin 블럭의 컬라이더
    BoxCollider2D coll_new;         // new 블럭의 컬라이더


    //public GameObject[] blockArr = new GameObject[5];  // 블럭 좌표를 확인하기 위한 배열

    public bool block_drop_min = false; // 블럭이 최소 좌표에 도달했는지 확인하는 변수

    public bool attackOn = false;    // 공격 버튼이 터치 되었는지 확인하는 변수
    public bool shieldOn = false;   // 방어 버튼이 터치 되었는지 확인

    public bool block_destroy = false;   // 블럭이 모두 파괴되었는지 확인하는 변수
    public bool trigger_on = false;     // 플레이어의 트리거 활성화 여부 판단

    private Touch tempTouchs;

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

        block_destroy = BlockGenerator.instance.BlockDestroy();

        this.playerRg = GetComponent<Rigidbody2D>();
        block_origin.AddComponent<BoxCollider>();

        coll_origin = block_origin.GetComponent<BoxCollider2D>();

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

        // 블록 y좌표가 -0.5 미만이라면( = 플레이어와 충돌했다면)   
        //if ((block_origin.transform.position.y < -0.4f && block_origin.transform.position.y >= -1.7)
        //    || (block_new.transform.position.y < -0.4f && block_new.transform.position.y >= -1.7f))
        //{
        //    col_player.isTrigger = true;
        //    playerRg.constraints = RigidbodyConstraints2D.FreezePositionY;
        //}
        //// 블럭 y좌표가 -0.5 이상이거나, 블럭이 모두 파괴되었다면
        //else if (block_origin.transform.position.y >= -0.6f || block_destroy
        //    || block_new.transform.position.y >= -0.6f)
        //{
        //    col_player.isTrigger = false;
        //    playerRg.constraints = RigidbodyConstraints2D.None;
        //    playerRg.constraints = RigidbodyConstraints2D.FreezeRotation;
        //    playerRg.constraints = RigidbodyConstraints2D.FreezePositionX;
        //}
        //else
        //    block_drop_min = false;


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
                        Debug.Log("attackOn : " + attackOn);
                        break;   //한 프레임(update)에는 하나만.
                    }
                    else if (tempTouchs.phase == TouchPhase.Ended)  // 터치가 끝났다면.
                    {
                        Debug.Log("attack 버튼 터치 End");
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
        //Debug.Log("Jump키 눌림");
        if (playerRg.velocity.y == 0)
        {
            playerRg.AddForce(new Vector2(0, jumpSpeed));
        }
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
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // 블럭 좌표가 -0.5 미만일 때
        if (collision.collider.transform.position.y < -0.4
            && collision.collider.transform.position.y >= -1.7)
        {
            //Debug.Log("collisionEnter에서 y좌표 0.5 미만일 때");
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


        //BoxCollider temp = (BoxCollider)(block_origin.GetComponent<Collider>());

        // 블럭과 충돌
        if (collision.collider.tag == ("block1") || collision.collider.tag == ("block2")
            || collision.collider.tag == ("block3") || collision.collider.tag == ("block4")
            || collision.collider.tag == ("block5"))
        {
          
            GameObject newObj = collision.collider.gameObject;
            if (attackOn) // 공격 누른 상태로 블록과 충돌
            {
                // 블록 destroy
                Destroy(newObj.gameObject);
                // 블록 컬라이더 사이즈 축소
                //coll_origin.size = new Vector2(coll_origin.size.x, coll_origin.size.y - (float)1.4);
                attackOn = false;
            }
                       
        }

        //if(shieldOn)
        //{
        //    // 컬라이더 활성화
        //    coll_origin.enabled = true;
        //    coll_origin.transform.Translate(0,
        //           coll_origin.transform.position.y + 3, 0);
        //    shieldOn = false;
        //}
        //else
        //{
        //    coll_origin.enabled = false;
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
        // 블럭 좌표가 -0.5 미만일 때
        if (collision.collider.transform.position.y < -0.4
            && collision.collider.transform.position.y >= -1.7)
        {
            //Debug.Log("collisionEnter에서 y좌표 0.5 미만일 때");
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

        // 블럭과 충돌
        if (collision.collider.tag == ("block1") || collision.collider.tag == ("block2")
            || collision.collider.tag == ("block3") || collision.collider.tag == ("block4")
            || collision.collider.tag == ("block5"))
        {          

            GameObject newObj = collision.collider.gameObject;
            
            if (attackOn) // 터치된 상태로, 블록과 충돌
            {
                // 블록 destroy
                Destroy(newObj.gameObject);
                // 블록 컬라이더 사이즈 축소
                //coll_origin.size = new Vector2(coll_origin.size.x, coll_origin.size.y - (float)1.4);
                attackOn = false;
            }

        }

        //if (shieldOn)
        //{
        //    // 컬라이더 활성화
        //    coll_origin.enabled = true;
        //    coll_origin.transform.Translate(0,
        //           coll_origin.transform.position.y + 3, 0);
        //    shieldOn = false;
        //}
        //else
        //{
        //    coll_origin.enabled = false;
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
        Debug.Log("OnCollisionExit2D");
        col_player.isTrigger = false;
        // 플레이어 y좌표 freeze 되있던 것을 초기화
        playerRg.constraints = RigidbodyConstraints2D.None;
        playerRg.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerRg.constraints = RigidbodyConstraints2D.FreezePositionX;

        if (collision.collider.tag == ("block1") || collision.collider.tag == ("block2") ||
            collision.collider.tag == ("block3") || collision.collider.tag == ("block4") ||
            collision.collider.tag == ("block5"))
        {
            col_player.isTrigger = false;
            // 플레이어 y좌표 freeze 되있던 것을 초기화
            playerRg.constraints = RigidbodyConstraints2D.None;
            playerRg.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerRg.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }

    // 트리거 함수 자체는 플레이어의 트리거가 true일 때만 발동.
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D");
        GameObject newObj = collision.gameObject;
        if (collision.tag == ("block1") || collision.tag == ("block2")
            || collision.tag == ("block3") || collision.tag == ("block4")
            || collision.tag == ("block5"))
        {
            //Debug.Log("블록과 충돌");
            if (attackOn) // 터치된 상태로, 블록과 충돌
            {
                //Debug.Log("터치된 상태로 블록과 충돌, OnTriggerEnter2D");
                Destroy(newObj);
                attackOn = false;
            }
        }
    }

    void BlockDestroy(Collider2D collision)
    {
        Debug.Log("BlockDestroy 호출");
        GameObject newObj = collision.gameObject;
        Destroy(newObj);
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("OnTriggerStay2D");

       // GameObject newObj = collision.gameObject;
        if (collision.tag == ("block1") || collision.tag == ("block2")
            || collision.tag == ("block3") || collision.tag == ("block4")
            || collision.tag == ("block5"))
        {
            // Debug.Log("블록과 충돌중");
            if (attackOn) // 터치된 상태로, 블록과 충돌
            {
                BlockDestroy(collision);
                attackOn = false;
                //Destroy(newObj);
            }

        
        }

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
        col_player.isTrigger = false;
        playerRg.constraints = RigidbodyConstraints2D.None;
        playerRg.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerRg.constraints = RigidbodyConstraints2D.FreezePositionX;

        if (/*collision.tag == ("block1") || collision.tag == ("block2")
            || collision.tag == ("block3") || collision.tag == ("block4")
            ||*/ collision.tag == ("block5"))
        {
            coll_origin.enabled = false;
            col_player.isTrigger = false;
            playerRg.constraints = RigidbodyConstraints2D.None;
            playerRg.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerRg.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }
}
