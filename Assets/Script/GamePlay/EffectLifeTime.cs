using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectLifeTime : MonoBehaviour
{

    float lifeTime = 1000;

    // Update is called once per frame
    void Update()
    {
        lifeTime = lifeTime - Time.deltaTime * 1000;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }

    }
}
