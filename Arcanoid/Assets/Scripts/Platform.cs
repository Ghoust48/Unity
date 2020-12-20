
using System;
using UnityEngine;

public class Platform : MonoBehaviour 
{
    [SerializeField] private float _speed;
    [SerializeField] private float _boundary;

    [SerializeField] private GameObject _ball;

    private Rigidbody2D _rigidbodyBall;
    
    
    private Vector2 _position;
    
    public delegate void Action();

    public event Action BallFlew;

    private void Awake ()
	{
		_position = transform.position;
		_rigidbodyBall = _ball.GetComponent<Rigidbody2D>();
		
		BallFlew += OnBallFlew;
	}
	
    private void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log("Ball is fly");
			if (BallFlew != null) 
				BallFlew.Invoke();
		}
		
		_position.x += Input.GetAxis("Horizontal") * _speed * Time.deltaTime;
		_position.x = Mathf.Clamp(_position.x, _boundary, Mathf.Abs(_boundary));

		transform.position = _position;
	}

    private void OnBallFlew()
    {
	    _ball.transform.SetParent(null);
	    _rigidbodyBall.AddForce(new Vector2(100f, 300));
    }
}
