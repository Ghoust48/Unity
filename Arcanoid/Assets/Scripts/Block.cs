
using UnityEngine;

public class Block : MonoBehaviour
{
	private int _countOfHits;
	private Score _score;

	public int point;
	public int hitsToKill;

	private void Awake()
	{
		_countOfHits = 0;
		_score = FindObjectOfType<Score>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		
		if (collision.gameObject.tag == "Ball")
		{
			_countOfHits++;

			if (_countOfHits == hitsToKill)
			{
				Destroy(this.gameObject);
				_score.AddPoint(point);
			}
		}
	}
}
