﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageColorReciever : ColorReciever
{
    public override void SetColors(NameAndColorPair[] colors)
    {
        GetComponent<Image>().color = GetColor(colors, m_colorToRecieve);

    }
}