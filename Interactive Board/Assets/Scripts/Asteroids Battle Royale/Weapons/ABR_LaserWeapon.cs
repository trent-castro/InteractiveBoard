using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_LaserWeapon : ABR_Weapon
{
    //constructor
    public ABR_LaserWeapon()
    {
        m_bulletTye = eBulletType.LASER;
        m_fireDelay = 1.0f;
    }

    /// <summary>
    /// Method extending from Weapon; Used to spawn the type of bullets this weapon uses
    /// </summary>
    /// <param name="shipVelocity">The position in which the bullet will spawn</param>
    /// <returns>true if it was able to fire, false if not</returns>
    public override bool Fire(Vector3 shipVelocity)
    {
        //grabs a bullet from the global bullet pool
        GameObject go = ABR_GlobalInfo.BulletManager.GetObjectFromTaggedPool(m_bulletTye.ToString());
        ABR_LaserBullet bullet = go.GetComponentInChildren<ABR_LaserBullet>();
        //sets the bullet to active
        go.gameObject.SetActive(true);
        //sets the bullets position to the bullet spawn location
        go.gameObject.transform.position = m_bulletSpawnLocation.position;
        //sets the bullets rotation to the base ship rotation
        go.gameObject.transform.rotation = gameObject.transform.rotation;
        //calculates the firedirection
        Vector3 fireDir = m_bulletSpawnLocation.transform.up;
        //Fire the bullet
        bullet.Fire(fireDir, shipVelocity);
        //return true
        return true;
    }
}
