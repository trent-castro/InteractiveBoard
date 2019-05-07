using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_ShotgunWeapon : ABR_Weapon
{
    private uint m_numOfBullets = 9;
    private float m_shotAngle = 60;

    /// <summary>
    /// ABR_Shotgun Constructor; Sets the bullet type and the amount of ammo it has
    /// </summary>
    public ABR_ShotgunWeapon()
    {
        m_bulletTye = eBulletType.SHOTGUN.ToString();
        m_ammo = 10;
        m_fireDelay = 1.0f;
    }
    /// <summary>
    /// Method extending from Weapon; Used to spawn the type of bullets this weapon uses
    /// </summary>
    /// <param name="shipVelocity">The position in which the bullet will spawn</param>
    /// <param name="fireDirection">The direction in which the bullet will travel</param>
    public override bool Fire(Vector3 shipVelocity)
    {
        if (m_ammo > 0 )
        {
            //set base vector
            Vector3 baseDir = Quaternion.AngleAxis(-30, Vector3.back) * m_bulletSpawnLocation.up;
            //grab m_numOfBullets number of bullets
            for (int i = 0; i < m_numOfBullets; i++)
            {
                //Find Bullet from the Global BullePoolManager
                ABR_Bullet bullet = ABR_GlobalInfo.BulletManager.GetObjectFromTaggedPool(m_bulletTye).GetComponent<ABR_Bullet>();
                //Set Bullet Traits
                bullet.gameObject.SetActive(true);
                bullet.gameObject.transform.position = m_bulletSpawnLocation.position;
                //set fireDirection
                Vector3 fireDirection = m_bulletSpawnLocation.up;
                //get a random angle within the shot angle
                float randomNum = Random.Range(0, m_shotAngle);
                //rotate bullets fire direction by random angle
                fireDirection = Quaternion.AngleAxis(randomNum, Vector3.back) * baseDir;
                //Call the bullet fire method
                bullet.Fire(fireDirection, shipVelocity);
            }
            //reduce Ammo by one
            m_ammo--;
            return true;
        }
        return false;
    }

}
