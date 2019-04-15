using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.AirHockey.UI;
using UnityEngine.SceneManagement;

public class AH_SetUpMenu : MonoBehaviour
{
	static public ETheme defaultTheme = ETheme.NEUMONT;
	[SerializeField] Sprite[] m_themeBackgrounds;
	[SerializeField] Image m_background;
	ETheme m_theme;

	int m_player1 = 0;
	int m_player2 = 0;

    // Start is called before the first frame update
    void Start()
    {
		SetUpTheme(defaultTheme);

    }

	void SetUpTheme(ETheme newTheme)
	{
		m_theme = newTheme;

		m_background.sprite = m_themeBackgrounds[(int)newTheme];
		
	}


	public void ChoosePuck(int choice)
	{
		SetUpTheme((ETheme) choice);

	}

	public void ChoosePaddlePlayer1(int choice)
	{
		m_player1 = choice;
	}

	public void ChoosePaddlePlayer2(int choice)
	{
		m_player2 = choice;
	}


	public void PlayGame()
	{
		PlayerPrefs.SetInt("AirHockeyTheme", (int)m_theme);
		PlayerPrefs.SetInt("AirHockeyPlayer1Puck", m_player1);
		PlayerPrefs.SetInt("AirHockeyPlayer2Puck", m_player2);

		SceneManager.LoadScene("AH_Scene01");
	}
}
