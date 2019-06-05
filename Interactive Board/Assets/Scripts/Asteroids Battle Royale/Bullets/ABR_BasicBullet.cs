using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bullet fired by the basic weapon.
/// </summary>
public class ABR_BasicBullet : ABR_Bullet
{
    /// <summary>
    /// Override of Abstract Fire Method from base bullet class
    /// </summary>
    /// <param name="direction">The direction that this is going to fire</param>
    /// <param name="shipVelocity">the current ship velocity</param>
    public override void Fire(Vector2 direction, Vector2 shipVelocity)
    {
        //set rigidbody velocity to the direction time speed plus ship velocity to avoid self collision
        m_rigidbody.velocity = (direction * m_speed) + shipVelocity;
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        //calls the base OnTriggerEnter2D
		base.OnTriggerEnter2D(collision);
    }
}