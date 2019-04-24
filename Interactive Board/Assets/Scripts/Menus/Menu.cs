using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
	[SerializeField] public string _name = null;
	[SerializeField] private string currentSubMenu = null;
	[SerializeField] private string launchMenu = null;
	[SerializeField] public List<GameObject> SubMenus = null;

	/// <summary>
	/// Activates the preferred submenu in this menu.
	/// </summary>
	public void ActivateMenu()
	{
		if (SubMenus.Exists(a => a.name == launchMenu))
		{
			SubMenus.Find(a => a.name == currentSubMenu).SetActive(false);
			SubMenus.Find(a => a.name == launchMenu).SetActive(true);
			currentSubMenu = launchMenu;
		}
		else
		{
			throw new System.Exception("Misspelled Menu name at Menu.ActivateMenu - " + this.gameObject.name
				+ ". Please check spelling of " + launchMenu + "(launchMenu) and the menu you are attempting to request.");
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="name"></param>
	public void TurnOnSubMenu(string name)
	{
		if (SubMenus.Exists(a => a.name == name))
		{
			SubMenus.Find(a => a.name == currentSubMenu).SetActive(false);
			SubMenus.Find(a => a.name == name).SetActive(true);
			currentSubMenu = name;
		}
		else
		{
			throw new System.Exception("Misspelled Menu name at Menu.TurnOnSubMenu - " + this.gameObject.name 
				+ ". Please check spelling of " + name + "(name) and the menu you are attempting to request.");
		}
	}
}
