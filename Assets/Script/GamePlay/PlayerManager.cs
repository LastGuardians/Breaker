using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour {
    
    float jumpSpeed = 800f;   // 점프 버튼 눌렀을 때 적용되는 힘
    float jumpSpeedCollision = 300f;    // 공중에서 블럭 파괴했을 때 적용되는 힘
    float shieldForcetoPlayer = -400f;  // 방어했을 때 플레이어에 적용되는 힘
    float shieldForcetoBlock = 500f;    // 방어했을 때 블럭에 적용되는 힘

    [Header("Player")]
    public Rigidbody2D playerRg;       // 플레이어 리지드바디
    public Collider2D col_player;   // 플레이어의 컬라이더
    public GameObject player;       // 플레이어 오브젝트
    public GameObject jump_effect;

    //public bool block_drop_min = false; // 블럭이 최소 좌표에 도달했는지 확인하는 변수

    [Header("StateCheck")]
    public bool attackOn = false;    // 공격 버튼이 터치 되었는지 확인하는 변수
    bool shieldOn = false;   // 방어 버튼이 터치 되었는지 확인
    public bool ground_collsion = false;     // 플레이어가 땅과 충돌되었는지 확인하는 변수
    public bool shield_able = false;   // 방어가 가능한 상태인지 판단.
    bool jumpOn = false;

    [Header ("Block")]
    public bool block_destroy = false;   // 블럭이 모두 파괴되었는지 확인하는 변수
    public Rigidbody2D blockRg;        // 블럭 리지드바디
    public Collider2D col_parent;       // 블럭의 부모 컬라이더(활성/비활성화용)
    public GameObject destroy_block;    // 파괴할 블럭 
    public GameObject parent;

    private Touch tempTouchs;

    [Header("UI")]
    public int score = 0;   // test용 score
    public int life = 0;    // test용 life
    public Button jumpButton;   // 점프 버튼 
    GameObject scoreText;   // score UI
    GameObject lifeSlider;  // 생명 UI 
    GameObject buttonCool;  // 쿨타임 오브젝트

    AudioClip jumping_sound;


    [Header("URL")]
    public string baseUrl = "http://ec2-18-220-97-254.us-east-2.compute.amazonaws.com/prisoncrush";

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
        lifeSlider = GameObject.Find("Life");

        jumpButton.GetComponent<Button>();

        block_destroy = BlockGenerator.instance.BlockDestroy();

        this.playerRg = GetComponent<Rigidbody2D>();
        blockRg = GameObject.Find("BlockGroup").GetComponent<Rigidbody2D>();
                
        playerRg.velocity = Vector2.zero;
        player.AddComponent<PlayerStatusManager>();

        buttonCool = GameObject.Find("ShieldButton");
    }

    void Update()
    {
        
        // UI 갱신
        scoreText.GetComponent<Text>().text = this.score.ToString();    // UI에 점수 갱신
        lifeSlider.GetComponent<Slider>().value = life;

        // 블럭이 일정 좌표 이상 내려오면 플레이어 트리거 활성화.
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

        //jumpButton.onClick.AddListener(Jump);

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
                        GlobalSFX.instance.PlayWeaponSwingSound();
                        transform.Find("swing").gameObject.SetActive(true);
                        // 점프했을 때 애니메이션.
                        if (jumpOn)
                        {
                            //Debug.Log("jumpOn: " + jumpOn);
                            CharacterAnimation.instance.CatJumpAttackAniControll();
                        }
                        else
                            CharacterAnimation.instance.CatAttackAniControll();

                        var touchedPos = Camera.main.ScreenToWorldPoint(tempTouchs.position);
                        attackOn = true;
                        //Debug.Log("attackOn : " + attackOn);
                        break;   //한 프레임(update)에는 하나만.
                    }
                    else if (tempTouchs.phase == TouchPhase.Ended)  // 터치가 끝났다면.
                    {
                        //Debug.Log("attack 버튼 터치 End");
                        attackOn = false;
                        transform.Find("swing").gameObject.SetActive(false);
                        //CharacterAnimation.instance.CatAttackAniControll(false);
                    }
                }
            }
        }

        if(life == 3)   // 게임 오버
        {
            GameObject gpgs = GameObject.Find("GPGSManager");
            if (gpgs != null)
            {
                GPGSManager.instance.game_score = score;
                SceneManager.LoadScene("Result");
                gpgs.GetComponent<GPGSManager>().ReportScore(score);
                StartCoroutine(_CreateRank(gpgs.GetComponent<GPGSManager>().mainplayeruserdata.userName,
                    GameManager.instance.game_score));
            }
        }

        if (playerRg.velocity.y != 0)
        {
            jumpOn = true;

        }
        if (ground_collsion)
        {
            //Debug.Log("jumpOn:" + jumpOn);
            jumpOn = false;
        }

        // 트리거 켜졌을 때 방어버튼 누르면.(조건: 블럭과 충돌했을때 && 방어버튼 눌렸을때 && 트리거 켜졌을때)
        if (shield_able)
        {
            if (shieldOn)
            {
                //Debug.Log("shieldOn: " + shieldOn);
                if (blockRg == null)
                    blockRg = GameObject.Find("BlockGroup(Clone)").GetComponent<Rigidbody2D>();
                blockRg.AddForce(new Vector2(0, shieldForcetoBlock));
                shieldOn = false;
            }
        }

        if (attackOn && col_player.isTrigger == false)
        {            
            if (PlayerCollision.instance.rayCollider != null)
            {
                int destroy_block_score = 0;
                GameObject newObj = PlayerCollision.instance.rayCollider.gameObject;
                destroy_block_score = newObj.GetComponent<BlockStatusManager>().score;

                if (PlayerCollision.instance.rayCollider.tag == "block5")
                {
                    parent = PlayerCollision.instance.rayCollider.transform.parent.gameObject;
                    GlobalSFX.instance.PlayDestroySound();
                    score += destroy_block_score;
                    GameObject.Find("FeverManager").GetComponent<FeverTime>().block_destroy_count += 1;
                    Destroy(parent);
                }
                else
                {
                    GlobalSFX.instance.PlayDestroySound();
                    score += destroy_block_score;
                    GameObject.Find("FeverManager").GetComponent<FeverTime>().block_destroy_count += 1;
                    Destroy(PlayerCollision.instance.rayCollider);
                }
            }
        }
    }

    /* 랭킹 생성 */
    public IEnumerator _CreateRank(string userId, int score)
    {
        string url = baseUrl + "/rank/create";
        WWWForm form = new WWWForm();
        form.headers["content-type"] = "application/json";
        form.AddField("userId", userId);
        form.AddField("score", score);

        WWW www = new WWW(url, form);
        yield return www;

        //PrintLog(www.error);
    }

    public void Jump()
    {
        // 땅에 충돌되어있을 때만 점프 가능.
        if (ground_collsion)
        {
            //StartCoroutine(PlayerJumpControll());
            jump_effect.SetActive(true);
            playerRg.AddForce(new Vector2(0, jumpSpeed));
            GlobalSFX.instance.PlayJumpSound();
            CharacterAnimation.instance.CatJumpAniControll();
            //CharacterAnimation.instance.CatJumpAttackAniControll();
        }
    }

    public void JumpCancle()    // 점프 버튼 뗐을 때
    {
        jump_effect.SetActive(false);
    }

    public void Attack()    // pc 테스트용 공격 함수
    {      
        attackOn = true;
        transform.Find("swing").gameObject.SetActive(true);

        if (jumpOn)
        {
            //Debug.Log("jumpOn: " + jumpOn);
            CharacterAnimation.instance.CatJumpAttackAniControll();
        }
        else
            CharacterAnimation.instance.CatAttackAniControll();
        GlobalSFX.instance.PlayWeaponSwingSound();
    }

    public void AttackCancle()
    {
        attackOn = false;
        transform.Find("swing").gameObject.SetActive(false);
    }

    public void Shield()    // 방어 버튼
    {
        // 쿨타임 시작
        if (buttonCool.GetComponent<ButtonCoolTime>().canUseShield)
        {
            buttonCool.GetComponent<ButtonCoolTime>().UseShield();
            shieldOn = true;
        }
    }

    public void ShieldCancle()
    {
        //if(!buttonCool.GetComponent<ButtonCoolTime>().canUseShield)
        shieldOn = false;
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
                parent = destroy_block.transform.parent.gameObject;
                destroy_block_score = destroy_block.GetComponent<BlockStatusManager>().score;
                Destroy(parent);
                GlobalSFX.instance.PlayDestroySound();
                GameObject.Find("FeverManager").GetComponent<FeverTime>().block_destroy_count += 1;              
                score += destroy_block_score;
            }
            else
            {
                destroy_block_score = destroy_block.GetComponent<BlockStatusManager>().score;
                Destroy(destroy_block);
                GlobalSFX.instance.PlayDestroySound();
                GameObject.Find("FeverManager").GetComponent<FeverTime>().block_destroy_count += 1;
                score += destroy_block_score;
            }
            attackOn = false;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("OnCollisionEnter2D");        

        // 땅과 충돌
        if (collision.collider.tag == "Collision")
        {
            ground_collsion = true;
            //CharacterAnimation.instance.CatJumpAniControll(false);
        }

        // 블럭과 충돌
        if (collision.collider.tag == ("block1") || collision.collider.tag == ("block2")
            || collision.collider.tag == ("block3") || collision.collider.tag == ("block4")
            || collision.collider.tag == ("block5"))
        {
            int destroy_block_score = 0;    // 파괴할 블럭의 점수

            playerRg.AddForce(new Vector2(0, 0));
            //shield_able = true;
            GameObject newObj = collision.collider.gameObject;
            destroy_block_score = newObj.GetComponent<BlockStatusManager>().score;
            if (attackOn)  // 공격 버튼만 눌렀을 때.
            {                
                if (collision.collider.tag == "block5")   // 마지막 블럭일 때, 부모 삭제
                {
                    parent = newObj.transform.parent.gameObject;
                    Destroy(parent);
                    GlobalSFX.instance.PlayDestroySound();
                    GameObject.Find("FeverManager").GetComponent<FeverTime>().block_destroy_count += 1;
                    score += destroy_block_score;
                    attackOn = false;
                }
                else
                {
                    Destroy(newObj.gameObject);
                    playerRg.AddForce(new Vector2(0, jumpSpeedCollision));
                    GlobalSFX.instance.PlayDestroySound();
                    GameObject.Find("FeverManager").GetComponent<FeverTime>().block_destroy_count += 1;
                    score += destroy_block_score;
                    attackOn = false;
                }
            }

            if (shieldOn)
            {
                if (blockRg == null)
                    blockRg = GameObject.Find("BlockGroup(Clone)").GetComponent<Rigidbody2D>();
                blockRg.AddForce(new Vector2(0, shieldForcetoBlock));
                PlayerManager.instance.playerRg.AddForce(new Vector2(0, shieldForcetoPlayer));
            }

        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
       // Debug.Log("OnCollisionStay2D");
     
        // 땅과 충돌
        if (collision.collider.tag == "Collision")
        {
            ground_collsion = true;
            //CharacterAnimation.instance.CatJumpAniControll(false);
        }

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
                if (collision.collider.tag == "block5")   // 마지막 블럭일 때, 부모 삭제
                {
                    parent = newObj.transform.parent.gameObject;
                    Destroy(parent);
                    GlobalSFX.instance.PlayDestroySound();
                    GameObject.Find("FeverManager").GetComponent<FeverTime>().block_destroy_count += 1;
                    score += destroy_block_score;
                    attackOn = false;
                }
                else
                {
                    Destroy(newObj.gameObject);
                    playerRg.AddForce(new Vector2(0, jumpSpeedCollision));
                    GlobalSFX.instance.PlayDestroySound();
                    GameObject.Find("FeverManager").GetComponent<FeverTime>().block_destroy_count += 1;
                    score += destroy_block_score;
                    // Debug.Log("score : " + score);
                    attackOn = false;
                }
            }

            if (shieldOn)
            {
                if(blockRg == null)
                    blockRg = GameObject.Find("BlockGroup(Clone)").GetComponent<Rigidbody2D>();
                blockRg.AddForce(new Vector2(0, shieldForcetoBlock));
                PlayerManager.instance.playerRg.AddForce(new Vector2(0, shieldForcetoPlayer));
            }
        }

      
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
            //Debug.Log("collision.tag : " + collision.tag);
            shield_able = true;
            if (destroy_block != null)
            {
                return;
            }
            destroy_block = collision.gameObject;
        }
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
