using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locatable : MonoBehaviour
{
    [SerializeField]
    private Indicator m_indicatorPrefab = null;

    [SerializeField]
    private float m_spaceFromCenter = 40;

    public float SpaceFromCenter => m_spaceFromCenter;

    [SerializeField]
    private float m_indicationRange = 1000000f;

    public float IndicationRange => m_indicationRange;

    private List<NameAndColorPair> m_colors = new List<NameAndColorPair>();

    public void SetColors(List<NameAndColorPair> colors)
    {
        foreach (NameAndColorPair npc in colors)
        {
            m_colors.Add(npc);
        }
    }

    [SerializeField]
    private Sprite m_innerImage = null;

    private void Awake()
    {
        LocatorManager.Instance.Register(this);
    }

    public Indicator GetIndicatorInstance(Transform parent)
    {
        Indicator indicator = Instantiate(m_indicatorPrefab, parent);

        indicator.AddColors(m_colors);

        indicator.SetColors();

        indicator.SetInnerImage(m_innerImage);

        indicator.SetFollowTarget(transform);

        return indicator;
    }
}
