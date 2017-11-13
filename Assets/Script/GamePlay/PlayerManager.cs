using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour {
    
    float jumpSpeed = 1600f;   // 점프 버튼 눌렀을 때 적용되는 힘
    float jumpSpeedCollision = 150f;    // 공중에서 블럭 파괴했을 때 적용되는 힘
    float shieldForcetoPlayer = -400f;  // 방어했을 때 플레이어에 적용되는 힘
    float shieldForcetoBlock = 1200f;    // 방어했을 때 블럭에 적용되는 힘
    float aniTime = 0;

    [Header("Player")]
    public Rigidbody2D playerRg;       // 플레이어 리지드바디
    public Collider2D col_player;   // 플레이어의 컬라이더
    public GameObject player;       // 플레이어 오브젝트
    public GameObject jump_effect;
    static private float damage = 0f;  // 데미지    
    static private float critical = 0f;  // 크리티컬 데미지
    static private float probablity = 0f;  // 크리티컬 확률
    static private float coin = 0;
    static private int key = 0;
    public GameObject ShieldCollider;  // 쉴드 콜라이더
    public GameObject AttackEffect;
    public GameObject SwordEffect;
    public GameObject BombEffect;
    public string BlockParticleName = "BlockParticle";
    public string CoinParticleName = "CoinParticle";
    public string AttackEffectName = "AttackEffect";
    public string BombEffectName = "BombEffect";
    float originDamage;

    //public bool block_drop_min = false; // 블럭이 최소 좌표에 도달했는지 확인하는 변수

    [Header("StateCheck")]
    public bool attackOn = false;    // 공격 버튼이 터치 되었는지 확인하는 변수
    bool shieldOn = false;   // 방어 버튼이 터치 되었는지 확인
    public bool ground_collsion = false;     // 플레이어가 땅과 충돌되었는지 확인하는 변수
    public bool shield_able = false;   // 방어가 가능한 상태인지 판단.
    public bool jumpOn = false;
    bool jumpButClick = false;
    public int blockCnt = 0;   // 파괴된 블럭 개수
    public bool criticalStart = false;

    [Header ("Block")]
    public bool isDestroy = false;      // 블럭이 파괴되었는지 확인하는 변수
    public Rigidbody2D blockRg;        // 블럭 리지드바디
    public Collider2D col_parent;       // 블럭의 부모 컬라이더(활성/비활성화용)
    public GameObject destroy_block;    // 파괴할 블럭 
    public GameObject parent;
    public GameObject DestroyParticle;  // 파괴 파티클
    public GameObject CoinParticle;     // 코인 파티클
    public GameObject ShieldEffect;

    private Touch tempTouchs;

    [Header("UI")]
    public int score = 0;   // test용 score
    public int life = 0;    // test용 life
    public Button jumpButton;   // 점프 버튼 
    GameObject scoreText;   // score UI
    GameObject lifeSlider;  // 생명 UI 
    GameObject buttonCool;  // 쿨타임 오브젝트
    public GameObject CriticalText;

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

        this.playerRg = GetComponent<Rigidbody2D>();
        blockRg = GameObject.Find("BlockGroup").GetComponent<Rigidbody2D>();
                
        playerRg.velocity = Vector2.zero;
        player.AddComponent<PlayerStatusManager>();

        buttonCool = GameObject.Find("ShieldButton");

        StartCoroutine(LifeCheck());
        StartCoroutine(JumpCheck());
        StartCoroutine(PlayerYLimit());
        StartCoroutine(PlayerTriggerCheck());
        StartCoroutine(GroundCollision());
        StartCoroutine(RayCollisionCheck());
        StartCoroutine(GroundNotCollision());
        StartCoroutine(BlockDestroy());
        StartCoroutine(TouchCheck());
        StartCoroutine(ShieldAbleCheck());
        StartCoroutine(CriticalCheck());

        SetWeaponAbility();

        originDamage = damage;
    }

    void Update()
    {        
        // UI 갱신
        scoreText.GetComponent<Text>().text = this.score.ToString();    // UI에 점수 갱신
        lifeSlider.GetComponent<Slider>().value = life;
    }

    // 무기 능력치 셋팅
    void SetWeaponAbility()
    {
        //Debug.Log("UserConnect.CurrentWeaponIndex: " + UserConnect.CurrentWeaponIndex);
        if(UserConnect.CurrentWeaponIndex.Equals(0))    // 무기1(브론즈)
        {
            damage = 5 * (UserWeapon.WeaponAbilityArray[UserConnect.CurrentWeaponIndex, 0]) + 5;
            critical = 0.05f * (float)(UserWeapon.WeaponAbilityArray[UserConnect.CurrentWeaponIndex, 1]) + 1.2f;
            probablity = (UserWeapon.WeaponAbilityArray[UserConnect.CurrentWeaponIndex, 2]) + 10;
        }
        else if(UserConnect.CurrentWeaponIndex.Equals(1))   // 무기2(실버)
        {
            damage = 7.5f * (UserWeapon.WeaponAbilityArray[UserConnect.CurrentWeaponIndex, 0]) + 10;
            critical = 0.05f * (float)(UserWeapon.WeaponAbilityArray[UserConnect.CurrentWeaponIndex, 1]) + 1.5f;
            probablity = 1.1f * (UserWeapon.WeaponAbilityArray[UserConnect.CurrentWeaponIndex, 2]) + 15;
        }
        else if (UserConnect.CurrentWeaponIndex.Equals(2))   // 무기3(골드)
        {
            damage = 10 * (UserWeapon.WeaponAbilityArray[UserConnect.CurrentWeaponIndex, 0]) + 20;
            critical = 0.07f * (float)(UserWeapon.WeaponAbilityArray[UserConnect.CurrentWeaponIndex, 1]) + 1.75f;
            probablity = 1.2f * (UserWeapon.WeaponAbilityArray[UserConnect.CurrentWeaponIndex, 2]) + 20;
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

        Debug.Log(www.error);


        //PrintLog(www.error);
    }

    // 트리거 켜졌을 때 방어 체크
    public IEnumerator ShieldAbleCheck()
    {
        while (true)
        {
            yield return new WaitUntil(() => shield_able);
            if (shieldOn)
            {
                if (blockRg == null)
                {
                    if (destroy_block.CompareTag("block1") || destroy_block.CompareTag("block2")
                         || destroy_block.CompareTag("block3") || destroy_block.CompareTag("block4")
                         || destroy_block.CompareTag("block5") || destroy_block.CompareTag("rope")
                         || destroy_block.CompareTag("handcuffs") || destroy_block.CompareTag("portion"))
                        blockRg = GameObject.Find("BlockGroup(Clone)").GetComponent<Rigidbody2D>();

                    else if (destroy_block.CompareTag("FeverBlock1")
                        || destroy_block.CompareTag("FeverBlock2") || destroy_block.CompareTag("FeverBlock3")
                        || destroy_block.CompareTag("FeverBlock4") || destroy_block.CompareTag("FeverBlock5")
                        || destroy_block.CompareTag("FeverBlock6") || destroy_block.CompareTag("FeverBlock7")
                        || destroy_block.CompareTag("FeverBlock8") || destroy_block.CompareTag("FeverBlock9")
                        || destroy_block.CompareTag("FeverBlock10"))
                       blockRg = GameObject.Find("BlockGroupFever(Clone)").GetComponent<Rigidbody2D>();
                    
                }
                blockRg.AddForce(new Vector2(0, shieldForcetoBlock));
                blockRg.gravityScale = 0.5f;
                shieldOn = false;
            }
        }
    }

    // 생명 체크
    public IEnumerator LifeCheck()
    {
        while (true)
        {           
            yield return new WaitUntil(() => life >= 3);
            GameObject gpgs = GameObject.Find("GPGSManager");
            if (gpgs != null)
            {
                GPGSManager.instance.iResultScore = score;
                UserConnect.CoinAmount += (int)coin;
                GPGSManager.instance.fResultCoin = coin;
                UserConnect.KeyAmount += key;
                GPGSManager.instance.iResultkey = key;
                GPGSManager.instance.bGameEnd = true;
                gpgs.GetComponent<GPGSManager>().ReportScore(score);
                //StartCoroutine(_CreateRank("TestUser", score));
                //StartCoroutine(UserConnect.instance._SetUser(GPGSManager.mainId, (int)coin, key,
                //    UserConnect.CurrentWeaponIndex.ToString(), UserConnect.CurrentCharacterIndex.ToString()));

                SceneManager.LoadScene("Result");
            }
        }
    }

    // 플레이어 y 좌표 제한
    public IEnumerator PlayerYLimit()
    {
        while (true)
        {
            yield return new WaitUntil(() => transform.position.y >= 34);
            playerRg.AddForce(new Vector2(0, -50));
        }
    }

    // 크리티컬 체크
    public IEnumerator CriticalCheck()
    {
        while (true)
        {
            yield return new WaitUntil(() => GameObject.Find("ComboManager").GetComponent<ComboManager>().criticalStart);
            //Debug.Log("criticalStart: " + criticalStart);
            if(!criticalStart)
            {
                damage *= critical;
                criticalStart = true;
            }
            if (isDestroy)
            {
                criticalStart = false;
                damage = originDamage;
                //StartCoroutine(GameObject.Find("ComboManager").GetComponent<ComboManager>().CriticalTextDuration());
            }
        }
    }

    public void Jump()
    {
        jumpButClick = true;
        CharacterAnimation.instance.CatJumpIdleAniControll(0.5f);

        // 땅에 충돌되어있을 때만 점프 가능.
        if (ground_collsion)
        {
            jump_effect.SetActive(true);
            playerRg.AddForce(new Vector2(0, jumpSpeed));
            GlobalSFX.instance.PlayJumpSound();
            //CharacterAnimation.instance.CatJumpAniControll();
        }
    }

    public void JumpCancle()    // 점프 버튼 뗐을 때
    {
        jumpButClick = false;
        jump_effect.SetActive(false);
    }

    public IEnumerator JumpCheck()
    {
        while (true)
        {
            yield return new WaitUntil(() => jumpButClick == true);
            //Debug.Log("jumpButClick: " + jumpButClick);
            CharacterAnimation.instance.CatJumpIdleAniControll(0.5f);
            //StartCoroutine("AniCor");
        }
    }

    public void Attack()    // pc 테스트용 공격 함수
    {      
        attackOn = true;
        //transform.Find("swing").gameObject.SetActive(true);
        SwordEffect.SetActive(true);

        if (jumpOn)
        {
           // CharacterAnimation.instance.CatJumpIdleAniControll(20f);
            CharacterAnimation.instance.CatJumpAttackAniControll();
        }
        else
            CharacterAnimation.instance.CatAttackAniControll();
        GlobalSFX.instance.PlayWeaponSwingSound();
    }

    public void AttackCancle()
    {
        attackOn = false;
        SwordEffect.SetActive(false);
    }

    // 공격 애니 체크
    public IEnumerator AttackCheck()
    {
        while (true)
        {
            yield return new WaitUntil(() => attackOn == true);
            if (jumpOn)
            {
                CharacterAnimation.instance.CatJumpIdleAniControll(20f);
            }
            else
                CharacterAnimation.instance.CatAttackAniControll();
        }
    }

    // 블럭이 일정 좌표 이상 내려오면 플레이어 트리거 활성화.
    public IEnumerator PlayerTriggerCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (BlockGenerator.instance.block_ypos_min)
            {
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
        }
    }

    // 화면 터치 체크
    public IEnumerator TouchCheck()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.touchCount > 0);
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (EventSystem.current.IsPointerOverGameObject(i) == false)
                {
                    tempTouchs = Input.GetTouch(i);
                    if (tempTouchs.phase == TouchPhase.Began)
                    {    //해당 터치가 시작됐다면.
                        GlobalSFX.instance.PlayWeaponSwingSound();
                        SwordEffect.SetActive(true);
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
                        break;   //한 프레임(update)에는 하나만.
                    }
                    else if (tempTouchs.phase == TouchPhase.Ended)  // 터치가 끝났다면.
                    {
                        attackOn = false;
                        SwordEffect.SetActive(false);
                    }
                }
            }
        }
    }
    
    // 바닥과 충돌 체크
    public IEnumerator GroundCollision()
    {
        while (true)
        {
            yield return new WaitUntil(() => ground_collsion == true);
            jumpOn = false;
            CharacterAnimation.instance.CatJumpIdleAniControll(-0.1f);
        }
    }

    public IEnumerator GroundNotCollision()
    {
        while (true)
        {
            yield return new WaitWhile(() => ground_collsion);
            jumpOn = true;
        }
    }   

    // 레이 충돌 체크
    public IEnumerator RayCollisionCheck()
    {
        while (true)
        {
            //isDestroy = false;
            yield return new WaitUntil(() => attackOn == true);
            if (!(col_player.isTrigger))
            {
                //Debug.Log("attack ray");
                if (PlayerCollision.instance.rayCollider != null)
                {
                    int destroy_block_score = 0;
                    float blockCoin = 0;
                    int blockKey = 0;
                    GameObject newObj = PlayerCollision.instance.rayCollider.gameObject;

                    if (PlayerCollision.instance.rayCollider.CompareTag("bomb")) // 폭탄 공격 시 생명력 -2
                    {
                        BombEffect = ObjectPool.Instance.PopFromPool(BombEffectName);
                        BombEffect.transform.position = new Vector2(newObj.transform.position.x, (newObj.transform.position.y - 2));
                        BombEffect.SetActive(true);

                        life += 2;
                        Handheld.Vibrate();
                        GlobalSFX.instance.PlayBombSound();
                        parent = PlayerCollision.instance.rayCollider.transform.parent.gameObject;
                        Destroy(parent);
                        Destroy(newObj);
                    }

                    else
                    {
                        destroy_block_score = newObj.GetComponent<BlockStatusManager>().score;
                        blockCoin = newObj.GetComponent<BlockStatusManager>().coin;
                        blockKey = newObj.GetComponent<BlockStatusManager>().key;

                        newObj.GetComponent<BlockStatusManager>().hp -= damage;
                        GlobalSFX.instance.PlayCollisionSound();

                        AttackEffect = ObjectPool.Instance.PopFromPool(AttackEffectName);
                        AttackEffect.transform.position = new Vector2(newObj.transform.position.x, (newObj.transform.position.y - 3));
                        AttackEffect.SetActive(true);
                    }

                    if (newObj.GetComponent<BlockStatusManager>().hp <= 0)
                    {
                        if (PlayerCollision.instance.rayCollider.CompareTag("block5") ||
                            PlayerCollision.instance.rayCollider.CompareTag("FeverBlock10"))
                        {
                            if (newObj.GetComponent<BlockStatusManager>().hp <= 0)
                            {
                                parent = PlayerCollision.instance.rayCollider.transform.parent.gameObject;
                                Destroy(parent);
                            }
                        }
                        else if (PlayerCollision.instance.rayCollider.CompareTag("portion")) // 포션 파괴 시 생명력 +1
                        {
                            life -= 1;
                            parent = PlayerCollision.instance.rayCollider.transform.parent.gameObject;
                            Destroy(parent);
                        }

                        GlobalSFX.instance.PlayDestroySound();
                        score += destroy_block_score;
                        coin += blockCoin;
                        key += blockKey;
                        if (PlayerCollision.instance.rayCollider.CompareTag("block1") || PlayerCollision.instance.rayCollider.CompareTag("block2") ||
                            PlayerCollision.instance.rayCollider.CompareTag("block3") || PlayerCollision.instance.rayCollider.CompareTag("block4") ||
                            PlayerCollision.instance.rayCollider.CompareTag("block5"))
                        {
                            GameObject.Find("FeverManager").GetComponent<FeverTime>().block_destroy_count += 1;
                        }
                        Destroy(PlayerCollision.instance.rayCollider);
                        //Instantiate(DestroyParticle, new Vector2(newObj.transform.position.x, (newObj.transform.position.y - 2)), transform.rotation);
                        DestroyParticle = ObjectPool.Instance.PopFromPool(BlockParticleName);
                        DestroyParticle.transform.position = new Vector2(newObj.transform.position.x, (newObj.transform.position.y - 2));
                        DestroyParticle.SetActive(true);

                        CoinParticle = ObjectPool.Instance.PopFromPool(CoinParticleName);
                        CoinParticle.transform.position = new Vector2(newObj.transform.position.x, (newObj.transform.position.y - 2));
                        CoinParticle.SetActive(true);
                        //Instantiate(CoinParticle, new Vector2(newObj.transform.position.x, (newObj.transform.position.y - 2)), transform.rotation);
                        isDestroy = true;
                        blockCnt += 1;

                        if (!ground_collsion)
                            playerRg.AddForce(new Vector2(0, jumpSpeedCollision));

                    }
                    else
                        isDestroy = false;
                }
                attackOn = false;
            }
        
        }
    }

    public void Shield()    // 방어 버튼
    {
        // 쿨타임 시작
        shieldOn = true;

        // 트리거 꺼졌을 때
        if (buttonCool.GetComponent<ButtonCoolTime>().canUseShield && !shield_able)
        {
            //GlobalSFX.instance.PlayShieldSound();
            buttonCool.GetComponent<ButtonCoolTime>().UseShield();
        }
        // 트리거 켜졌을 때
        else if(buttonCool.GetComponent<ButtonCoolTime>().canUseShield && shield_able)
        {
            if (destroy_block.CompareTag("block1") || destroy_block.CompareTag("block2")
                || destroy_block.CompareTag("block3") || destroy_block.CompareTag("block4")
                || destroy_block.CompareTag("block5"))
            {
                if (blockRg == null)
                    blockRg = GameObject.Find("BlockGroup(Clone)").GetComponent<Rigidbody2D>();
                blockRg.AddForce(new Vector2(0, shieldForcetoBlock -1000f));
                blockRg.gravityScale = 0.5f;
                GlobalSFX.instance.PlayShieldSound();
            }
            else if(destroy_block.CompareTag("FeverBlock1")
               || destroy_block.CompareTag("FeverBlock2") || destroy_block.CompareTag("FeverBlock3")
               || destroy_block.CompareTag("FeverBlock4") || destroy_block.CompareTag("FeverBlock5")
               || destroy_block.CompareTag("FeverBlock6") || destroy_block.CompareTag("FeverBlock7")
               || destroy_block.CompareTag("FeverBlock8") || destroy_block.CompareTag("FeverBlock9")
               || destroy_block.CompareTag("FeverBlock10"))
            {
                if (blockRg == null)
                    blockRg = GameObject.Find("BlockGroupFever(Clone)").GetComponent<Rigidbody2D>();
                blockRg.AddForce(new Vector2(0, shieldForcetoBlock - 1000f));
                blockRg.gravityScale = 0.5f;
                GlobalSFX.instance.PlayShieldSound();
            }

            else if (destroy_block.CompareTag("rope") || destroy_block.CompareTag("handcuffs"))  // 부모 삭제
            {
                parent = destroy_block.transform.parent.gameObject;
                GlobalSFX.instance.PlayObjectSound();
                Destroy(parent);
                Destroy(destroy_block);
            }
            destroy_block.transform.Find("ShieldEffect").gameObject.GetComponent<Animator>().SetTrigger("ShieldOn");

            buttonCool.GetComponent<ButtonCoolTime>().UseShield();
        }
    }

    public void ShieldCancle()
    {
        shieldOn = false;
    }

    // 파괴할 블럭 처리
    public IEnumerator BlockDestroy()
    {
        while (true)
        {            
            yield return new WaitForSeconds(0.1f);
            if (attackOn)   // 플레이어의 트리거가 켜져있을 때 공격 버튼이 눌리면.
            {
                if (destroy_block == null)
                    continue;

                int destroy_block_score = 0;    // 파괴할 블럭의 점수                
                float blockCoin = 0;
                int blockKey = 0;

                if (destroy_block.CompareTag("bomb"))     // 폭탄 공격 시 생명력 -2
                {
                    BombEffect = ObjectPool.Instance.PopFromPool(BombEffectName);
                    BombEffect.transform.position = new Vector2(destroy_block.transform.position.x, (destroy_block.transform.position.y - 2));
                    BombEffect.SetActive(true);

                    life += 2;
                    Handheld.Vibrate();
                    parent = destroy_block.transform.parent.gameObject;
                    GlobalSFX.instance.PlayBombSound();
                    Destroy(parent);
                    Destroy(destroy_block);
                }
                else
                {
                    destroy_block_score = destroy_block.GetComponent<BlockStatusManager>().score;
                    blockCoin = destroy_block.GetComponent<BlockStatusManager>().coin;
                    blockKey = destroy_block.GetComponent<BlockStatusManager>().key;

                    destroy_block.GetComponent<BlockStatusManager>().hp -= damage;
                    GlobalSFX.instance.PlayCollisionSound();

                    AttackEffect = ObjectPool.Instance.PopFromPool(AttackEffectName);
                    AttackEffect.transform.position = new Vector2(destroy_block.transform.position.x, (destroy_block.transform.position.y - 3));
                    AttackEffect.SetActive(true);
                }
             
                if (destroy_block.GetComponent<BlockStatusManager>().hp <= 0)
                {                    
                    Destroy(destroy_block);
                    DestroyParticle = ObjectPool.Instance.PopFromPool(BlockParticleName);
                    DestroyParticle.transform.position = new Vector2(destroy_block.transform.position.x, (destroy_block.transform.position.y - 2));
                    DestroyParticle.SetActive(true);

                    CoinParticle = ObjectPool.Instance.PopFromPool(CoinParticleName);
                    CoinParticle.transform.position = new Vector2(destroy_block.transform.position.x, (destroy_block.transform.position.y - 2));
                    CoinParticle.SetActive(true);
                    isDestroy = true;
                    blockCnt += 1;

                    if (destroy_block.CompareTag("portion"))  // 포션 파괴 시 생명력 +1
                    {
                        life -= 1;
                    }

                    GlobalSFX.instance.PlayDestroySound();

                    if (destroy_block.CompareTag("block1") || destroy_block.CompareTag("block2")
                        || destroy_block.CompareTag("block3") || destroy_block.CompareTag("block4")
                        || destroy_block.CompareTag("block5"))
                    {
                        GameObject.Find("FeverManager").GetComponent<FeverTime>().block_destroy_count += 1;
                    }
                    score += destroy_block_score;
                    coin += blockCoin;
                    key += blockKey;

                    if (destroy_block.CompareTag("block5") || destroy_block.CompareTag("portion") ||
                        destroy_block.CompareTag("FeverBlock10"))   // 마지막 블럭일 때, 부모 삭제
                    {
                        parent = destroy_block.transform.parent.gameObject;
                        Destroy(parent);
                    }
                }
                else
                    isDestroy = false;
                

                attackOn = false;
            }
            else
            {
                isDestroy = false;
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("OnCollisionEnter2D");        

        // 땅과 충돌
        if (collision.collider.CompareTag ("Collision"))
        {
            ground_collsion = true;
            //CharacterAnimation.instance.CatJumpAniControll(false);
        }

        // 일반 블록
        if (collision.collider.CompareTag ("block1") || collision.collider.CompareTag ("block2")
            || collision.collider.CompareTag ("block3") || collision.collider.CompareTag ("block4")
            || collision.collider.CompareTag ("block5")) 
        {            
            GameObject newObj = collision.collider.gameObject;

            // 쉴드 버튼 누르면.
            if (shieldOn)
            {
                if (blockRg == null)
                    blockRg = GameObject.Find("BlockGroup(Clone)").GetComponent<Rigidbody2D>();

                newObj.transform.Find("ShieldEffect").gameObject.GetComponent<Animator>().SetTrigger("ShieldOn");
                GlobalSFX.instance.PlayShieldSound();
                blockRg.AddForce(new Vector2(0, shieldForcetoBlock));
                blockRg.velocity = (Vector2.up * 3f);
                playerRg.AddForce(new Vector2(0, shieldForcetoPlayer));
            }
        }

        // 피버 블록
        else if(collision.collider.CompareTag ("FeverBlock1")
            || collision.collider.CompareTag ("FeverBlock2") || collision.collider.CompareTag ("FeverBlock3")
            || collision.collider.CompareTag ("FeverBlock4") || collision.collider.CompareTag ("FeverBlock5")
            || collision.collider.CompareTag ("FeverBlock6") || collision.collider.CompareTag ("FeverBlock7")
            || collision.collider.CompareTag ("FeverBlock8") || collision.collider.CompareTag ("FeverBlock9")
            || collision.collider.CompareTag ("FeverBlock10"))
        {
            GameObject newObj = collision.collider.gameObject;

            // 쉴드 버튼 누르면.
            if (shieldOn)
            {
                if (blockRg == null)
                    blockRg = GameObject.Find("BlockGroupFever(Clone)").GetComponent<Rigidbody2D>();

                newObj.transform.Find("ShieldEffect").gameObject.GetComponent<Animator>().SetTrigger("ShieldOn");
                GlobalSFX.instance.PlayShieldSound();
                blockRg.AddForce(new Vector2(0, shieldForcetoBlock));
                blockRg.velocity = (Vector2.up * 3f);
                playerRg.AddForce(new Vector2(0, shieldForcetoPlayer));
            }
        }

        else if (collision.collider.CompareTag("rope") || collision.collider.CompareTag("handcuffs"))
        {
            GameObject newObj = collision.collider.gameObject;

            if (shieldOn)
            {
                if (blockRg == null)
                    blockRg = GameObject.Find("BlockGroup(Clone)").GetComponent<Rigidbody2D>();

                newObj.transform.Find("ShieldEffect").gameObject.GetComponent<Animator>().SetTrigger("ShieldOn");
                blockRg.AddForce(new Vector2(0, shieldForcetoBlock));
                blockRg.velocity = (Vector2.up * 3f);
                playerRg.AddForce(new Vector2(0, shieldForcetoPlayer));
                GlobalSFX.instance.PlayObjectSound();
                parent = newObj.transform.parent.gameObject;
                Destroy(newObj);
                Destroy(parent);
            }
        }        
    }

    public void OnCollisionStay2D(Collision2D collision)
    {     
        // 땅과 충돌
        if (collision.collider.CompareTag("Collision"))
        {
            ground_collsion = true;
        }

        // 블럭과 충돌
        if (collision.collider.CompareTag ("block1") || collision.collider.CompareTag ("block2")
            || collision.collider.CompareTag ("block3") || collision.collider.CompareTag ("block4")
            || collision.collider.CompareTag ("block5"))
        {
            GameObject newObj = collision.collider.gameObject;
            //shield_able = true;
            // 쉴드 버튼 누르면.
            if (shieldOn)
            {
                if (blockRg == null)
                {
                    blockRg = GameObject.Find("BlockGroup(Clone)").GetComponent<Rigidbody2D>();
                }

                newObj.transform.Find("ShieldEffect").gameObject.GetComponent<Animator>().SetTrigger("ShieldOn");

                GlobalSFX.instance.PlayShieldSound();
                blockRg.AddForce(new Vector2(0, shieldForcetoBlock));
                blockRg.velocity = (Vector2.up * 3f);
                playerRg.AddForce(new Vector2(0, shieldForcetoPlayer));
            }
        }

        // 피버 블록
        else if (collision.collider.CompareTag ("FeverBlock1")
            || collision.collider.CompareTag ("FeverBlock2") || collision.collider.CompareTag ("FeverBlock3")
            || collision.collider.CompareTag ("FeverBlock4") || collision.collider.CompareTag ("FeverBlock5")
            || collision.collider.CompareTag ("FeverBlock6") || collision.collider.CompareTag ("FeverBlock7")
            || collision.collider.CompareTag ("FeverBlock8") || collision.collider.CompareTag ("FeverBlock9")
            || collision.collider.CompareTag ("FeverBlock10"))
        {
            GameObject newObj = collision.collider.gameObject;

            // 쉴드 버튼 누르면.
            if (shieldOn)
            {
                if (blockRg == null)
                    blockRg = GameObject.Find("BlockGroupFever(Clone)").GetComponent<Rigidbody2D>();

                newObj.transform.Find("ShieldEffect").gameObject.GetComponent<Animator>().SetTrigger("ShieldOn");
                GlobalSFX.instance.PlayShieldSound();
                blockRg.AddForce(new Vector2(0, shieldForcetoBlock));
                blockRg.velocity = (Vector2.up * 3f);
                playerRg.AddForce(new Vector2(0, shieldForcetoPlayer));
            }
        }

        else if (collision.collider.CompareTag("rope") || collision.collider.CompareTag("handcuffs") || collision.collider.CompareTag("bomb"))
        {
            GameObject newObj = collision.collider.gameObject;

            if (shieldOn)
            {
                if (blockRg == null)
                    blockRg = GameObject.Find("BlockGroup(Clone)").GetComponent<Rigidbody2D>();

                newObj.transform.Find("ShieldEffect").gameObject.GetComponent<Animator>().SetTrigger("ShieldOn");
                blockRg.AddForce(new Vector2(0, shieldForcetoBlock));
                blockRg.velocity = (Vector2.up * 3f);
                playerRg.AddForce(new Vector2(0, shieldForcetoPlayer));
                GlobalSFX.instance.PlayObjectSound();
                parent = newObj.transform.parent.gameObject;
                Destroy(newObj);
                Destroy(parent);
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

        if (collision.collider.CompareTag("block1") || collision.collider.CompareTag ("block2") ||
            collision.collider.CompareTag ("block3") || collision.collider.CompareTag ("block4") ||
            collision.collider.CompareTag ("block5") || collision.collider.CompareTag ("FeverBlock1")
            || collision.collider.CompareTag ("FeverBlock2") || collision.collider.CompareTag ("FeverBlock3")
            || collision.collider.CompareTag ("FeverBlock4") || collision.collider.CompareTag ("FeverBlock5")
            || collision.collider.CompareTag ("FeverBlock6") || collision.collider.CompareTag ("FeverBlock7")
            || collision.collider.CompareTag ("FeverBlock8") || collision.collider.CompareTag ("FeverBlock9")
            || collision.collider.CompareTag ("FeverBlock10"))
        {
            col_player.isTrigger = false;
            // 플레이어 y좌표 freeze 되있던 것을 초기화
            playerRg.constraints = RigidbodyConstraints2D.None;
            playerRg.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerRg.constraints = RigidbodyConstraints2D.FreezePositionX;
        }

        if (collision.collider.CompareTag("Collision"))
            ground_collsion = false;
    }

    // 트리거 함수 자체는 플레이어의 트리거가 true일 때만 발동.
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("OnTriggerEnter2D");
        GameObject newObj = collision.gameObject;
        if (collision.CompareTag("block1") || collision.CompareTag("block2")
            || collision.CompareTag("block3") || collision.CompareTag("block4")
            || collision.CompareTag("block5") || collision.CompareTag("rope")
            || collision.CompareTag("handcuffs") || collision.CompareTag("bomb")
            || collision.CompareTag("portion") || collision.CompareTag("FeverBlock1")
            || collision.CompareTag("FeverBlock2") || collision.CompareTag ("FeverBlock3")
            || collision.CompareTag ("FeverBlock4") || collision.CompareTag ("FeverBlock5")
            || collision.CompareTag ("FeverBlock6") || collision.CompareTag ("FeverBlock7")
            || collision.CompareTag ("FeverBlock8") || collision.CompareTag ("FeverBlock9")
            || collision.CompareTag ("FeverBlock10"))
        {
            //Debug.Log("collision.CompareTag : " + collision.CompareTag);
            shield_able = true;
            if (destroy_block != null)
            {
                return;
            }
            destroy_block = newObj;
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

        if (collision.CompareTag("block5") || collision.CompareTag ("FeverBlock10"))
        {
            col_player.isTrigger = false;
            playerRg.constraints = RigidbodyConstraints2D.None;
            playerRg.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerRg.constraints = RigidbodyConstraints2D.FreezePositionX;
        }

        else if(collision.CompareTag ("bomb"))
        {
            destroy_block = null;
        }
    }
}
