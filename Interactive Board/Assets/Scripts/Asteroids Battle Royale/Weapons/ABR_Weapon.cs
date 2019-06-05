using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bullet/weapon type Enum
/// </summary>
public enum eBulletType
{
    BASIC,
    SHOTGUN,
    EXPLOSION,
    PIERCE,
    LASER
}

/// <summary>
/// Abstract base class for all weapons
/// </summary>
public abstract class ABR_Weapon : MonoBehaviour
{

    //protected member variables
    public Transform m_bulletSpawnLocation;
    protected float m_fireDelay;
    protected eBulletType m_bulletType;

    /// <summary>
    /// Get the fireDelay of the weapon
    /// </summary>
    /// <returns>The current weapons fire delay as a float</returns>
    public float GetFireDelay()
    {
        return m_fireDelay;
    }

    /// <summary>
    /// Gets the Weapon/Bullet Type
    /// </summary>
    /// <returns>this objects current bullet type as an eBulletType</returns>
    public eBulletType GetBulletType()
    {
        return m_bulletType;
    }

    abstract public bool Fire(Vector3 shipVelocity);
}
