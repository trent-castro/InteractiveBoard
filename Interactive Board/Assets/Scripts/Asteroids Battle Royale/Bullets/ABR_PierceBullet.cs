using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_PierceBullet : ABR_Bullet
{
    private int m_numOfPierced = 0;
    public override void Fire(Vector2 direction, Vector2 shipVelocity)
    {
        m_rigidbody.velocity = (direction * m_speed) + shipVelocity;
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(ABR_Tags.BulletTag) && !collision.gameObject.CompareTag(ABR_Tags.ShipCollisionTag) 
            && !collision.CompareTag(ABR_Tags.WallTag) && !collision.CompareTag(ABR_Tags.GravityTag) && !collision.CompareTag(ABR_Tags.WeaponTag))
        {
            ABR_Health health = collision.GetComponent<ABR_Health>();
            if (health)
            {
                DealDamage(ref health);
            }
            m_numOfPierced++;
            if (m_numOfPierced >= 3)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private new void DealDamage(ref ABR_Health health)
    {
        int modifiedDamage = (int)m_damage;
            modifiedDamage = modifiedDamage / (m_numOfPierced + 1);
        health.TakeDamage(modifiedDamage);
    }
}
