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

    private void OnTriggerEnter2D(Collider2D collision)
    {

        m_numOfPierced++;
        if(m_numOfPierced >= 3)
        {
            gameObject.SetActive(false);
        }
    }

}
