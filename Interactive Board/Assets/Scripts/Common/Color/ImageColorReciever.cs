using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageColorReciever : ColorReciever
{
    public override void SetColors(NameAndColorPair[] colors)
    {
        GetComponent<Image>().color = colors.Where(ncp => ncp.name.Equals(m_colorToRecieve)).Select(npc => npc.color).FirstOrDefault();
    }
}
