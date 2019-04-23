using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// marked for fixing
public class AH_MainMenuManager : MonoBehaviour
{
	[SerializeField] GameObject m_Title = null;
	[SerializeField] GameObject m_SetUp = null;
	static bool hasSeenMainMenu = false;

	private void Start()
	{
		if (hasSeenMainMenu)
		{
			m_Title.SetActive(false);
			m_SetUp.SetActive(true);
		}
		else
		{
			hasSeenMainMenu = true;
		}
	}
}
