using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAnimation : MonoBehaviour {

    public Animator crack1;

    //public static EffectAnimation instance = null;

    // Use this for initialization
    void Start () {
        //if (instance == null)
        //{
        //    instance = this;
        //}
        //else if (instance != this)
        //{
        //    //잘못된 인스턴스를 가르키고 있을 경우
        //    Destroy(gameObject);
        //}
        crack1 = gameObject.GetComponent<Animator>();
    }
	
	public void Crack1()
    {
        crack1.SetTrigger("crack1");
    }

    public void Crack2()
    {
        crack1.SetTrigger("crack2");
    }
}
