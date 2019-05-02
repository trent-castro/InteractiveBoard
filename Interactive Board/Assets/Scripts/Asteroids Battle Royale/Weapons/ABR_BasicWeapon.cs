using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ABR_BasicWeapon : ABR_Weapon
{
    public ABR_BasicWeapon()
    {
        m_bulletTye = eBulletType.BASIC.ToString();
        m_fireDelay = 0.5f;
    }
    public override void Fire(Vector3 firePos, Vector3 fireDir)
    {
        ABR_Bullet bullet = ABR_GlobalInfo.BulletManager.GetObjectFromTaggedPool(m_bulletTye).GetComponent<ABR_Bullet>();
        bullet.gameObject.SetActive(true);
        bullet.gameObject.transform.position = (Vector3)firePos;

        bullet.Fire(fireDir);
    }
}