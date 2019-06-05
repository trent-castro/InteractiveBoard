using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that handles logic for a specific weapon.
/// </summary>
public class ABR_ExplosionWeapon : ABR_Weapon
{
    public ABR_ExplosionWeapon()
    {
        m_bulletType = eBulletType.EXPLOSION;
        m_fireDelay = 2.5f;
    }
    /// <summary>
    /// Method extending from Weapon; Used to spawn the type of bullets this weapon uses
    /// </summary>
    /// <param name="shipVelocity">The position in which the bullet will spawn</param>
    /// <returns>true if it was able to fire, false if not</returns>
    public override bool Fire(Vector3 shipVelocity)
    {
        //grabs next object from desired bullet pool
        ABR_ExplosionBullet bullet = ABR_GlobalInfo.BulletManager.GetObjectFromTaggedPool(m_bulletType.ToString()).GetComponent<ABR_ExplosionBullet>();
        //sets up the bullet
        bullet.gameObject.SetActive(true);
        bullet.gameObject.transform.position = m_bulletSpawnLocation.position;
        bullet.gameObject.transform.rotation = gameObject.transform.rotation;
        //calculates fire direction
        Vector3 fireDir = m_bulletSpawnLocation.transform.up;
        //fire the bullet
        bullet.Fire(fireDir, shipVelocity);
        return true;
    }
}