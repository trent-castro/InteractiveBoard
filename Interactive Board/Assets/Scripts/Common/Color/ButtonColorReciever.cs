using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonColorReciever : ColorReciever
{
    public override void SetColors(NameAndColorPair[] colors)
    {
        ColorBlock buttonColors = GetComponent<Button>().colors;

        buttonColors.normalColor = colors.Where(ncp => ncp.name.Equals(m_colorToRecieve)).Select(npc => npc.color).FirstOrDefault();

        GetComponent<Button>().colors = buttonColors;
    }
}
