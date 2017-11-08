using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLifeTime : MonoBehaviour {

    public string poolItemName = "BlockParticle";
    public float lifeTime = 3f;
    public float elapsedTime = 0f;
	
	void Update () {
       if(GetTimer() > lifeTime)
        {
            SetTimer();
            ObjectPool.Instance.PushToPool(poolItemName, gameObject);
        }
    }

    float GetTimer()
    {
        return (elapsedTime += Time.deltaTime);
    }

    void SetTimer()
    {
        elapsedTime = 0f;
    }
}
