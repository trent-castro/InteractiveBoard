using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ColorReciever : MonoBehaviour
{
    [SerializeField]
    protected string m_colorToRecieve;

    public abstract void SetColors(NameAndColorPair[] colors);

    protected Color GetColor(NameAndColorPair[] colors, string name)
    {
        foreach(NameAndColorPair nameAndColorPair in colors)
        {
            if (nameAndColorPair.name == name)
            {
                return nameAndColorPair.color;
            }
        }

        return Color.clear;
    }
}
