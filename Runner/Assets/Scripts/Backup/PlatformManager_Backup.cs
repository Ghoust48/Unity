﻿using System.Collections.Generic;
using UnityEngine;

public class PlatformManager_Backup : MonoBehaviour
{
	public Transform prefab;
	
	[Space]
	public int numberOfObjects;
	public float recycleOffset;
	
	[Space]
	public Vector3 startPosition;
	public Vector3 minSize;
	public Vector3 maxSize;	
	public Vector3 minGap;
	public Vector3 maxGap;
	
	[Space]
	public float minY;
	public float maxY;
	
	[Space]
	public Material[] materials;
	
	[Space]
	public PhysicMaterial[] physicMaterials;
	
	[Space] 
	//public CoinManager coin;

	private Vector3 _nextPosition;
	private Queue<Transform> _objectQueue = new Queue<Transform>();

	private Renderer _renderer;
	private Collider _collider;

	private void Start()
	{
		GameManager.GameStarted += GameStarted;
		GameManager.GameOver += GameOver;
		
		_nextPosition = startPosition;

		for (var i = 0; i < numberOfObjects; i++)
		{
			_objectQueue.Enqueue(Instantiate(prefab, new Vector3(0f, 0f, -100f), Quaternion.identity));
		}
        
		for (var i = 0; i < numberOfObjects; i++)
		{
			Recycle();
		}
	}

	private void Update()
	{
		if (_objectQueue.Peek().localPosition.x + recycleOffset < Runner.DistanceTraveled)
		{
			Recycle();
		}
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
//		coin.Spawned(position);
        
		var obj = _objectQueue.Dequeue();
		obj.localScale = scale;
		obj.localPosition = position;

		var materialIndex = Random.Range(0, materials.Length);

		obj.GetComponent<Renderer>().material = materials[materialIndex];
		obj.GetComponent<Collider>().material = physicMaterials[materialIndex];
		
		_objectQueue.Enqueue(obj);
		
		_nextPosition += new Vector3
		(
			Random.Range(minGap.x, maxGap.x) + scale.x,
			Random.Range(minGap.y, maxGap.y),
			Random.Range(minGap.z, maxGap.z)
		);

		if (_nextPosition.y < minY)
		{
			_nextPosition.y = minY + maxGap.y;
		}
		else if (_nextPosition.y > maxY)
		{
			_nextPosition.y = maxY - maxGap.y;
		}
	}

	private void GameStarted()
	{
		_nextPosition = startPosition;
		
		for (var i = 0; i < numberOfObjects; i++)
		{
			Recycle();
		}
		
		enabled = true;
	}

	private void GameOver()
	{
		enabled = false;
	}
}

