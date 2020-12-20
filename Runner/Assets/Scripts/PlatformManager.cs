using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
	[SerializeField] protected Transform prefab;
	
	[Space]
	[SerializeField] protected int numberOfObjects;
	[SerializeField] protected float recycleOffset;
	
	[Space]
	[SerializeField] protected Vector3 startPosition;
	
	[Space]
	[SerializeField] private PhysicMaterial[] physicMaterials;
	
	public RecycleObjects RecycleObjects { get; set; }
	
	private MaterialController _materialController;

	private Vector3 _nextPosition;
	private readonly Queue<Transform> _objectQueue = new Queue<Transform>();
//	private Recycle _recycle;
	
	private void Awake()
	{
	//	_recycle = GetComponent<Recycle>();
		_materialController = GetComponent<MaterialController>();
	}

	private void Start()
	{
		GameManager.GameStarted += GameStarted;
		GameManager.GameOver += GameOver;
		
		_nextPosition = startPosition;

		for (var i = 0; i < numberOfObjects; i++)
		{
			var obj = Instantiate(prefab, new Vector3(0f, 0f, -100f), Quaternion.identity);
			obj.SetParent(transform);
			_objectQueue.Enqueue(obj);
		}
        
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

