using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
	[SerializeField] private Vector3 offset;
	[SerializeField] private Vector3 rotationVelocity;

	[Space]
	[SerializeField] private float recycleOffset;
	[SerializeField] private float spawnChance;

	private void Start()
	{
		GameManager.GameOver += GameOver;
		gameObject.SetActive(false);
	}

	private void Update()
	{
		if (transform.localPosition.x + recycleOffset < Runner.DistanceTraveled)
		{
			gameObject.SetActive(false);
			return;
		}
		transform.Rotate(rotationVelocity * Time.deltaTime);
	}

	public void Spawned(Vector3 position)
	{
		if (gameObject.activeInHierarchy || spawnChance <= Random.Range(0f, 100f))
		{
			return;
		}

		transform.localPosition = position + offset;
		gameObject.SetActive(true);
	}
	
	private void OnTriggerEnter(Collider other)
	{
        Runner.AddCoin();
        gameObject.SetActive(false);
	}

	private void GameOver()
	{
		gameObject.SetActive(false);
	}
}
