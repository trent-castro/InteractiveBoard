using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_BasicBullet : ABR_Bullet
{

    public override void Fire(Vector2 direction, Vector2 shipVelocity)
    {
        m_rigidbody.velocity = (direction * m_speed) + shipVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Wall") && ! collision.CompareTag("TouchArea"))
        {
        }
    }
}
