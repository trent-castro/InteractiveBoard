using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.AirHockey.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the variables dealing with the themes and the selected pucks, saving them to player prefs.
/// </summary>
// marked for fixing
public class AH_SetUpMenu : MonoBehaviour
{
	static public AH_SetUpMenu instance;
	static public ETheme defaultTheme = ETheme.BASIC;
	[SerializeField] Sprite[] m_themeBackgrounds = null;
	[SerializeField] Image m_background = null;
	[SerializeField] Sprite[] m_themeForegrounds = null;
	[SerializeField] Image m_foreground = null;

	[SerializeField] TextMeshProUGUI m_pointsRequiredText = null;
	[Range(3, 7)] int m_pointsRequired = 3;

	ETheme m_theme;

	int m_player1 = 0;
	int m_player2 = 0;

    // Start is called before the first frame update
    void Start()
    {
		instance = this;
		SetUpTheme(defaultTheme);

    }

	void SetUpTheme(ETheme newTheme)
	{
		m_theme = newTheme;

		m_background.sprite = m_themeBackgrounds[(int)newTheme];
		m_foreground.sprite = m_themeForegrounds[(int)newTheme];
		
	}

	public ETheme GetCurrentTheme()
	{
		return m_theme;
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
	}

	public void SetPointsRequired(int offset)
	{
		m_pointsRequired += offset;
		if (m_pointsRequired < 3) m_pointsRequired = 3;
		if (m_pointsRequired > 7) m_pointsRequired = 7;

		m_pointsRequiredText.text = m_pointsRequired.ToString();
		PlayerPrefs.SetInt("AirHockeyPointsRequired", m_pointsRequired);
	}
}
