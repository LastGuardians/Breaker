using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLifeTime : MonoBehaviour {

    float lifeTime = 3000;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        lifeTime = lifeTime - Time.deltaTime * 1000;
        if(lifeTime <= 0)
        {
            Destroy(gameObject);
        }

    }
}
