using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eBulletType
{
    BASIC,
    EXPLOSION,
    PIERCE,
    LASER
}

public abstract class Weapon
{
    float m_fireDelay;
    string m_bulletTye;
    uint m_ammo;
    abstract public void Fire();
}

public class Shotgun : Weapon
{
    public Shotgun()
    {

    }

    public override void Fire()
    {
      
    }
}
