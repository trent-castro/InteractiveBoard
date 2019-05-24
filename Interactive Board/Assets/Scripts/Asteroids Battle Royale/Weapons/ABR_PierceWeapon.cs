using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_PierceWeapon : ABR_Weapon
{
    public ABR_PierceWeapon()
    {
        m_bulletTye = eBulletType.PIERCE;
        m_fireDelay = 1.0f;
    }

    public override bool Fire(Vector3 shipVelocity)
    {
        ABR_PierceBullet bullet = ABR_GlobalInfo.BulletManager.GetObjectFromTaggedPool(m_bulletTye.ToString()).GetComponent<ABR_PierceBullet>();
        bullet.gameObject.SetActive(true);
        bullet.gameObject.transform.position = m_bulletSpawnLocation.position;
        bullet.gameObject.transform.rotation = gameObject.transform.rotation;
        Vector3 fireDir = m_bulletSpawnLocation.transform.up;

        bullet.Fire(fireDir, shipVelocity);
        return true;
    }
}