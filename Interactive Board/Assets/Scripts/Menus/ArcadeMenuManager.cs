using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeMenuManager : MonoBehaviour
{

	[SerializeField] List<Menu> Menus = null;
	[SerializeField] string currentMenu;
	[SerializeField] string preferredMenu;

	public void ChangedPreferredMenu()
	{

	}
}
