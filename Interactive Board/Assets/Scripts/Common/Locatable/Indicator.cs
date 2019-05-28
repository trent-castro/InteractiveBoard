using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : ColorSetter
{
    [SerializeField]
    private Image m_innerImage = null;

    [SerializeField]
    private FollowRotation m_innerImageRotator = null;

    public void SetInnerImage(Sprite sprite)
    {
        if (m_innerImage != null)
        {
            m_innerImage.sprite = sprite;
        }
    }

    public void SetFollowTarget(Transform target)
    {
        m_innerImageRotator.m_target = target;
    }
}
