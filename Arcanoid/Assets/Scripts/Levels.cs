
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levels : MonoBehaviour 
{
	public int numberLevel;
	
	public void LevelPressed()
	{
		SceneManager.LoadScene("Level" + numberLevel);
	}
}
