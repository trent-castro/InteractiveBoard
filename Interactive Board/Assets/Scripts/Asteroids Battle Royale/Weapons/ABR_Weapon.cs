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

public abstract class ABR_Weapon
{

    protected float m_fireDelay;
    protected string m_bulletTye;
    protected uint m_ammo;
    protected bool isOkayToFire = true;
    abstract public void Fire(Vector3 firePos, Vector3 fireDir);
    public uint GetAmmo()
    {
        return m_ammo;
    }

    protected void ResetIsOkayToFire()
    {
        isOkayToFire = true;
    }

}
