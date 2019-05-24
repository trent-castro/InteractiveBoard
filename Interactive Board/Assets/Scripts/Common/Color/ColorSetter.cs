using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct NameAndColorPair
{
    public string name;
    public Color color;
}

public class ColorSetter : MonoBehaviour
{
    [SerializeField]
    private NameAndColorPair[] m_Colors = null;

    private void Start()
    {
        ColorReciever[] colorRecievers = GetComponentsInChildren<ColorReciever>(true);

        foreach (ColorReciever colorReciever in colorRecievers)
        {
            colorReciever.SetColors(m_Colors);
        }
    }
}
