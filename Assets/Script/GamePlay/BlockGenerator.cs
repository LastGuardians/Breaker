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
    public Rigidbody2D block_gravity = new Rigidbody2D();
    GameObject blockManager;

    public bool block_ypos_min = false;
    public bool game_start = false;

    float ypos = 30;
    
    System.Random r = new System.Random();
    //Random r = new Random();
    public int range = 0;      // 확률 범위
    public int grade_range = 0; // 블록 등급 확률 범위

    // 건물 리소스
    Sprite[] jail_normal = new Sprite[5];  // 교도소 내부(철창) 기본 건물 리소스
    Sprite[] jail_upgrade = new Sprite[5];  // 교도소 내부(철창) 강화 건물 리소스

    Sprite[] door_normal = new Sprite[5];   // 교도소 내부(철문) 기본
    Sprite[] door_upgrade = new Sprite[5];   // 교도소 내부(철문) 강화

    Sprite[] prison_normal = new Sprite[5];   // 교도소 외부 건물 기본
    Sprite[] prison_upgrade = new Sprite[5];   // 교도소 외부 건물 강화

    Sprite[] watch_normal = new Sprite[5];    // 감시탑 기본
    Sprite[] watch_upgrade = new Sprite[5];   // 감시탑 강화

    Sprite[] wall_normal = new Sprite[5];    // 담장 기본
    Sprite[] wall_upgrade = new Sprite[5];   // 담장 강화

    //BlockStatusManager[] block_stat = new BlockStatusManager[5];
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

        for (int i = 0; i < 5; ++i)
        {
            //grade_range = r.Next(0, 100);
            if (i == range)      // 랜덤한 한 블럭을 강화블럭으로 셋팅
            {
                //Debug.Log("range : " + range);
                blockArr[range].GetComponent<SpriteRenderer>().sprite = jail_upgrade[range];
                blockArr[range].GetComponent<BlockStatusManager>().BlockUpgrade();

            }
            else
            {
                //Debug.Log("i : " + i);
                blockArr[i].GetComponent<SpriteRenderer>().sprite = jail_normal[i];
                blockArr[i].GetComponent<BlockStatusManager>().BlockNormal();
            }
        }

        // 블럭 중력 
        block_gravity = GameObject.Find("BlockGroup").GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        BlockDestroy();
        //grade_range = r.Next(0, 100);
      
    }

    // 블럭이 모두 파괴되었는지 체크
    public bool BlockDestroy()
    {
        GameObject obj = GameObject.Find("building5");
        if (obj != null)  // hierarchy 상에 오브젝트가 존재하면 블록 생성 x
            return false; 
        else
        {
            block_ypos_min = false;
            //Debug.Log("true 리턴");
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

                if (blockArr[i].transform.position.y < 1.4 &&
                    blockArr[i].transform.position.y >= -0.4)
                {
                    block_ypos_min = true;
                }
            }
        }
    }

    // 블럭 단계 체크(리소스 바꾸기 위해)
    IEnumerator BlockStageCheck()
    {
        while (true)
        {
            BlockDestroy();
            yield return new WaitUntil(BlockDestroy);
            
            /// 단계별 공통 작업 ///
            blockParents = Instantiate(Resources.Load("Prefabs/BlockGroup"),
                  new Vector2(transform.position.x, (transform.position.y + 15)), transform.rotation) as GameObject;

            if (null == block_gravity)
                block_gravity = blockParents.GetComponent<Rigidbody2D>();

            range = r.Next(0, 5);

            for (int i = 0; i < 5; ++i)
            {
                blockArr[i] = GameObject.Find("BlockGroup(Clone)").transform.Find("building" + (i + 1).ToString()).gameObject;
                blockArr[i].AddComponent<BlockStatusManager>();
            }
            //////////////////////////

            if (blockManager.GetComponent<BlockStatusManager>().stage == 1)  // 1단계
            {             
                for (int i = 0; i < 5; ++i)
                {
                  
                    if (i == range)      // 랜덤한 한 블럭을 강화블럭으로 셋팅
                    {
                        blockArr[range].GetComponent<SpriteRenderer>().sprite = jail_upgrade[range];
                        blockArr[i].GetComponent<BlockStatusManager>().SetBlockUpgrade(1);

                    }
                    else
                    {
                        blockArr[i].GetComponent<SpriteRenderer>().sprite = jail_normal[i];
                        blockArr[i].GetComponent<BlockStatusManager>().SetBlockNormal(1);
                    }
                }
            }

            else if(blockManager.GetComponent<BlockStatusManager>().stage == 2)     // 2단계
            {              
                grade_range = r.Next(0, 100);

                for (int i = 0; i < 5; ++i)
                {                    
                   
                    if (grade_range < 30)   // 1단계 건물 확률 30%
                    {
                        if (i == range)      // 랜덤한 한 블럭을 강화블럭으로 셋팅
                        {
                            //Debug.Log("range : " + range);
                            blockArr[range].GetComponent<SpriteRenderer>().sprite = jail_upgrade[range];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockUpgrade(1);
                        }
                        else
                        {
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = jail_normal[i];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockNormal(1);
                        }
                    }
                    else    // 2단계 건물 확률 70%
                    {
                        if (i == range)      // 랜덤한 한 블럭을 강화블럭으로 셋팅
                        {
                            //Debug.Log("range : " + range);
                            blockArr[range].GetComponent<SpriteRenderer>().sprite = door_upgrade[range];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockUpgrade(2);
                        }
                        else
                        {
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = door_normal[i];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockNormal(2);
                        }
                    }
                }
            }

            else if (blockManager.GetComponent<BlockStatusManager>().stage == 3)     // 3단계
            {               
                grade_range = r.Next(0, 100);

                for (int i = 0; i < 5; ++i)
                {
                  
                    if (grade_range < 10)   // 1단계 건물 확률 10%
                    {
                        if (i == range)
                        {
                            //Debug.Log("range : " + range);
                            blockArr[range].GetComponent<SpriteRenderer>().sprite = jail_upgrade[range];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockUpgrade(1);
                        }
                        else
                        {
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = jail_normal[i];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockNormal(1);
                        }
                    }
                    else if(grade_range < 40)    // 2단계 건물 확률 70%
                    {
                        if (i == range)
                        {
                            //Debug.Log("range : " + range);
                            blockArr[range].GetComponent<SpriteRenderer>().sprite = door_upgrade[range];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockUpgrade(2);
                        }
                        else
                        {
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = door_normal[i];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockNormal(2);
                        }
                    }
                    else if (grade_range < 100)    // 3단계 건물 확률 30%
                    {
                        if (i == range)
                        {
                            //Debug.Log("range : " + range);
                            blockArr[range].GetComponent<SpriteRenderer>().sprite = prison_upgrade[range];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockUpgrade(3);
                        }
                        else
                        {
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = prison_normal[i];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockNormal(3);
                        }
                    }
                }
            }

            else if (blockManager.GetComponent<BlockStatusManager>().stage == 4)     // 4단계
            {               
                grade_range = r.Next(0, 100);

                for (int i = 0; i < 5; ++i)
                {
                 
                    if (grade_range < 5)   // 1단계 건물 확률 5%
                    {
                        if (i == range)
                        {
                            //Debug.Log("range : " + range);
                            blockArr[range].GetComponent<SpriteRenderer>().sprite = jail_upgrade[range];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockUpgrade(1);
                        }
                        else
                        {
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = jail_normal[i];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockNormal(1);
                        }
                    }
                    else if (grade_range < 10)    // 2단계 건물 확률 5%
                    {
                        if (i == range)
                        {
                            //Debug.Log("range : " + range);
                            blockArr[range].GetComponent<SpriteRenderer>().sprite = door_upgrade[range];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockUpgrade(2);
                        }
                        else
                        {
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = door_normal[i];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockNormal(2);
                        }
                    }
                    else if (grade_range < 40)    // 3단계 건물 확률 30%
                    {
                        if (i == range)
                        {
                            //Debug.Log("range : " + range);
                            blockArr[range].GetComponent<SpriteRenderer>().sprite = prison_upgrade[range];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockUpgrade(3);
                        }
                        else
                        {
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = prison_normal[i];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockNormal(3);
                        }
                    }
                    else if (grade_range < 100)    // 4단계 건물 확률 30%
                    {
                        if (i == range)
                        {
                            //Debug.Log("range : " + range);
                            blockArr[range].GetComponent<SpriteRenderer>().sprite = watch_upgrade[range];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockUpgrade(4);
                        }
                        else
                        {
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = watch_normal[i];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockNormal(4);
                        }
                    }
                }
            }

            else if (blockManager.GetComponent<BlockStatusManager>().stage == 5)     // 5단계
            {               
                grade_range = r.Next(0, 100);

                for (int i = 0; i < 5; ++i)
                {
                  
                    if (grade_range < 2)   // 1단계 건물 확률 2%
                    {
                        if (i == range)
                        {
                            //Debug.Log("range : " + range);
                            blockArr[range].GetComponent<SpriteRenderer>().sprite = jail_upgrade[range];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockUpgrade(1);
                        }
                        else
                        {
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = jail_normal[i];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockNormal(1);
                        }
                    }
                    else if (grade_range < 5)    // 2단계 건물 확률 3%
                    {
                        if (i == range)
                        {
                            //Debug.Log("range : " + range);
                            blockArr[range].GetComponent<SpriteRenderer>().sprite = door_upgrade[range];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockUpgrade(2);
                        }
                        else
                        {
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = door_normal[i];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockNormal(2);
                        }
                    }
                    else if (grade_range < 10)    // 3단계 건물 확률 5%
                    {
                        if (i == range)
                        {
                            //Debug.Log("range : " + range);
                            blockArr[range].GetComponent<SpriteRenderer>().sprite = prison_upgrade[range];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockUpgrade(3);
                        }
                        else
                        {
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = prison_normal[i];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockNormal(3);
                        }
                    }
                    else if (grade_range < 40)    // 4단계 건물 확률 30%
                    {
                        if (i == range)
                        {
                            //Debug.Log("range : " + range);
                            blockArr[range].GetComponent<SpriteRenderer>().sprite = watch_upgrade[range];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockUpgrade(4);
                        }
                        else
                        {
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = watch_normal[i];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockNormal(4);
                        }
                    }
                    else if (grade_range < 100)    // 5단계 건물 확률 30%
                    {
                        if (i == range)
                        {
                            //Debug.Log("range : " + range);
                            blockArr[range].GetComponent<SpriteRenderer>().sprite = wall_upgrade[range];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockUpgrade(5);
                        }
                        else
                        {
                            blockArr[i].GetComponent<SpriteRenderer>().sprite = wall_normal[i];
                            blockArr[i].GetComponent<BlockStatusManager>().SetBlockNormal(5);
                        }
                    }
                }
            }
        }
    }
}