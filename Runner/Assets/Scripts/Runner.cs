using UnityEngine;

public class Runner : MonoBehaviour
{
    public static float DistanceTraveled;
    public float acceleration;
    public Vector3 jumpVelocity;

    public float gameOverY;

    private bool _touchingPlatform;
    private Rigidbody _rigidbody;
    private Renderer _renderer;
    private Vector3 _startPosition;

    private static int _coin;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        
        GameManager.GameStarted += GameStarted;
        GameManager.GameOver += GameOver;
        GameManager.GamePaused += GamePaused;
        GameManager.GamePlayed += GamePlayed;
        
        _startPosition = transform.localPosition;
        _renderer.enabled = false;
        _rigidbody.isKinematic = true;
        enabled = false;

    }

    private void Update()
    {
        
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (_touchingPlatform && Input.GetButtonDown("Jump"))
            {
                _rigidbody.AddForce(jumpVelocity, ForceMode.VelocityChange);
                _touchingPlatform = false;
            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!GameManager.IsGamePaused)
                {
                    GameManager.OnGamePaused();
                    GameManager.IsGamePaused = true;
                }
                else
                {
                    GameManager.OnGamePlayed();
                    GameManager.IsGamePaused = false;
                }
                
            }
        }
        
        if (Application.platform == RuntimePlatform.Android)
        {
            if (_touchingPlatform && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                _rigidbody.AddForce(jumpVelocity, ForceMode.VelocityChange);
                _touchingPlatform = false;
            }
//            
//            if (Input.GetKeyDown(KeyCode.Escape))
//            {
//                if (!GameManager.IsGamePaused)
//                {
//                    Debug.Log("Runner Pause");
//                    GameManager.OnGamePaused();
//                    GameManager.IsGamePaused = true;
//                }
//                else
//                {
//                    GameManager.OnGamePlayed();
//                    GameManager.IsGamePaused = false;
//                }
//                
//            }
        }
        
        DistanceTraveled = transform.localPosition.x;
        
        UIManager.SetDistance(DistanceTraveled);
        UIManager.SetCoin(_coin);

        if (transform.localPosition.y < gameOverY)
        {
            GameManager.OnGameOver();
        }
    }

    private void FixedUpdate()
    {
        if (_touchingPlatform)
        {
            _rigidbody.AddForce(acceleration, 0f, 0f, ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        _touchingPlatform = true;
    }

    private void OnCollisionExit(Collision other)
    {
        _touchingPlatform = false;       
    }
    
    private void GameStarted()
    {
        UIManager.SetCoin(_coin);
        DistanceTraveled = 0f;
        UIManager.SetDistance(DistanceTraveled);
        transform.localPosition = _startPosition;
        _renderer.enabled = true;
        _rigidbody.isKinematic = false;
        enabled = true;
    }

    private void GameOver()
    {
        Records.RecordDistance = DistanceTraveled;
        UIManager.SetRecord(DistanceTraveled);
        
        _renderer.enabled = false;
        _rigidbody.isKinematic = true;
        enabled = true;
    }

    private static void GamePaused()
    {
        Time.timeScale = 0;
    }

    private static void GamePlayed()
    {
        Time.timeScale = 1;
    }

    public static void AddCoin()
    {
        _coin++;
    }
}
