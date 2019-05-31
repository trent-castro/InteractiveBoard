using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonColorReciever : ColorReciever
{

    [SerializeField]
    protected string m_highlightedColor;

    [SerializeField]
    protected string m_pressedColor;

    [SerializeField]
    protected string m_disabledColor;

    public override void SetColors(List<NameAndColorPair> colors)
    {
        ColorBlock buttonColors = GetComponent<Button>().colors;

        buttonColors.normalColor = GetColor(colors, m_colorToRecieve, buttonColors.normalColor);
        buttonColors.highlightedColor = GetColor(colors, m_highlightedColor, buttonColors.highlightedColor);
        buttonColors.pressedColor = GetColor(colors, m_pressedColor, buttonColors.pressedColor);
        buttonColors.disabledColor = GetColor(colors, m_disabledColor, buttonColors.disabledColor);

        GetComponent<Button>().colors = buttonColors;
    }
}
