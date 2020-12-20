
using UnityEngine;

public class Stars : MonoBehaviour
{
	private Transform[] _stars;

	private void Awake()
	{
		_stars = new Transform[3];

		for (int i = 0; i < _stars.Length; i++)
			_stars[i] = transform.GetChild(i);
		
//		for (int i = 0; i < _stars.Length; i++)
//		{
//			if(i < Ball.CountHeart) 
//				_stars[i].gameObject.SetActive(true);
//			else
//				_stars[i].gameObject.SetActive(false);
//		}
	}
	
}
