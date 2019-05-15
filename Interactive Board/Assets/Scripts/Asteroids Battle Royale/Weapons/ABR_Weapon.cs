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
    protected string m_bulletTye;
    protected uint m_ammo;

    public uint GetAmmo()
    {
        return m_ammo;
    }

    public float GetFireDelay()
    {
        return m_fireDelay;
    }

	virtual public bool Fire(Vector3 shipVelocity)
	{
		return true;
	}

}
