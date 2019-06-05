using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// References the type of area of the composite shape.
/// </summary>
[Serializable]
public enum ECompositeType
{
    COMBINE,
    SUBTRACT,
    INTERSECT,
    DIFFERENCE,
}

/// <summary>
/// A touchable area as defined by a 2D collider, and the type of interaction it will have on a composite touch area.
/// </summary>
[Serializable]
public struct TouchArea
{
    public Collider2D area;
    public ECompositeType compositeType;
}

/// <summary>
/// A summation of multiple TouchAreas interacting as a composite.
/// </summary>
public class CompositeTouchArea: MonoBehaviour
{
	/// <summary>
	/// The initial collider placed into the composite.
	/// </summary>
    private Collider2D BaseArea { get { return m_touchAreas[0].area; } }

	/// <summary>
	/// The list of touch areas that makes up the entire composite
	/// </summary>
    [SerializeField]
    private TouchArea[] m_touchAreas = null;

	/// <summary>
	/// Checks if a point is within the bounds of the composite collider, taking into account the various composite types of each TouchArea.
	/// </summary>
	/// <param name="point">The point that is being checked to see if it is colliding with the composite.</param>
	/// <returns></returns>
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

	/// <summary>
	/// Finds the bounds of the entire composite area.
	/// </summary>
	/// <returns></returns>
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
