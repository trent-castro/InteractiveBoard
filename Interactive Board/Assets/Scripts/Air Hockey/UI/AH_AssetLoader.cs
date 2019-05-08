using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.AirHockey.UI;

/// <summary>
/// Assists with loading assets at the beginning of gameplay
/// </summary>
public class AH_AssetLoader : MonoBehaviour
{
    [Header("Debug Mode")]
    [SerializeField]
    [Tooltip("Enables Debug Mode")]
    private bool m_debugMode = false;

    [Header("External References")]
	[SerializeField] SpriteRenderer m_background    = null;
	[SerializeField] SpriteRenderer m_foreground    = null;
	[SerializeField] SpriteRenderer m_puck          = null;
	[SerializeField] SpriteRenderer m_player1       = null;
	[SerializeField] SpriteRenderer m_player2       = null;
	[SerializeField] Transform m_particleParent		= null;

    [Header("Asset References")]
	[SerializeField] Sprite[] m_backgrounds = null;
	[SerializeField] Sprite[] m_foregrounds = null;
	[SerializeField] Sprite[] m_paddles     = null;
	[SerializeField] Sprite[] m_pucks       = null;
	[SerializeField] GameObject[] m_particleEffects = null;

    /// <summary>
    /// [DEBUG MODE] Records a message if debug mode is enabled.
    /// </summary>
    /// <param name="debugLog">The message to record</param>
    private void DebugLog(string debugLog)
    {
        if (m_debugMode)
        {
            Debug.Log(debugLog);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
		ETheme theme = ETheme.BASIC;
		int player1 = 0;
		int player2 = 0;
		if (PlayerPrefs.HasKey("AirHockeyTheme"))
		{
			theme = (ETheme)PlayerPrefs.GetInt("AirHockeyTheme");
			player1 = PlayerPrefs.GetInt("AirHockeyPlayer1Puck");
			player2 = PlayerPrefs.GetInt("AirHockeyPlayer2Puck");
		}
		DebugLog("Theme: " + theme + " Player 1: " + player1 + " Player2: " + player2);
		SetUpTheme(theme, player1, player2);
	}

	void SetUpTheme(ETheme theme, int player1, int player2)
	{
		if (m_puck) m_puck.sprite = m_pucks[(int)theme];
		if (m_background) m_background.sprite = m_backgrounds[(int)theme];
		if (m_foreground) m_foreground.sprite = m_foregrounds[(int)theme];
		if (m_particleParent) Instantiate<GameObject>(m_particleEffects[(int)theme], m_particleParent);
		switch (theme)
		{
			case ETheme.BASIC:
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
