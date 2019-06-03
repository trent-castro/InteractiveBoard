using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Bullet fired by the basic weapon, 
/// </summary>
public class ABR_BasicBullet : ABR_Bullet
{
    /// <summary>
    /// Override of Abstract Fire Function
    /// </summary>
    /// <param name="direction">The direction on which the bullet will fire</param>
    /// <param name="shipVelocity">The Velocity of the ship</param>
    public override void Fire(Vector2 direction, Vector2 shipVelocity)
    {
        m_rigidbody.velocity = (direction * m_speed) + shipVelocity;
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
		base.OnTriggerEnter2D(collision);
    }
}
