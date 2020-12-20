using System.Collections.Generic;
using UnityEngine;

public class SkylineManager : MonoBehaviour
{
    [SerializeField] protected Transform prefab;
	
    [Space]
    [SerializeField] protected int numberOfObjects;
    [SerializeField] protected float recycleOffset;
	
    [Space]
    [SerializeField] protected Vector3 startPosition;
    
    
    public RecycleObjects RecycleObjects { get; set; }
    
    [Space]
    [SerializeField] private Material[] materials;

    private Vector3 _nextPosition;
    private readonly Queue<Transform> _objectQueue = new Queue<Transform>();

    private void Awake()
    {
    }

    private void Start()
    {
        GameManager.GameStarted += GameStarted;
        GameManager.GameOver += GameOver;
        
        _nextPosition = startPosition;

        for (var i = 0; i < numberOfObjects; i++)
            _objectQueue.Enqueue(Instantiate(prefab, transform, true));
        
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
        RecycleObjects.Recycle();
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

