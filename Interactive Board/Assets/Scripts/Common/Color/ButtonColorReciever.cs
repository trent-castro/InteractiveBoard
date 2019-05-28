using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonColorReciever : ColorReciever
{
    public override void SetColors(List<NameAndColorPair> colors)
    {
        ColorBlock buttonColors = GetComponent<Button>().colors;

        buttonColors.normalColor = GetColor(colors, m_colorToRecieve);

        GetComponent<Button>().colors = buttonColors;
    }
}
