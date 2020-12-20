using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


public class Pool : IDisposable
{

    private Transform _parentPool;
    private readonly Dictionary<int, Queue<GameObject>> _cachedObjects = new Dictionary<int, Queue<GameObject>>();
    private readonly Dictionary<int, int> _cachedIds = new Dictionary<int, int>();

//    public Pool PopulateWith(GameObject prefab, int amount)
//    {
//        var key = prefab.GetInstanceID();
//        Queue<GameObject> queue;
//        var isQueued = _cachedObjects.TryGetValue(key, out queue);
//        if (isQueued==false)
//        _cachedObjects.Add(key, new Queue<GameObject>());
//
//        for (var i = 0; i < amount; i++)
//        {
//            var go = Populate(prefab, Vector3.zero, _parentPool);
//            go.SetActive(false);
//            _cachedIds.Add(go.GetInstanceID(), key);
//            _cachedObjects[key].Enqueue(go);
//        }
//        
//        return this;
//    }

    public void SetParent(Transform parent)
    {
        _parentPool = parent;
    }

    public GameObject Spawn(GameObject prefab, Vector3 position = default(Vector3), Transform parent = null)
    {
        var key = prefab.GetInstanceID();
        Queue<GameObject> queue;
        var isQueued = _cachedObjects.TryGetValue(key, out queue);

        if (isQueued && queue.Count > 0)
        {
            var transform = queue.Dequeue().transform;
            transform.SetParent(parent);
            transform.gameObject.SetActive(true);
            if (parent) transform.position = position;
            else transform.localPosition = position;
            var poolable = transform.GetComponent<IPoolable>();
            if (poolable != null) poolable.OnSpawn();
            return transform.gameObject;
        }

        if (!isQueued) _cachedObjects.Add(key, new Queue<GameObject>());

        var createdPrefab = Populate(prefab, position, parent);
        _cachedIds.Add(createdPrefab.GetInstanceID(), key);
        return createdPrefab;
    }

    public void Despawn(GameObject go)
    {
        go.SetActive(false);
        _cachedObjects[_cachedIds[go.GetInstanceID()]].Enqueue(go);
        var poolable = go.GetComponent<IPoolable>();
        if (poolable != null) poolable.OnDespawn();
        if (_parentPool != null) go.transform.SetParent(_parentPool);
    }

    public void Dispose()
    {
        _parentPool = null;
        _cachedObjects.Clear();
        _cachedIds.Clear();
    }

    public GameObject Populate(GameObject prefab, Vector3 position = default(Vector3), Transform parent = null)
    {
        var go = Object.Instantiate(prefab, position, Quaternion.identity, parent).transform;
        if (parent == null) go.position = position;
        else go.localPosition = position;
        return go.gameObject;
    }
}
