using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ABR_BasicWeapon : ABR_Weapon
{
    public ABR_BasicWeapon()
    {
        m_bulletTye = eBulletType.BASIC.ToString();
        m_fireDelay = 0.2f;
    }

    public override bool Fire(Vector3 shipVelocity)
    {
        ABR_Bullet bullet = ABR_GlobalInfo.BulletManager.GetObjectFromTaggedPool(m_bulletTye).GetComponent<ABR_Bullet>();
        bullet.gameObject.SetActive(true);
        bullet.gameObject.transform.position = m_bulletSpawnLocation.position;
        Vector3 fireDir = m_bulletSpawnLocation.transform.up;

        bullet.Fire(fireDir, shipVelocity);
        return true;
    }
}