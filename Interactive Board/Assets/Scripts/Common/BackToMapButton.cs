using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A monobehaviour for buttons to return to the map website.
/// </summary>
public class BackToMapButton : MonoBehaviour
{
	/// <summary>
	/// This function redirects the page back to the map webstie url.
	/// </summary>
    public void Back()
    {
        Application.OpenURL("https://interactive.neumont.edu/");
    }
}
