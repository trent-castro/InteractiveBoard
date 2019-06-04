using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteColorReciever : ColorReciever
{
    public override void SetColors(List<NameAndColorPair> colors)
    {
        GetComponent<SpriteRenderer>().color = GetColor(colors, m_colorToRecieve, GetComponent<SpriteRenderer>().color);
    }
}
