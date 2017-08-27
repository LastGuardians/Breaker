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
    public Collider2D[] blockColArr = new Collider2D[5];
    //public Block[] blockData = new Block[5];
    public bool block_ypos_min = false;

    float span = 10.0f;
    float delta = 0; 

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

        //blockParents.AddComponent<BlockStatusManager>();
        //blockParents = Instantiate(blockGroup, new Vector2(transform.position.x, (transform.position.y) + 15)
        //    , transform.rotation) as GameObject;
    }


    void Start()
    {
        BlockDestroy();
        //StartCoroutine(BlockGenerate());
    }

    void Update()
    {

        if (BlockDestroy()) // 블럭이 모두 파괴되었을 때.
        {
            blockParents = Instantiate(Resources.Load("Background/BlockGroup"),
            new Vector2(transform.position.x, (transform.position.y) + 20), transform.rotation) as GameObject;
            //blockParents.AddComponent<BlockGenerator>();

            for (int i = 0; i < 5; ++i)
            {
                blockArr[i] = GameObject.Find("BlockGroup(Clone)").transform.Find("building" + (i + 1).ToString()).gameObject;
                blockArr[i].AddComponent<BlockStatusManager>();
            }
        }
        else 
        {
            for (int i = 0; i < 5; ++i)
            {
                if (blockArr[i] == null)
                    continue;

                if (blockArr[i].transform.position.y < -0.3 &&
                    blockArr[i].transform.position.y >= -1.7)
                {
                    Debug.Log("블럭 ypos -0.3 미만");
                    block_ypos_min = true;
                }              
            }
        }

        //if (blockColArr[0].transform.position.y > 0
        //            || blockColArr[4] == null)
        //{
        //    block_ypos_min = false;
        //}
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
        yield return new WaitUntil(BlockDestroy);   // 함수 리턴값이 true면 아래 코드 실행
        Debug.Log("BlockDestroy 리턴 값 : " + BlockDestroy());

        blockParents = Instantiate(Resources.Load("Background/BlockGroup"),
        new Vector2(transform.position.x, (transform.position.y) + 20), transform.rotation) as GameObject;

        //blockParents = Instantiate(blockGroup, new Vector2(transform.position.x, (transform.position.y) + 50)
        //    , transform.rotation) as GameObject;                    

    }
}