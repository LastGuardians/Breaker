using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour {
    
    public float jumpSpeed = 200f;
    Rigidbody2D playerRg;
    public Collider2D col_player;
    public bool isCollision = false;

    public bool touchOn = false;
      
    public Button jumpBut;

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

        this.playerRg = GetComponent<Rigidbody2D>();
        //Player.transform.position = new Vector2(0, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Time.timeScale = 0;     // 일시정지 -> 나중에 사용
            SceneManager.LoadScene("Main");
        }
        

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
                        touchOn = true;
                        Debug.Log("touchOn : " + touchOn);
                        break;   //한 프레임(update)에는 하나만.
                    }
                }
            }
        }
        else if(tempTouchs.phase == TouchPhase.Ended)
        {
            touchOn = false;
            //Debug.Log("touchOn : " + touchOn);
        }
        
            
    }

    public void Jump()
    {
        //Debug.Log("Jump키 눌림");
        isCollision = false;
        if (playerRg.velocity.y == 0)
        {
            playerRg.AddForce(new Vector2(0, jumpSpeed));
        }
    }

    public void Attack()    // pc 테스트용 공격 함수
    {      
        touchOn = true;
        
        //if (Input.GetMouseButtonUp(0))
        //{
        //    Debug.Log("attack 버튼 up");
        //    touchOn = false;
        //}
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == ("Collision"))
        {
            isCollision = true;
            //Debug.Log("바닥과 충돌중");
        }

        if ((collision.collider.tag == ("block1") || collision.collider.tag == ("block2")
            || collision.collider.tag == ("block3") || collision.collider.tag == ("block4")
            || collision.collider.tag == ("block5")))
        {
            GameObject newObj = collision.collider.gameObject;
            if (touchOn) // 터치된 상태로, 블록과 충돌
            {
                Debug.Log("터치된 상태로 블록과 충돌");
                // 블록 destroy
                Destroy(newObj);
                touchOn = false;
            }
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == ("Collision"))
        {
            isCollision = true;
            //Debug.Log("바닥과 충돌중");
        }

        if (collision.collider.tag == ("block1") || collision.collider.tag == ("block2"))
        {
            //col_player.isTrigger = true;
            if (isCollision)
            {
                //Debug.Log("블록과 충돌");
                col_player.isTrigger = true;
                playerRg.constraints = RigidbodyConstraints2D.FreezePositionY;
            }
            
        }

        //if (collision.collider.tag == ("BlockPrefab"))
        //{
        //    if (collision.collider.tag == ("Collision"))
        //    {
        //        col_player.isTrigger = true;
        //        Debug.Log("onCollision");
        //    }
        //}
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == ("Collision"))
        {
            isCollision = false;
        }

        if (collision.collider.tag == ("block1") || collision.collider.tag == ("block2") ||
            collision.collider.tag == ("block3")|| collision.collider.tag == ("block4") ||
            collision.collider.tag == ("block5"))
        {
            isCollision = false;
            col_player.isTrigger = false;
            playerRg.constraints = RigidbodyConstraints2D.None;
            playerRg.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerRg.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject newObj = collision.gameObject;
        if (collision.tag == ("block1") || collision.tag == ("block2")
            || collision.tag == ("block3") || collision.tag == ("block4")
            || collision.tag == ("block5"))
        {
            Debug.Log("블록과 충돌");
            if (touchOn) // 터치된 상태로, 블록과 충돌
            {
                Debug.Log("터치된 상태로 블록과 충돌, OnTriggerEnter2D");
                Destroy(newObj);
                touchOn = false;
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        GameObject newObj = collision.gameObject;
        if (collision.tag == ("block1") || collision.tag == ("block2")
             || collision.tag == ("block3") || collision.tag == ("block4")
            || collision.tag == ("block5"))
        {
           // Debug.Log("블록과 충돌중");
            if (touchOn) // 터치된 상태로, 블록과 충돌
            {
                Debug.Log("터치된 상태로 블록과 충돌, OnTriggerStay2D");
                Destroy(newObj);
                touchOn = false;
            }

        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == ("block1") || collision.tag == ("block2")
             || collision.tag == ("block3") || collision.tag == ("block4")
            || collision.tag == ("block5"))
        {
            isCollision = false;
            col_player.isTrigger = false;
            playerRg.constraints = RigidbodyConstraints2D.None;
            playerRg.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerRg.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }
}
