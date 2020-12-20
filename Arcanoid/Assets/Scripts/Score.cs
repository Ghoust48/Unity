
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour 
{
    [SerializeField]private Text _scoreText;
    public static int Point = 0;

    private void Awake()
    {
        _scoreText.text = Point.ToString();
    }

    public void AddPoint(int value)
    {
        Point += value;
        _scoreText.text = Point.ToString();
    }
}
