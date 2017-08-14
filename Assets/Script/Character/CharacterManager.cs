using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour {

    private SpriteRenderer[] CharacterSprite = new SpriteRenderer[16];

    public static CharacterManager instance = null;


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
    }
    

    void Start () {

        //GameObject baseObj = new GameObject("CharacterList");

        //for (int i = 0; i < 4; i++)
        //{
        //    for (int j = 0; j < 4; j++)
        //    {
        //        GameObject childObj = new GameObject(string.Format("Background{0}", i));
        //        SpriteRenderer sr = childObj.AddComponent<SpriteRenderer>();
        //        sr.sprite = Resources.Load("Background", typeof(Sprite)) as Sprite;
        //        childObj.transform.position = new Vector3((j + 1)-2, i-1, 0) ;
        //        childObj.transform.SetParent(baseObj.transform);

        //    }
        //    Selection.activeObject = baseObj;

        //}

      
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Main");
    }
}
