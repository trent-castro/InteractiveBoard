using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum ECompositeType
{
    COMBINE,
    SUBTRACT,
    INTERSECT,
    DIFFERENCE,
}

[Serializable]
public struct TouchArea
{
    public Collider2D area;
    public ECompositeType compositeType;
}

public class CompositeTouchArea : MonoBehaviour
{
    private Collider2D BaseArea { get { return m_touchAreas[0].area; } }

    [SerializeField]
    private TouchArea[] m_touchAreas = null;

    public bool OverlapPoint(Vector2 point)
    {
        if (!BaseArea) return false;
        bool result = BaseArea.OverlapPoint(point);

        for (int i = 1; i< m_touchAreas.Length; i++)
        {
            if (m_touchAreas[i].area == null) continue;

            bool areaContains = m_touchAreas[i].area.OverlapPoint(point);

            switch (m_touchAreas[i].compositeType)
            {
                case ECompositeType.COMBINE:
                    result = result || areaContains;
                    break;
                case ECompositeType.SUBTRACT:
                    result = result && !areaContains;
                    break;
                case ECompositeType.INTERSECT:
                    result = result && areaContains;
                    break;
                case ECompositeType.DIFFERENCE:
                    result = (result || areaContains) && !(result && areaContains);
                    break;
            }
        }
        return result;
    }

    public Bounds GetBounds()
    {
        if (BaseArea == null) return new Bounds();

        Bounds result = BaseArea.bounds;
        for (int i = 1; i < m_touchAreas.Length; i++)
        {
            if (m_touchAreas[i].area == null) continue;

            Bounds toAdd = m_touchAreas[i].area.bounds;
            result.Encapsulate(toAdd);
        }
        return result;
    }
}
