using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text gameOverText;
    public Text instructionText;
    public Text runnerText;
    public Text distanceText;
    public Text coinText;
    public Text currentDistanceText;
    public Text recordDistanceText;
    public Text pauseText;
    
    public Image coinImage;

    private static UIManager _instance;

    private void Awake()
    {
        _instance = this;
        
        GameManager.GameStarted += GameStarted;
        GameManager.GameOver += GameOver;
        GameManager.GamePaused += GamePaused;
        GameManager.GamePlayed += GamePlayed;
        
        gameOverText.enabled = false;
        distanceText.enabled = false;
        coinText.enabled = false;
        coinImage.enabled = false;
        currentDistanceText.enabled = false;
        recordDistanceText.enabled = false;
        pauseText.enabled = false;
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Menu))
            {
                Application.Quit();
                return;
            }
            
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                GameManager.OnGameStarted();
            }
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetButtonDown("Jump"))
            {
                GameManager.OnGameStarted();
            }
        }
    }

    private void GameStarted()
    {
        coinText.enabled = true;
        coinImage.enabled = true;
        distanceText.enabled = true;
        
        pauseText.enabled = false;
        gameOverText.enabled = false;
        instructionText.enabled = false;
        runnerText.enabled = false;
        currentDistanceText.enabled = false;
        recordDistanceText.enabled = false;
        
        enabled = false;
    }

    private void GameOver()
    {
        pauseText.enabled = false;
        distanceText.enabled = false;
        coinText.enabled = false;
        coinImage.enabled = false;
        
        gameOverText.enabled = true;
        instructionText.enabled = true;
        currentDistanceText.enabled = true;
        recordDistanceText.enabled = true;
        
        enabled = true;
    }

    private void GamePaused()
    {
        pauseText.enabled = true;
        
        gameOverText.enabled = false;
        distanceText.enabled = false;
        coinText.enabled = false;
        coinImage.enabled = false;
        currentDistanceText.enabled = false;
        recordDistanceText.enabled = false;
    }

    private void GamePlayed()
    {
        distanceText.enabled = true;
        coinText.enabled = true;
        coinImage.enabled = true;
        
        pauseText.enabled = false;
        gameOverText.enabled = false;
        
        currentDistanceText.enabled = false;
        recordDistanceText.enabled = false;
    }

    public static void SetDistance(float distance)
    {
        _instance.distanceText.text = distance.ToString("f0");
    }

    public static void SetCoin(int coin)
    {
        _instance.coinText.text = coin.ToString();
    }

    public static void SetRecord(float distance)
    {
        var record = Records.RecordDistance;

        _instance.recordDistanceText.text = string.Format("Record: {0:f0}", record);
        _instance.currentDistanceText.text = string.Format("Current distance: {0:f0}", distance);
    }
}
