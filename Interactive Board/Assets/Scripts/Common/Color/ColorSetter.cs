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
    private List<NameAndColorPair> m_Colors = null;

    private void Awake()
    {
        SetColors();
    }

    public void SetColors()
    {
        ColorReciever[] colorRecievers = GetComponentsInChildren<ColorReciever>(true);

        foreach (ColorReciever colorReciever in colorRecievers)
        {
            colorReciever.SetColors(m_Colors);
        }
    }

    public void AddColor(string name, Color color)
    {
        m_Colors.Add(new NameAndColorPair()
        {
            name = name,
            color = color
        });
    }

    public void AddColors(List<NameAndColorPair> colors)
    {
        foreach(NameAndColorPair npc in colors)
        {
            m_Colors.Add(npc);
        }
    }

    public void RemoveColor(string name, Color color)
    {
        m_Colors.RemoveAll(npc => npc.name == name);
    }
}
