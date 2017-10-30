using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    //---객체 배열생성 
    //ArrayList[] objArray = new ArrayList[100];
    ////-----------객체 생성 
    //BlockObject obj = Instantiate(CloneObjectPrefab) as BlockObject;
    ////----------객체 배열에 등록 
    //objArray.Add(obj);

    ////------등록된 객체를 전부 삭제 
    //foreach(BlockObject blockobj in objArray) 
    //{ 
    //Destroy(blockobj);
    //}
    
    public GameObject blockParents;
    //public GameObject blockGroup;
    public GameObject[] blockArr = new GameObject[5];
    public GameObject[] blockArrFever = new GameObject[10];
    public Rigidbody2D block_gravity = new Rigidbody2D();
    GameObject blockManager;

    public bool block_ypos_min = false;
    public bool game_start = false;
    public bool isDestroy = false;
    public bool feverStart = false;

    float ypos = 30;
    static double[] blockHp = new double[5];
    
    System.Random r = new System.Random();
    //Random r = new Random();
    public int range = 0;      //  블록 리소스 확률 범위
    public int grade_range = 0; // 블록 등급 확률 범위
    public int upgrade_range = 0;   // 강화 블록 확률
    int type_range = 0;     // 블록 타입 확률(블록 or 오브젝트)

    // 건물 리소스
    Sprite[] jail_normal = new Sprite[5];  // 교도소 내부(철창) 기본
    Sprite[] jail_upgrade = new Sprite[5];  // 교도소 내부(철창) 강화

    Sprite[] door_normal = new Sprite[5];   // 교도소 내부(철문) 기본
    Sprite[] door_upgrade = new Sprite[5];   // 교도소 내부(철문) 강화

    Sprite[] prison_normal = new Sprite[5];   // 교도소 외부 건물 기본
    Sprite[] prison_upgrade = new Sprite[5];   // 교도소 외부 건물 강화

    Sprite[] watch_normal = new Sprite[5];    // 감시탑 기본
    Sprite[] watch_upgrade = new Sprite[5];   // 감시탑 강화

    Sprite[] wall_normal = new Sprite[5];    // 담장 기본
    Sprite[] wall_upgrade = new Sprite[5];   // 담장 강화

    Sprite rope = new Sprite();  // 밧줄
    Sprite handcuffs = new Sprite();  // 수갑
    Sprite bomb = new Sprite();  // 폭탄
    Sprite portion = new Sprite();  // 포션


    public static BlockGenerator instance = null;

    void Awake()
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

        for (int i = 0; i < 5; ++i)
        {    
            blockArr[i] = GameObject.Find("BlockGroup").transform.Find("building" + (i + 1).ToString()).gameObject;
            blockArr[i].AddComponent<BlockStatusManager>();
        }

        game_start = true;
    }


    void Start()
    {
        StartCoroutine(BlockTranslate());
        StartCoroutine(BlockStageCheck());
       // StartCoroutine(FeverBlock());

        blockManager = GameObject.Find("BlockManager");
        range = r.Next(0, 5);   // 강화블럭 확률 범위

        for (int i = 0; i < 5; ++i)
        {
            // 리소스 셋팅
            jail_normal[i] = Resources.Load<Sprite>("Building/1.Jail/jail" + (i + 1).ToString());
            jail_upgrade[i] = Resources.Load<Sprite>("Building/1.Jail/upgrade/jail_upgrade" + (i + 1).ToString());

            door_normal[i] = Resources.Load<Sprite>("Building/2.Door/door" + (i + 1).ToString());
            door_upgrade[i] = Resources.Load<Sprite>("Building/2.Door/upgrade/door_upgrade" + (i + 1).ToString());

            prison_normal[i] = Resources.Load<Sprite>("Building/3.Prison_Out_Building/prison" + (i + 1).ToString());
            prison_upgrade[i] = Resources.Load<Sprite>("Building/3.Prison_Out_Building/upgrade/prison_upgrade" + (i + 1).ToString());

            watch_normal[i] = Resources.Load<Sprite>("Building/4.WatchTower/watchTower" + (i + 1).ToString());
            watch_upgrade[i] = Resources.Load<Sprite>("Building/4.WatchTower/upgrade/watchTower_upgrade" + (i + 1).ToString());

            wall_normal[i] = Resources.Load<Sprite>("Building/5.Wall/wall" + (i + 1).ToString());
            wall_upgrade[i] = Resources.Load<Sprite>("Building/5.Wall/upgrade/wall_upgrade" + (i + 1).ToString());
                    
        }

        rope = Resources.Load<Sprite>("Building/Object/rope");
        handcuffs = Resources.Load<Sprite>("Building/Object/handcuffs");
        bomb = Resources.Load<Sprite>("Building/Object/bomb");
        portion = Resources.Load<Sprite>("Building/Object/potion");

        for (int i = 0; i < 5; ++i)
        {
            if (i == range)      // 랜덤한 한 블럭을 강화블럭으로 셋팅
            {
                blockArr[range].GetComponent<SpriteRenderer>().sprite = jail_upgrade[range];
                blockArr[range].GetComponent<BlockStatusManager>().SetBlockUpgrade(1);

            }
            else
            {
                blockArr[i].GetComponent<SpriteRenderer>().sprite = jail_normal[i];
                blockArr[i].GetComponent<BlockStatusManager>().SetBlockNormal(1);
            }
        }
        StartCoroutine(BlockHPCheck());
        // 블럭 중력 
        block_gravity = GameObject.Find("BlockGroup").GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        BlockDestroy();
        //grade_range = r.Next(0, 100);


        //for (int i = 0; i < 5; ++i)
        //{
            //if (blockArr[0].GetComponent<BlockStatusManager>().hp < 2)
            //{
            //    EffectAnimation.instance.Crack1();
            //}
        //}
    }

    // 블럭이 모두 파괴되었는지 체크
    public bool BlockDestroy()
    {
        GameObject obj1 = GameObject.Find("BlockGroup");
        GameObject obj2 = GameObject.Find("BlockGroup(Clone)");
        GameObject obj3 = GameObject.Find("BlockGroupFever(Clone)");
        if (obj1 != null || obj2 != null || obj3 != null)  // hierarchy 상에 오브젝트가 존재하면 블록 생성 x
            return false;  
        else
        {
            block_ypos_min = false;
            //StartCoroutine(BlockStageCheck());
            return true;
        }
    }
   

    // 블럭 위치 체크
    IEnumerator BlockTranslate()
    {
        while (true)
        {
            yield return new WaitWhile(BlockDestroy);   // false면 아래 코드 실행
            for (int i = 0; i < 5; ++i)
            {
                if (blockArr[i] == null)
                    continue;

                if ((blockArr[i].transform.position.y < 1.4 &&
                     blockArr[i].transform.position.y >= -0.4))
                {
                    block_ypos_min = true;
                }             
            }

            for (int i = 0; i < 10; ++i)
            {
                if (blockArrFever[i] == null)
                    continue;

                if (blockArrFever[i].transform.position.y < 1.4 &&
                    blockArrFever[i].transform.position.y >= -0.4)
                {
                    //Debug.Log("block_ypos_min : " + block_ypos_min);
                    block_ypos_min = true;
                }
            }
        }
    }

    IEnumerator FeverBlock()    // 피버 발동 시 블럭 생성
    {
        blockParents = Instantiate(Resources.Load("Prefabs/BlockGroupFever"),
                new Vector2(transform.position.x, (transform.position.y + 15)), transform.rotation) as GameObject;

        if (null == block_gravity)
            block_gravity = blockParents.GetComponent<Rigidbody2D>();

        block_gravity.gravityScale *= 1.8f;

        range = r.Next(0, 5);
        upgrade_range = r.Next(0, 5);
        grade_range = r.Next(0, 101);

        for (int i = 0; i < 10; ++i)
        {
            blockArrFever[i] = GameObject.Find("BlockGroupFever(Clone)").transform.Find("building" + (i + 1).ToString()).gameObject;
            blockArrFever[i].AddComponent<BlockStatusManager>();

            if(i<5)
                blockArrFever[i].GetComponent<SpriteRenderer>().sprite = door_normal[i];
            else
                blockArrFever[i].GetComponent<SpriteRenderer>().sprite = door_normal[i-5];
        }

        SetFeverBlock(1);
        StartCoroutine(BlockStageCheck());
        yield break;
    }

    // 블럭 단계 체크(리소스 바꾸기 위해)
    IEnumerator BlockStageCheck()
    {
        while (true)
        {
            BlockDestroy();
            yield return new WaitUntil(BlockDestroy);   // true면 아래 코드 실행.
            //Debug.Log("BlockDestroy : " + BlockDestroy());
            /// 단계별 공통 작업 ///
            if (!feverStart)
            {
                blockParents = Instantiate(Resources.Load("Prefabs/BlockGroup"),
                      new Vector2(transform.position.x, (transform.position.y + 15)), transform.rotation) as GameObject;

                if (null == block_gravity)
                    block_gravity = blockParents.GetComponent<Rigidbody2D>();
            }

            else      // 피버 발동 시
            {
                StartCoroutine(FeverBlock());
                yield break;
            }

            range = r.Next(0, 5);
            upgrade_range = r.Next(0, 5);
            grade_range = r.Next(0, 101);
            //type_range = r.Next(0, 101);

            int block_set_range = 0;
            if (blockManager.GetComponent<BlockStatusManager>().stage < 3)
                block_set_range = 101;
            else
                block_set_range = 70;


            if (grade_range < block_set_range)
            {
                for (int i = 0; i < 5; ++i)
                {
                    blockArr[i] = GameObject.Find("BlockGroup(Clone)").transform.Find("building" + (i + 1).ToString()).gameObject;
                    blockArr[i].AddComponent<BlockStatusManager>();
                    // 리소스 랜덤
                    if (range == 0)
                    {
                        blockArr[i].GetComponent<SpriteRenderer>().sprite = jail_normal[i];
                        if (i == upgrade_range)
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = jail_upgrade[i];
                    }
                    else if (range == 1)
                    {
                        blockArr[i].GetComponent<SpriteRenderer>().sprite = door_normal[i];
                        if (i == upgrade_range)
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = door_upgrade[i];
                    }
                    else if (range == 2)
                    {
                        blockArr[i].GetComponent<SpriteRenderer>().sprite = prison_normal[i];
                        if (i == upgrade_range)
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = prison_upgrade[i];
                    }
                    else if (range == 3)
                    {
                        blockArr[i].GetComponent<SpriteRenderer>().sprite = watch_normal[i];
                        if (i == upgrade_range)
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = watch_upgrade[i];
                    }
                    else if (range == 4)
                    {
                        blockArr[i].GetComponent<SpriteRenderer>().sprite = wall_normal[i];
                        if (i == upgrade_range)
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = wall_upgrade[i];
                    }
                }
            }

            // if (blockManager.GetComponent<BlockStatusManager>().stage > 2)
            else if (grade_range >= 70 && !FeverTime.instance.fever_start)
            {
                for (int i = 0; i < 5; ++i)
                {
                    blockArr[i] = GameObject.Find("BlockGroup(Clone)").transform.Find("building" + (i + 1).ToString()).gameObject;
                    blockArr[i].AddComponent<BlockStatusManager>();
                }               

                if (grade_range >= 70 && grade_range < 80)    // 밧줄 확률 10%
                {
                    blockArr[0].tag = "rope";
                    for (int i = 0; i < 5; ++i)
                    {
                        if (i == 0)
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = rope;
                        else
                        {
                            Destroy(blockArr[i]);
                            //blockArr[i] = null;
                            //blockArr[i].GetComponent<SpriteRenderer>().sprite = ;
                        }
                    }

                    SetObject(1);
                }
                else if (grade_range >= 80 && grade_range < 90)    // 수갑 확률 10%
                {
                    blockArr[0].tag = "handcuffs";
                    for (int i = 0; i < 5; ++i)
                    {
                        if (i == 0)
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = handcuffs;
                        else
                        {
                            Destroy(blockArr[i]);
                            //blockArr[i] = null;
                            //blockArr[i].GetComponent<SpriteRenderer>().sprite = null;
                        }

                    }

                    SetObject(2);
                }
                else if (grade_range >= 90 && grade_range < 99)    // 폭탄 확률 9%
                {
                    blockArr[0].tag = "bomb";
                    for (int i = 0; i < 5; ++i)
                    {
                        if (i == 0)
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = bomb;
                        else
                        {
                            Destroy(blockArr[i]);
                            //blockArr[i] = null;
                            //blockArr[i].GetComponent<SpriteRenderer>().sprite = null;
                        }
                    }

                    SetObject(3);
                }
                else if (grade_range >= 99 && grade_range < 100)    // 포션 확률 1%
                {
                    blockArr[0].tag = "portion";
                    for (int i = 0; i < 5; ++i)
                    {
                        if (i == 0)
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = portion;
                        else
                        {
                            Destroy(blockArr[i]);
                            //blockArr[i] = null;
                            //blockArr[i].GetComponent<SpriteRenderer>().sprite = null;
                        }
                    }
                    SetObject(4);
                }

                if (FeverTime.instance.fever_start)      // 피버 발동 시
                {
                    SetBlock(1);
                    //StartCoroutine(BlockHPCheck());
                }
            }

            //////////////////////////

            if (blockManager.GetComponent<BlockStatusManager>().stage == 1 && !FeverTime.instance.fever_start)  // 1단계
            {
                SetBlock(1);
                StartCoroutine(BlockHPCheck());
            }

            else if (blockManager.GetComponent<BlockStatusManager>().stage == 2 && !FeverTime.instance.fever_start)     // 2단계
            {
                for (int i = 0; i < 5; ++i)
                {
                    if (grade_range < 30)   // 1단계 건물 확률 30%
                    {
                        SetBlock(1);
                    }
                    else    // 2단계 건물 확률 70%
                    {
                        SetBlock(2);
                    }
                }
                StartCoroutine(BlockHPCheck());
            }

            else if (blockManager.GetComponent<BlockStatusManager>().stage == 3 && !FeverTime.instance.fever_start)     // 3단계
            {
                for (int i = 0; i < 5; ++i)
                {
                    if (grade_range < 5)   // 1단계 건물 확률 5%
                        SetBlock(1);
                    else if (grade_range < 20)    // 2단계 건물 확률 15%
                        SetBlock(2);
                    else if (grade_range < 70)    // 3단계 건물 확률 50%
                        SetBlock(3);
                }
                StartCoroutine(BlockHPCheck());
            }

            else if (blockManager.GetComponent<BlockStatusManager>().stage == 4 && !FeverTime.instance.fever_start)     // 4단계
            {
                for (int i = 0; i < 5; ++i)
                {
                    if (grade_range < 1)   // 1단계 건물 확률 1%
                        SetBlock(1);
                    else if (grade_range < 10)    // 2단계 건물 확률 9%
                        SetBlock(2);
                    else if (grade_range < 30)    // 3단계 건물 확률 20%
                        SetBlock(3);
                    else if (grade_range < 70)    // 4단계 건물 확률 40%
                        SetBlock(4);
                }
                StartCoroutine(BlockHPCheck());
            }

            else if (blockManager.GetComponent<BlockStatusManager>().stage == 5 && !FeverTime.instance.fever_start)     // 5단계
            {
                for (int i = 0; i < 5; ++i)
                {
                    if (grade_range < 1)   // 1단계 건물 확률 1%
                        SetBlock(1);
                    else if (grade_range < 2)    // 2단계 건물 확률 1%
                        SetBlock(2);
                    else if (grade_range < 10)    // 3단계 건물 확률 8%
                        SetBlock(3);
                    else if (grade_range < 30)    // 4단계 건물 확률 20%
                        SetBlock(4);
                    else if (grade_range < 70)    // 5단계 건물 확률 40%
                        SetBlock(5);
                }
                StartCoroutine(BlockHPCheck());
            }

            else if (blockManager.GetComponent<BlockStatusManager>().stage == 6 && !FeverTime.instance.fever_start)     // 6단계
            {
                for (int i = 0; i < 5; ++i)
                {
                    if (grade_range < 1)   // 1단계 건물 확률 1%
                        SetBlock(1);
                    else if (grade_range < 2)    // 2단계 건물 확률 1%
                        SetBlock(2);
                    else if (grade_range < 3)    // 3단계 건물 확률 1%
                        SetBlock(3);
                    else if (grade_range < 10)    // 4단계 건물 확률 7%
                        SetBlock(4);
                    else if (grade_range < 30)    // 5단계 건물 확률 20%
                        SetBlock(5);
                    else if (grade_range < 70)    // 6단계 건물 확률 40%
                        SetBlock(6);
                }
                StartCoroutine(BlockHPCheck());
            }

            else if (blockManager.GetComponent<BlockStatusManager>().stage == 7 && !FeverTime.instance.fever_start)     // 7단계
            {
                for (int i = 0; i < 5; ++i)
                {
                    if (grade_range < 1)   // 1단계 건물 확률 1%
                        SetBlock(1);
                    else if (grade_range < 2)    // 2단계 건물 확률 1%
                        SetBlock(2);
                    else if (grade_range < 3)    // 3단계 건물 확률 1%
                        SetBlock(3);
                    else if (grade_range < 4)    // 4단계 건물 확률 1%
                        SetBlock(4);
                    else if (grade_range < 10)    // 5단계 건물 확률 6%
                        SetBlock(5);
                    else if (grade_range < 30)    // 6단계 건물 확률 20%
                        SetBlock(6);
                    else if (grade_range < 70)    // 7단계 건물 확률 40%
                        SetBlock(7);
                }
                StartCoroutine(BlockHPCheck());
            }

            else if (blockManager.GetComponent<BlockStatusManager>().stage == 8 && !FeverTime.instance.fever_start)     // 8단계
            {
                for (int i = 0; i < 5; ++i)
                {
                    if (grade_range < 1)   // 1단계 건물 확률 1%
                        SetBlock(1);
                    else if (grade_range < 2)    // 2단계 건물 확률 1%
                        SetBlock(2);
                    else if (grade_range < 3)    // 3단계 건물 확률 1%
                        SetBlock(3);
                    else if (grade_range < 4)    // 4단계 건물 확률 1%
                        SetBlock(4);
                    else if (grade_range < 5)    // 5단계 건물 확률 1%
                        SetBlock(5);
                    else if (grade_range < 10)    // 6단계 건물 확률 5%
                        SetBlock(6);
                    else if (grade_range < 30)    // 7단계 건물 확률 20%
                        SetBlock(7);
                    else if (grade_range < 70)    // 8단계 건물 확률 40%
                        SetBlock(8);
                }
                StartCoroutine(BlockHPCheck());
            }

            else if (blockManager.GetComponent<BlockStatusManager>().stage == 9 && !FeverTime.instance.fever_start)     // 9단계
            {
                for (int i = 0; i < 5; ++i)
                {
                    if (grade_range < 1)   // 1단계 건물 확률 1%
                        SetBlock(1);
                    else if (grade_range < 2)    // 2단계 건물 확률 1%
                        SetBlock(2);
                    else if (grade_range < 3)    // 3단계 건물 확률 1%
                        SetBlock(3);
                    else if (grade_range < 4)    // 4단계 건물 확률 1%
                        SetBlock(4);
                    else if (grade_range < 5)    // 5단계 건물 확률 1%
                        SetBlock(5);
                    else if (grade_range < 6)    // 6단계 건물 확률 1%
                        SetBlock(6);
                    else if (grade_range < 10)    // 7단계 건물 확률 4%
                        SetBlock(7);
                    else if (grade_range < 30)    // 8단계 건물 확률 20%
                        SetBlock(8);
                    else if (grade_range < 70)    // 9단계 건물 확률 40%
                        SetBlock(9);
                }
                StartCoroutine(BlockHPCheck());
            }

            else if (blockManager.GetComponent<BlockStatusManager>().stage == 10 && !FeverTime.instance.fever_start)     // 10단계
            {
                for (int i = 0; i < 5; ++i)
                {
                    if (grade_range < 1)   // 1단계 건물 확률 1%
                        SetBlock(1);
                    else if (grade_range < 2)    // 2단계 건물 확률 1%
                        SetBlock(2);
                    else if (grade_range < 3)    // 3단계 건물 확률 1%
                        SetBlock(3);
                    else if (grade_range < 4)    // 4단계 건물 확률 1%
                        SetBlock(4);
                    else if (grade_range < 5)    // 5단계 건물 확률 1%
                        SetBlock(5);
                    else if (grade_range < 6)    // 6단계 건물 확률 1%
                        SetBlock(6);
                    else if (grade_range < 7)    // 7단계 건물 확률 1%
                        SetBlock(7);
                    else if (grade_range < 10)    // 8단계 건물 확률 3%
                        SetBlock(8);
                    else if (grade_range < 30)    // 9단계 건물 확률 20%
                        SetBlock(9);
                    else if (grade_range < 70)    // 10단계 건물 확률 40%
                        SetBlock(10);
                    //else if (grade_range < 80)    // 밧줄 확률 10%
                    //    SetBlock(10);
                    //else if (grade_range < 90)    // 수갑 확률 10%
                    //    SetBlock(10);
                    //else if (grade_range < 99)    // 폭탄 확률 9%
                    //    SetBlock(10);
                    //else if (grade_range < 101)    // 포션 확률 1%
                    //    SetBlock(10);
                }
                StartCoroutine(BlockHPCheck());
            }
        }
    }

    public void SetBlock(int num)
    {
        for (int i = 0; i < 5; ++i)
        {
            if (i == upgrade_range)
            {
                blockArr[i].GetComponent<BlockStatusManager>().SetBlockUpgrade(num);
            }
            else
            {
                blockArr[i].GetComponent<BlockStatusManager>().SetBlockNormal(num);
               // Debug.Log("hp : " + blockArr[i].GetComponent<BlockStatusManager>().hp);
            }
        }
    }

    public void SetFeverBlock(int num)
    {
        for (int i = 0; i < 10; ++i)
        {
             blockArrFever[i].GetComponent<BlockStatusManager>().SetBlockNormal(num);
        }

    }

    public void SetObject(int num)
    {
        for (int i = 0; i < 5; ++i)
        {
            if (i == 0) 
                blockArr[i].GetComponent<BlockStatusManager>().SetObject(num);
            else
                blockArr[i] = null;

        }

    }

    IEnumerator BlockHPCheck()
    {
        for (int i = 0; i < 5; ++i)
        {
            if (blockArr[i] == null)
                continue;
            else
                blockHp[i] = blockArr[i].GetComponent<BlockStatusManager>().hp;
        }

        while (true)
        {         
            yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < 5; ++i)
            {
                if (blockArr[i] == null)
                {
                    isDestroy = true;
                    continue;
                }
                else
                {
                    isDestroy = false;
                    if (blockArr[i].GetComponent<BlockStatusManager>().hp <= blockHp[i] / 1.5)
                    {
                        //Debug.Log("hp / 1.5 이하");
                        blockArr[i].GetComponent<EffectAnimation>().Crack1();
                        if (blockArr[i].GetComponent<BlockStatusManager>().hp <= blockHp[i] / 4)
                        {
                            //Debug.Log("hp / 4 이하");
                            blockArr[i].GetComponent<EffectAnimation>().Crack2();
                        }
                    }                    
                }

            }
        }
    }
}