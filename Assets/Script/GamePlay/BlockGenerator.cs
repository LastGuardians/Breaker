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
    public GameObject blockGroup;
    public GameObject[] blockArr = new GameObject[5];
    public Block[] blockData = new Block[5];

    public float span = 10.0f;
    public float delta = 0;

    public int hp = 0;      // 블록 hp

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
                
        //Instantiate(Resources.Load("Background/BlockGroup"),
        //    new Vector2(transform.position.x, (transform.position.y) + 15), transform.rotation) ;



        for (int i = 0; i < 5; ++i)
        {
            blockData[i].hp = 1;
            blockArr[i] = transform.Find("BlockGroup").transform.Find("building" + (i + 1).ToString()).gameObject;
        

        }
        //blockParents = Instantiate(blockGroup, new Vector2(transform.position.x, (transform.position.y) + 15)
        //    , transform.rotation) as GameObject;
    }


    void Start()
    {
     
        for (int i = 0; i < 5; ++i)
        {
            blockArr[i].AddComponent<DropControll>().hp = 1;
        }
        BlockDestroy();
        //StartCoroutine(BlockGenerate());
    }

    void Update()
    {

        //this.delta += Time.deltaTime;
        //if (this.delta > this.span)
        //{
        //    this.delta = 0;

        //     Instantiate(blockGroup, new Vector2(transform.position.x, (transform.position.y) + 15), transform.rotation);
        
        //}
        BlockDestroy(); // 나중에 삭제

        //if (blockParents.transform.position.y < 1.0f)
        //{
        //    if (blockParents != null)
        //        Destroy(blockParents.gameObject);
        //}
    }

    public void BlockHpControll()
    {
        this.delta += Time.deltaTime;

        if (this.delta > this.span)
        {
            hp += 1;
        }

    }

    public bool BlockDestroy()
    {
        GameObject obj = GameObject.Find("building5");
        if (obj != null)  // hierarchy 상에 오브젝트가 존재하면 블록 생성 x
            return false;
        else
            return true;
    }

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