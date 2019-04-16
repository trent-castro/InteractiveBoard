using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AH_GameOverUI : MonoBehaviour
{

	public void PlayAgain()
	{
		SceneManager.LoadScene("AH_Scene01");
	}

	public void QuitGame()
	{
		SceneManager.LoadScene("ArcadeMenu");
	}
}
