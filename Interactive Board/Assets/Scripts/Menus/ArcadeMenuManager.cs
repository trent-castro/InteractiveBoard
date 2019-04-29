using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcadeMenuManager : MonoBehaviour
{

	[SerializeField] List<Menu> m_Menus = null;
	[SerializeField] string m_currentMenu = null;
	[SerializeField] string m_launchMenu= null;
	private void Start()
	{
		ChangeMenu(m_launchMenu);
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
		{
			SceneManager.LoadScene(scene);
		}
		//else
		//{
		//	Debug.Log("Your scene is either not loaded in Build Settings, or you can't spell. Please check " + 
		//		scene +"(scene) in MenuManager.ChangesScenes on " + this.gameObject.name);
		//}
	}
}
