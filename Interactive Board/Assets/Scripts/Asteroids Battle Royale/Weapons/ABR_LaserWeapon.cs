//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ABR_LaserWeapon : ABR_Weapon
//{
//    public ABR_LaserWeapon()
//    {
//        m_bulletTye = eBulletType.LASER.ToString();
//        m_ammo = 15;
//        m_fireDelay = 1.0f;
//    }

//    public override void Fire(Vector3 shipVelocity)
//    {
//        ABR_LaserBullet bullet = ABR_GlobalInfo.BulletManager.GetObjectFromTaggedPool(m_bulletTye).GetComponent<ABR_LaserBullet>();
//        bullet.gameObject.SetActive(true);
//        bullet.gameObject.transform.position = m_BulletSpawnPosition.position;

//        bullet.Fire(Vector2.zero, shipVelocity);
//    }
//}
