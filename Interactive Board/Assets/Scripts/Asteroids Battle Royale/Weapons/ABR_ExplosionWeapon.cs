﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ABR_ExplosionWeapon : ABR_Weapon
{
    public ABR_ExplosionWeapon()
    {
        m_bulletTye = eBulletType.EXPLOSION;
        m_fireDelay = 2.5f;
    }

    public override bool Fire(Vector3 shipVelocity)
    {
        ABR_ExplosionBullet bullet = ABR_GlobalInfo.BulletManager.GetObjectFromTaggedPool(m_bulletTye.ToString()).GetComponent<ABR_ExplosionBullet>();
        bullet.gameObject.SetActive(true);
        bullet.gameObject.transform.position = m_bulletSpawnLocation.position;
        bullet.gameObject.transform.rotation = gameObject.transform.rotation;
        Vector3 fireDir = m_bulletSpawnLocation.transform.up;

        bullet.Fire(fireDir, shipVelocity);
        return true;
    }
}
