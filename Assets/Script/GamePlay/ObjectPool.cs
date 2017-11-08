using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    public List<PooledObject> objectPool = new List<PooledObject>();

    private void Awake()
    {
        for(int i=0; i< objectPool.Count; ++i)
        {
            objectPool[i].Initialize(transform);
        }
    }

    public bool PushToPool(string itemName, GameObject item, Transform parent = null)
    {
        PooledObject pool = GetPoolItem(itemName);
        if (pool == null)
            return false;

        // parent 값이 지정된 경우에는 parent 값을 그대로 사용하고, 지정되지 않은 경우에는
        // ObjectPool 게임 오브젝트를 parent로 지정.
        pool.PushToPool(item, parent == null ? transform : parent);
        return true;
    }

    public GameObject PopFromPool(string itemName, Transform parent = null)
    {
        PooledObject pool = GetPoolItem(itemName);
        if (pool == null)
            return null;

        return pool.PopFromPool(parent);
    }

    PooledObject GetPoolItem(string itemName)
    {
        for (int i = 0; i < objectPool.Count; ++i)
        {
            if (objectPool[i].poolItemName.Equals(itemName))
                return objectPool[i];
        }

        Debug.LogWarning("There's no matched pool list");
        return null;
    }
}