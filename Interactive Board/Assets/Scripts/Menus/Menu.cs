using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
	[SerializeField] private string m_currentSubMenu = null;
	[SerializeField] private string m_launchMenu = null;
	[SerializeField] public List<GameObject> SubMenus = null;

	private void Start()
	{
		TurnOnSubMenu(m_launchMenu);
	}

	/// <summary>
	/// Switches active submenu to named menu.
	/// </summary>
	/// <param name="name">Name of submenu requested</param>
	public void TurnOnSubMenu(string name)
	{
		if (SubMenus.Exists(a => a.name == name))
		{
			SubMenus.Find(a => a.name == m_currentSubMenu).SetActive(false);
			SubMenus.Find(a => a.name == name).SetActive(true);
			m_currentSubMenu = name;
		}
		else
		{
			Debug.Log("Misspelled Menu name at Menu.TurnOnSubMenu - " + this.gameObject.name 
				+ ". Please check spelling of " + name + "(name) and the menu you are attempting to request.");
		}
	}
}
