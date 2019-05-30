using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AH_GameOverUI : MonoBehaviour
{

	public void PlayAgain()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void QuitGame()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene("Arcade Menu");
	}
}
