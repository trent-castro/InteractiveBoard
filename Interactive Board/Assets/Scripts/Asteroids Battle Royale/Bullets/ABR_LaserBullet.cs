﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_LaserBullet : ABR_Bullet
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (!collision.CompareTag("Wall") && !collision.CompareTag("TouchArea"))
        {
            Debug.Log("Collided with something else");
        }
    }
    public override void Fire(Vector2 direction, Vector2 shipVelocity)
    {
        m_rigidbody.velocity = (direction * m_speed) + shipVelocity;
    }
}
