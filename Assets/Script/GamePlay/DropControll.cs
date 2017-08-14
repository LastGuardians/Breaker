using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropControll : MonoBehaviour {

    public float dropSpeed = -1.0f;
    public GameObject[] blockParents = new GameObject[5];

    public int hp = 0;
    void Start () {

        //StartCoroutine("BuildingGenerator");
        
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Block"), LayerMask.NameToLayer("Block"), true);

        //for (int i = 1; i < 6; ++i)
        //    //GameObject.FindGameObjectWithTag("block" + i.ToString()).GetComponent<DropControll>().hp = 1;
        //    gameObject.transform.Find("building1") = 1;

        //for (int i = 0; i < 5; ++i)
        //{
        //    //blockParents[i] = GameObject.Find("BlockManager").GetComponent<BlockGenerator>(); 
        //    blockParents[i] = transform.Find("building" + (i + 1).ToString()).gameObject;

        //}
    }


    void Update () {
        if (transform.position.y < 1.0f)
        {
            //for (int i = 0; i < 5; ++i)
                //Destroy(blockParents[0].gameObject);

        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Collision"))
        {
            //col_building.isTrigger = false;
        }

    }
    
}
