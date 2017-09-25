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

    public bool block_ypos_min = false;
    public bool game_start = false;

    float ypos = 30;
    
    System.Random r = new System.Random();
    //Random r = new Random();
    public int range = 0;      // 확률 범위
    public int grade_range = 0; // 블록 등급 확률 범위

    public Sprite[] jail_normal = new Sprite[5];  // 교도소 내부 기본 건물 리소스
    public Sprite[] jail_upgrade = new Sprite[5];  // 교도소 내부 강화 건물 리소스

    BlockStatusManager[] block_stat = new BlockStatusManager[5];
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
            //jail_normal[i] = Resources.Load("Background/Building/prison_inside/jail" + (i + 1).ToString()) as GameObject;
            //jail_upgrade[i] = Resources.Load("Background/Building/prison_inside/upgrade/jail_upgrade" + (i + 1).ToString());

            //blockArr[i] = jail_upgrade[i] as GameObject;
            //blockArr[i] = Resources.Load("Prefabs/BlockGroup") as GameObject;
            //blockParents = Instantiate(Resources.Load("Prefabs/BlockGroup"), new Vector2(transform.position.x, (transform.position.y)), transform.rotation) as GameObject;

            blockArr[i] = GameObject.Find("BlockGroup").transform.Find("building" + (i + 1).ToString()).gameObject;
            blockArr[i].AddComponent<BlockStatusManager>();
        }

        game_start = true;
    }


    void Start()
    {
        StartCoroutine(BlockGenerate());
        StartCoroutine(BlockTranslate());
        range = r.Next(0, 5);

        for (int i = 0; i < 5; ++i)
        {
            // 리소스 셋팅
            jail_normal[i] = Resources.Load<Sprite>("Background/Building/prison_inside/jail" + (i + 1).ToString());
            jail_upgrade[i] = Resources.Load<Sprite>("Background/Building/prison_inside/upgrade/jail_upgrade" + (i + 1).ToString());
        }

        for (int i = 0; i < 5; ++i)
        {
            grade_range = r.Next(0, 100);
            if (i == range)      // 랜덤한 한 블럭을 강화블럭으로 셋팅
            {
                Debug.Log("range : " + range);
                if (i == 0)
                {
                    blockArr[i].GetComponent<SpriteRenderer>().sprite = jail_upgrade[0];                   
                }
                if (i == 1)
                {
                    blockArr[i].GetComponent<SpriteRenderer>().sprite = jail_upgrade[1];
                }
                if (i == 2)
                {
                    blockArr[i].GetComponent<SpriteRenderer>().sprite = jail_upgrade[2];
                }
                if (i == 3)
                {
                    blockArr[i].GetComponent<SpriteRenderer>().sprite = jail_upgrade[3];
                }
                if (i == 4)
                {
                    blockArr[i].GetComponent<SpriteRenderer>().sprite = jail_upgrade[4];
                }
                //blockArr[i].AddComponent<BlockStatusManager>();
                blockArr[i].GetComponent<BlockStatusManager>().BlockUpgrade();

            }
            else
            {
                if (i == range)
                {
                    //Debug.Log("range : " + range);
                    continue;
                }
                //blockArr[i] = jail_normal[i] as GameObject;
                //blockArr[i].AddComponent<BlockStatusManager>();
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

        if (BlockDestroy()) // 블럭이 모두 파괴되었을 때.
        {
            Debug.Log("블럭 모두 파괴");
            //for (int i = 0; i < 5; ++i)
            //{
            //    blockArr[i] = Instantiate(jail_normal[i], new Vector2(transform.position.x, (transform.position.y+(i+2))), transform.rotation) as GameObject;
            //}

            blockParents = Instantiate(Resources.Load("Prefabs/BlockGroup"),
            new Vector2(transform.position.x, (transform.position.y)), transform.rotation) as GameObject;

            if (null == block_gravity)
                block_gravity = blockParents.GetComponent<Rigidbody2D>();

            range = r.Next(0, 5);
            for (int i = 0; i < 5; ++i)
            {
                grade_range = r.Next(0, 100);
                blockArr[i] = GameObject.Find("BlockGroup(Clone)").transform.Find("building" + (i + 1).ToString()).gameObject;
                blockArr[i].AddComponent<BlockStatusManager>();

                if (i == range)      // 랜덤한 한 블럭을 강화블럭으로 셋팅
                {                  
                    blockArr[i].GetComponent<BlockStatusManager>().BlockUpgrade();
                }
                else
                    blockArr[i].GetComponent<BlockStatusManager>().BlockNormal();

            }
        }
        else 
        {
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

    // 블럭이 모두 파괴되었는지 체크
    public bool BlockDestroy()
    {
        GameObject obj = GameObject.Find("building5");
        if (obj != null)  // hierarchy 상에 오브젝트가 존재하면 블록 생성 x
            return false; 
        else
        {
            block_ypos_min = false;
            return true;
        }
    }
   

    // 블럭 생성 코루틴
    IEnumerator BlockGenerate()
    {
        while (true)
        {
            yield return new WaitUntil(BlockDestroy);   // 함수 리턴값이 true면 아래 코드 실행
            Debug.Log("BlockDestroy 리턴 값 : " + BlockDestroy());

            //blockParents = Instantiate(Resources.Load("Prefabs/BlockGroup"),
            //     new Vector2(transform.position.x, (transform.position.y)), transform.rotation) as GameObject;

            //if (null == block_gravity)
            //    block_gravity = blockParents.GetComponent<Rigidbody2D>();
        }
        //blockParents = Instantiate(blockGroup, new Vector2(transform.position.x, (transform.position.y) + 50)
        //    , transform.rotation) as GameObject;
    }

    IEnumerator BlockTranslate()
    {
        while (true)
        {
            yield return new WaitWhile(BlockDestroy);   // false면 아래 코드 실행
            //Debug.Log("BlockDestroy false");
        }
    }
}