
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
	public void PlayPressed()
	{
		SceneManager.LoadScene("Level1");
	}
	
	public void MapPressed()
	{
		SceneManager.LoadScene("Map");
	}
	
	public void ExitPressed()
	{
		Application.Quit();
	}
}
