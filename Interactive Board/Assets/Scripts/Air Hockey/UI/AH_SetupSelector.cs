using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.AirHockey.UI;

/// <summary>
/// Changes the actual sprites of the objects on screen to match the chosen scene
/// </summary>
public class AH_SetupSelector : MonoBehaviour
{
    [Header("Debug Mode")]
    [SerializeField]
    [Tooltip("Enable or Disable Debug Mode")]
    private bool m_debugMode = false;

	[SerializeField]
	[Tooltip("Any other selectors affected by this selector")]
	private AH_SetupSelector[] m_children = null;

	[Header("Configuration")]
	[SerializeField] 
    [Tooltip("The options for paddles available with the basic theme")]
    private Sprite[] m_themeBasic = null;
	[SerializeField]
    [Tooltip("The options for paddles available with the Nature theme")]
    private Sprite[] m_themeNature = null;
	[SerializeField]
    [Tooltip("The options for paddles available with the Space theme")]
    private Sprite[] m_themeSpace = null;

	[SerializeField] int player = 0;

	private ETheme m_currentTheme = ETheme.BASIC;
	private int m_currentIndex = 0;
    /// <summary>
    /// [DEBUG MODE] Records a message if debug mode is enabled.
    /// </summary>
    /// <param name="debugLog">The message to record</param>
    private void DebugLog(string message)
    {
        if (m_debugMode)
        {
            Debug.Log(message);
        }
    }
    
    /// <summary>
    /// Initializes the themes as well as checks for asset inconsistency
    /// </summary>
    void Start()
    {
		m_currentIndex = 0;
		SetTheme((int)AH_SetUpMenu.defaultTheme);
        //if(!(m_selections.Length != m_themeBasic.Length) || !(m_selections.Length != m_themeNature.Length)
        //    || !(m_selections.Length != m_themeSpace.Length))
        //{
        //    DebugLog("The UI Set up selector has detected an inconsistency in the amount of " +
        //        "assets for themes.");
        //}
    }

	public void CycleThroughSelections(int offset)
	{
		Sprite[] selection = m_themeBasic;
		switch (AH_SetUpMenu.instance.GetCurrentTheme())
		{
			case ETheme.BASIC:
				selection = m_themeBasic;
				break;
			case ETheme.NATURE:
				selection = m_themeNature;
				break;
			case ETheme.SPACE:
				selection = m_themeSpace;
				break;
		}

		if ((m_currentIndex + offset) >= selection.Length) m_currentIndex = 0;
		else if ((m_currentIndex + offset) < 0) m_currentIndex = selection.Length - 1;
		else m_currentIndex += offset;

		if (player == 1)
		{
			AH_SetUpMenu.instance.ChoosePaddlePlayer1(m_currentIndex);
		}
		else
		{
			AH_SetUpMenu.instance.ChoosePaddlePlayer2(m_currentIndex);
		}

		this.GetComponent<Image>().sprite = selection[m_currentIndex];

	}

	public void CycleThroughThemes(int offset)
	{
		if ((int)(m_currentTheme + offset) > 2) m_currentTheme = (ETheme)0;
		else if ((int)(m_currentTheme + offset) < 0) m_currentTheme = (ETheme)2;
		else m_currentTheme += offset;
		SetTheme((int)m_currentTheme);
		AH_SetUpMenu.instance.ChoosePuck((int)m_currentTheme);
		foreach (var item in m_children)
		{
			item.CycleThroughSelections(0);
		}
	}

	/// <summary>
	/// Sets the theme of the game based on the enum state
	/// </summary>
	/// <param name="theme"></param>
	public void SetTheme(int theme)
	{
		m_currentTheme = (ETheme)theme;
		Sprite[] selection = m_themeBasic;
		switch ((ETheme)theme)
		{
			case ETheme.BASIC:
				selection = m_themeBasic;
				break;
			case ETheme.NATURE:
				selection = m_themeNature;
				break;
			case ETheme.SPACE:
				selection = m_themeSpace;
				break;
		}

		if (m_currentIndex > selection.Length) m_currentIndex = selection.Length - 1;
		this.GetComponent<Image>().sprite = selection[m_currentIndex];
	}
}
