﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ABR_ExplosionWeapon : ABR_Weapon
{
    public ABR_ExplosionWeapon()
    {
        m_bulletTye = eBulletType.EXPLOSION.ToString();
        m_ammo = 5;
        m_fireDelay = 2.5f;
    }

    public override void Fire(Vector3 firePos, Vector3 fireDir)
    {
        ABR_ExplosionBullet bullet = ABR_GlobalInfo.BulletManager.GetObjectFromTaggedPool(m_bulletTye).GetComponent<ABR_ExplosionBullet>();
        bullet.gameObject.SetActive(true);
        bullet.gameObject.transform.position = (Vector3)firePos;

        bullet.Fire(fireDir);
    }
}
