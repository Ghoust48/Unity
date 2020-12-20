using System.Collections.Generic;
using UnityEngine;

public class SkylineManager_Copy : MonoBehaviour
{
    public Transform prefab;
    public int numberOfObjects;
    public float recycleOffset;
    public Vector3 startPosition;
    public Vector3 minSize;
    public Vector3 maxSize;

    private Vector3 _nextPosition;
    private readonly Queue<Transform> _objectQueue = new Queue<Transform>();

    private void Start()
    {
        GameManager.GameStarted += GameStarted;
        GameManager.GameOver += GameOver;
        
        _nextPosition = startPosition;

        for (var i = 0; i < numberOfObjects; i++)
            _objectQueue.Enqueue(Instantiate(prefab));
        
        for (var i = 0; i < numberOfObjects; i++)
            Recycle();
        
    }

    private void Update()
    {
        if (_objectQueue.Peek().localPosition.x + recycleOffset < Runner.DistanceTraveled)
            Recycle();  
    }

    private void Recycle()
    {
        var scale = new Vector3
        (
            Random.Range(minSize.x, maxSize.x),
            Random.Range(minSize.y, maxSize.y),
            Random.Range(minSize.z, maxSize.z)
        );

        var position = _nextPosition;
        position.x += scale.x * 0.5f;
        position.y += scale.y * 0.5f;
        
        var obj = _objectQueue.Dequeue();
        obj.localScale = scale;
        obj.localPosition = position;
        _nextPosition.x += scale.x;
        _objectQueue.Enqueue(obj);
    }
    
    private void GameStarted()
    {
        _nextPosition = startPosition;
		
        for (var i = 0; i < numberOfObjects; i++)
            Recycle();
        
        enabled = true;
    }

    private void GameOver()
    {
        enabled = false;
    }
}

