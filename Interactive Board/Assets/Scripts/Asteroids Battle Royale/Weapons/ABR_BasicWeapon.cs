using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that handles logic for a specific weapon.
/// </summary>
public class ABR_BasicWeapon : ABR_Weapon
{
    public ABR_BasicWeapon()
    {
        m_bulletType = eBulletType.BASIC;
        m_fireDelay = 0.2f;
    }
    /// <summary>
    /// Method extending from Weapon; Used to spawn the type of bullets this weapon uses
    /// </summary>
    /// <param name="shipVelocity">The position in which the bullet will spawn</param>
    /// <returns>true if it was able to fire, false if not</returns>
    public override bool Fire(Vector3 shipVelocity)
    {
        //Grabs next object from desired bullet pool
        ABR_Bullet bullet = ABR_GlobalInfo.BulletManager.GetObjectFromTaggedPool(m_bulletType.ToString()).GetComponent<ABR_Bullet>();
        //sets up the bullet
        bullet.gameObject.SetActive(true);
        bullet.gameObject.transform.position = m_bulletSpawnLocation.position;
        //calculates fire direction
        Vector3 fireDir = m_bulletSpawnLocation.transform.up;
        //fires the bullet
        bullet.Fire(fireDir, shipVelocity);
        return true;
    }
}