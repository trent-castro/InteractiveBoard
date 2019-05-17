using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_LaserWeapon : ABR_Weapon
{
    public ABR_LaserWeapon()
    {
        m_bulletTye = eBulletType.LASER;
        m_fireDelay = 1.0f;
    }

    public override bool Fire(Vector3 shipVelocity)
    {
        ABR_LaserBullet bullet = ABR_GlobalInfo.BulletManager.GetObjectFromTaggedPool(m_bulletTye.ToString()).GetComponent<ABR_LaserBullet>();
        bullet.gameObject.SetActive(true);
        bullet.gameObject.transform.position = m_bulletSpawnLocation.position;
        Vector3 fireDir = m_bulletSpawnLocation.transform.up;

        bullet.Fire(fireDir, shipVelocity);
        return true;
    }
}
