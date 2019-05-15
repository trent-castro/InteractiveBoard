using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_LaserBullet : ABR_Bullet
{
    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(ABR_Tags.WallTag) && !collision.CompareTag(ABR_Tags.ShipCollisionTag))
        {
            //Do lots o' damage
        }
		base.OnTriggerEnter2D(collision);
	}
	public override void Fire(Vector2 direction, Vector2 shipVelocity)
    {
        m_rigidbody.velocity = (direction * m_speed) + shipVelocity;
    }
}
