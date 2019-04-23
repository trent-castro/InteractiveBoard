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

    [Header("Configuration")]
	[SerializeField] 
    [Tooltip("References to the UI elements displaying the puck")]
    private GameObject[] m_selections = null;
	[SerializeField] 
    [Tooltip("The options available with the basic theme")]
    private Sprite[] m_themeBasic = null;
	[SerializeField]
    [Tooltip("The options available with the basic theme")]
    private Sprite[] m_themeNature = null;
	[SerializeField]
    [Tooltip("The options available with the basic theme")]
    private Sprite[] m_themeSpace = null;

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
		SetTheme((int)AH_SetUpMenu.defaultTheme);
        if(!(m_selections.Length != m_themeBasic.Length) || !(m_selections.Length != m_themeNature.Length)
            || !(m_selections.Length != m_themeSpace.Length))
        {
            DebugLog("The UI Set up selector has detected an inconsistency in the amount of " +
                "assets for themes.");
        }
    }

    /// <summary>
    /// Sets the theme of the game based on the enum state
    /// </summary>
    /// <param name="theme"></param>
	public void SetTheme(int theme)
	{
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
		for (int i = 0; i < m_selections.Length; i++)
		{
			m_selections[i].GetComponent<Image>().sprite = selection[i];
		}
	}
}
