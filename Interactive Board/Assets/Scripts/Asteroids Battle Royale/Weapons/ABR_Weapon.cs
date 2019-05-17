using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eBulletType
{
    BASIC,
    SHOTGUN,
    EXPLOSION,
    PIERCE,
    LASER
}

public abstract class ABR_Weapon : MonoBehaviour
{
    public Transform m_bulletSpawnLocation;
    protected float m_fireDelay;
    protected eBulletType m_bulletTye;

    public float GetFireDelay()
    {
        return m_fireDelay;
    }

    public eBulletType GetBulletType()
    {
        return m_bulletTye;
    }

	virtual public bool Fire(Vector3 shipVelocity)
	{
		return true;
	}

}
