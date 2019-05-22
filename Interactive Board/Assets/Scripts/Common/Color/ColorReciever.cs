using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ColorReciever : MonoBehaviour
{
    [SerializeField]
    protected string m_colorToRecieve;

    public abstract void SetColors(NameAndColorPair[] colors);
}
