using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_ExplosionBullet : ABR_Bullet
{
    public override void Fire(Vector2 direction, Vector2 shipVelocity)
    {
        m_rigidbody.velocity = (direction * m_speed) + shipVelocity;
    }
}
