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
        GameObject go = ABR_GlobalInfo.BulletManager.GetObjectFromTaggedPool(m_bulletTye.ToString());
        ABR_LaserBullet bullet = go.GetComponentInChildren<ABR_LaserBullet>();
        go.gameObject.SetActive(true);
        go.gameObject.transform.position = m_bulletSpawnLocation.position;
        go.gameObject.transform.rotation = gameObject.transform.rotation;
        Vector3 fireDir = m_bulletSpawnLocation.transform.up;

        bullet.Fire(fireDir, shipVelocity);
        return true;
    }
}
