using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eBulletType
{
    BASIC,
    SHOTGUN,
    EXPLOSION,
    PIERCE,
    LASER
}

public abstract class Weapon
{
    protected float m_fireDelay;
    protected string m_bulletTye;
    protected uint m_ammo;
    abstract public void Fire(Vector3 firePos, Vector3 fireDir);
}

public class BasicWeapon : Weapon
{
    public BasicWeapon()
    {
        m_bulletTye = eBulletType.BASIC.ToString();
    }
    public override void Fire(Vector3 firePos, Vector3 fireDir)
    {
        ABR_Bullet bullet = ABR_GlobalInfo.BulletManager.GetObjectFromTaggedPool(m_bulletTye).GetComponent<ABR_Bullet>();
        bullet.gameObject.SetActive(true);
        bullet.gameObject.transform.position = (Vector3)firePos;

        bullet.Fire(fireDir);
    }
}


public class Shotgun : Weapon
{
    private uint m_numOfBullets = 9;
    private float m_shotAngle = 60;


    public Shotgun()
    {
        m_bulletTye = eBulletType.SHOTGUN.ToString();
        m_ammo = 10;
    }

    public override void Fire(Vector3 firePos, Vector3 fireDir)
    {
        if (m_ammo > 0)
        {
            Vector3 baseDir = fireDir;
            for (int i = 0; i < m_numOfBullets; i++)
            {
                ABR_Bullet bullet = ABR_GlobalInfo.BulletManager.GetObjectFromTaggedPool(m_bulletTye).GetComponent<ABR_Bullet>();
                bullet.gameObject.SetActive(true);
                bullet.gameObject.transform.position = firePos;
                float randomNum = Random.Range(-30, 30);
                fireDir = Quaternion.AngleAxis(randomNum, Vector3.back) * fireDir;
                //TODO Fire in random directions within a 60 degree angle
                bullet.Fire(fireDir);
            }
            m_ammo--;
        }
    }
}

public class Laser : Weapon
{
    public Laser()
    {
        m_bulletTye = eBulletType.LASER.ToString();
        m_ammo = 15;
    }

    public override void Fire(Vector3 firePos, Vector3 fireDir)
    {
    }
}

public class Explosion : Weapon
{
    public Explosion()
    {
        m_bulletTye = eBulletType.EXPLOSION.ToString();
        m_ammo = 5;
    }

    public override void Fire(Vector3 firePos, Vector3 fireDir)
    {
        throw new System.NotImplementedException();
    }
}

public class Pierce : Weapon
{
    public Pierce()
    {
        m_bulletTye = eBulletType.PIERCE.ToString();
        m_ammo = 10;
    }

    public override void Fire(Vector3 firePos, Vector3 fireDir)
    {
        throw new System.NotImplementedException();
    }
}
