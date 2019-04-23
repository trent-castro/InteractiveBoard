using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_MainMenuManager : MonoBehaviour
{
	[SerializeField] GameObject m_Title;
	[SerializeField] GameObject m_SetUp;
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
