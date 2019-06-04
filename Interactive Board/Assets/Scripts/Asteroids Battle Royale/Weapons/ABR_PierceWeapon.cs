using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_PierceWeapon : ABR_Weapon
{
    //constructor
    public ABR_PierceWeapon()
    {
        m_bulletTye = eBulletType.PIERCE;
        m_fireDelay = 1.0f;
    }

    /// <summary>
    /// Method extending from Weapon; Used to spawn the type of bullets this weapon uses
    /// </summary>
    /// <param name="shipVelocity">The position in which the bullet will spawn</param>
    /// <returns>true if it was able to fire, false if not</returns>
    public override bool Fire(Vector3 shipVelocity)
    {
        //Grabs the next pooled object in the desired bullet pool
        ABR_PierceBullet bullet = ABR_GlobalInfo.BulletManager.GetObjectFromTaggedPool(m_bulletTye.ToString()).GetComponent<ABR_PierceBullet>();
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