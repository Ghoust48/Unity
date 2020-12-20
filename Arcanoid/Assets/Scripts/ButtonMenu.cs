
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonMenu : MonoBehaviour
{
	public void MenuPressed()
	{
		SceneManager.LoadScene("Menu");
	}
}
