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
        if (!collision.gameObject.CompareTag(ABR_Tags.BulletTag) && !collision.gameObject.CompareTag(ABR_Tags.ShipCollisionTag) && !collision.CompareTag(ABR_Tags.WallTag) && !collision.CompareTag(ABR_Tags.GravityTag))
        {
            ABR_Health health = collision.GetComponent<ABR_Health>();
            if (health)
            {
                health.TakeDamage(m_damage);
            }
            m_numOfPierced++;
            if (m_numOfPierced >= 3)
            {
                gameObject.SetActive(false);
            }
        }
	}
}
