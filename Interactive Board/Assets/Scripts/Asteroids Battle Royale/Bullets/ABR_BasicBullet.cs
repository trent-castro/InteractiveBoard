using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_BasicBullet : ABR_Bullet
{

    public override void Fire(Vector2 direction, Vector2 shipVelocity)
    {
        m_rigidbody.velocity = (direction * m_speed) + shipVelocity;
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if(!collision.CompareTag(ABR_Tags.WallTag) && ! collision.CompareTag(ABR_Tags.ShipCollisionTag))
        {
            Debug.Log("Collided with something else");
        }
		base.OnTriggerEnter2D(collision);
    }
}
