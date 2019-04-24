using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcadeMenuManager : MonoBehaviour
{

	[SerializeField] List<Menu> Menus = null;
	[SerializeField] string currentMenu;
	[SerializeField] string preferredMenu;

	public void ChangeMenu(string menu)
	{
		if (Menus.Exists(a => a.name == preferredMenu))
		{
			Menus.Find(a => a.name == currentMenu).gameObject.SetActive(false);
			Menus.Find(a => a.name == menu).gameObject.SetActive(true);
			currentMenu = menu;
			Menus.Find(a => a.name == menu).ActivateMenu();
		}
		else
		{
			throw new System.Exception("Misspelled Menu name at Menu.ActivateMenu - " + this.gameObject.name
				+ ". Please check spelling of " + menu + "(menu) and the menu you are attempting to request.");
		}
	}

	public void ChangeScenes(string scene)
	{
		if (SceneManager.GetSceneByName(scene) != null)
		{
			SceneManager.LoadScene(scene);
		}
		else
		{
			throw new System.Exception("Sen I stg if you duplicated the game scene again I will fight you to the death." +
				" Otherwise, your scene is either not loaded in Build Settings, or you can't spell. Please check " + 
				scene +"(scene) in MenuManager.ChangesScenes on " + this.gameObject.name);
		}
	}
}
