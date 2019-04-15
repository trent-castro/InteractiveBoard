using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.AirHockey.UI;


public class AH_GameSetUpManager : MonoBehaviour
{
	[SerializeField] SpriteRenderer m_background;
	[SerializeField] SpriteRenderer m_puck;
	[SerializeField] SpriteRenderer m_player1;
	[SerializeField] SpriteRenderer m_player2;

	[SerializeField] Sprite[] m_backgrounds;
	[SerializeField] Sprite[] m_paddles;
	[SerializeField] Sprite[] m_pucks;
    // Start is called before the first frame update
    void Start()
    {
		ETheme theme = ETheme.NEUMONT;
		int player1 = 0;
		int player2 = 0;
		if (PlayerPrefs.HasKey("AirHockeyTheme"))
		{
			theme = (ETheme)PlayerPrefs.GetInt("AirHockeyTheme");
			player1 = PlayerPrefs.GetInt("AirHockeyPlayer1Puck");
			player2 = PlayerPrefs.GetInt("AirHockeyPlayer2Puck");
		}
		Debug.Log("Theme: " + theme + " Player 1: " + player1 + " Player2: " + player2);
		SetUpTheme(theme, player1, player2);
	}

	void SetUpTheme(ETheme theme, int player1, int player2)
	{
		if (m_puck) m_puck.sprite = m_pucks[(int)theme];
		if (m_background) m_background.sprite = m_backgrounds[(int)theme];
		switch (theme)
		{
			case ETheme.NEUMONT:
				m_player1.sprite = m_paddles[player1];
				m_player2.sprite = m_paddles[player2];
				break;
			case ETheme.SPACE:
				m_player1.sprite = m_paddles[player1 + 3]; // offset of space theme in array
				m_player2.sprite = m_paddles[player2 + 3];
				break;
			case ETheme.NATURE:
				m_player1.sprite = m_paddles[player1 + 6]; // offset of nature theme in array
				m_player2.sprite = m_paddles[player2 + 6];
				break;
		}
	}
}
