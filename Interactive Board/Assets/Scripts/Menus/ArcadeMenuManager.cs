using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcadeMenuManager : MonoBehaviour
{

	[SerializeField] List<Menu> m_Menus = null;
	[SerializeField] string m_currentMenu = null;
	[SerializeField] string m_launchMenu= null;

	[SerializeField] GameObject m_timeWarning = null;

	[SerializeField] float m_timer = 30;
	[SerializeField] float m_time = 0;
	bool HasOpenedTab = false;
	private void Start()
	{
		ChangeMenu(m_launchMenu);
		Time.timeScale = 1.0f;
	}

	void Update()
	{
		m_time += Time.unscaledDeltaTime;
		if (m_time >= m_timer / 2)
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
		Application.OpenURL("https://interactive.neumont.edu/");
	}
}
