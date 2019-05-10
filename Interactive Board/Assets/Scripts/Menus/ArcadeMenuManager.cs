using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ArcadeMenuManager : MonoBehaviour
{
	[Tooltip("All menu roots.")]
	[SerializeField] List<Menu> m_Menus = null;
	[Tooltip("The currently active menu.")]
	[SerializeField] string m_currentMenu = null;
	[Tooltip("The menu root that will be lauched first on startup.")]
	[SerializeField] string m_launchMenu= null;

	[SerializeField] GameObject m_timeWarning = null;

	[SerializeField] float m_timer = 30;
	float m_time = 0;
	bool HasOpenedTab = false;
	private void Start()
	{
		foreach (var item in m_Menus)
		{
			item.gameObject.SetActive(false);
		}
		ChangeMenu(m_launchMenu);
		Time.timeScale = 1.0f;
	}

	void Update()
	{
		if (Input.anyKeyDown)
		{
			m_time = 0;
		}
		m_time += Time.unscaledDeltaTime;
		if (m_time >=( m_timer - 10))
		{
			TurnOnWarning();
		}
		if (m_time >= m_timer && !HasOpenedTab)
		{
			HasOpenedTab = true;
			TimedReturnToMap();
		}
	}

	public void ChangeMenu(string menu)
	{
		if (m_Menus.Exists(a => a.name == m_launchMenu))
		{
			m_Menus.Find(a => a.name == m_currentMenu).gameObject.SetActive(false);
			m_Menus.Find(a => a.name == menu).gameObject.SetActive(true);
			m_currentMenu = menu;
		}
		else
		{
			Debug.Log("Misspelled Menu name at Menu.ActivateMenu - " + this.gameObject.name
				+ ". Please check spelling of " + menu + "(menu) and the menu you are attempting to request.");
		}
	}

	public void ChangeScenes(string scene)
	{
		SceneManager.LoadScene(scene);
	}

	public void ResetMapTimer()
	{
		m_time = 0;
		HasOpenedTab = false;
	}

	private void TurnOnWarning()
	{
		m_timeWarning.SetActive(true);
	}

	private void TimedReturnToMap()
	{
		if (!Application.isEditor) Application.OpenURL("https://interactive.neumont.edu/");
	}
}
