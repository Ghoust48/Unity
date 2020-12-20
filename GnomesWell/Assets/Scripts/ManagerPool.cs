using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerPool : Singleton<ManagerPool>, IDisposable
{
    private Dictionary<int, Pool> pools = new Dictionary<int, Pool>();

//    public Pool PopulateWith(PoolType id,GameObject prefab, int amount)
//    {
//        var  obj = pools[(int) id].PopulateWith(prefab, amount);
//        return obj;
//    }     
    
    public Pool AddPool(PoolType id, bool reparent = true)
    {
        Pool pool;
        if (pools.TryGetValue((int) id, out pool) == false)
        {
            pool = new Pool();
            pools.Add((int) id, pool);
            if (reparent)
            {
                var poolsGO = GameObject.Find("PoolObjects") ?? new GameObject("PoolObjects");
                var poolGO = new GameObject("Pool" + id);
                poolGO.transform.SetParent(poolsGO.transform);
                pool.SetParent(poolGO.transform);
            }
        }
        return pool;
    }
    
    public GameObject Spawn(PoolType id, GameObject prefab, Vector3 position = default(Vector3), Transform parent = null)
    {
        return pools[(int) id].Spawn(prefab, position, parent);
    }

    public T Spawn<T>(PoolType id, GameObject prefab, Vector3 position = default(Vector3), Transform parent = null) where T : class
    {
        var val = pools[(int) id].Spawn(prefab, position, parent);
        return val.GetComponent<T>();
    }

    public void Despawn(PoolType id, GameObject obj)
    {
        pools[(int)id].Despawn(obj);
    }

    public  void Dispose()
    {
        foreach (var poolsValue in pools.Values)
            poolsValue.Dispose();
        pools.Clear();
    }
}


