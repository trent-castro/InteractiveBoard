using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMapButton : MonoBehaviour
{
    public void Back()
    {
        Application.OpenURL("https://interactive.neumont.edu/");
    }
}
