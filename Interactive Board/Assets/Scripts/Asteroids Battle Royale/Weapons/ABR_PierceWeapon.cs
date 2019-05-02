using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_PierceWeapon : ABR_Weapon
{
    public ABR_PierceWeapon()
    {
        m_bulletTye = eBulletType.PIERCE.ToString();
        m_ammo = 10;
        m_fireDelay = 1.0f;
    }

    public override void Fire(Vector3 firePos, Vector3 fireDir)
    {
        ABR_PierceBullet bullet = ABR_GlobalInfo.BulletManager.GetObjectFromTaggedPool(m_bulletTye).GetComponent<ABR_PierceBullet>();
        bullet.gameObject.SetActive(true);
        bullet.gameObject.transform.position = (Vector3)firePos;

        bullet.Fire(fireDir);
    }
}