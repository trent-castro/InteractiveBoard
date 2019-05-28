using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Locatable))]
public class LocatableColorReciever : ColorReciever
{
    public override void SetColors(List<NameAndColorPair> colors)
    {
        GetComponent<Locatable>().SetColors(colors);
    }
}
