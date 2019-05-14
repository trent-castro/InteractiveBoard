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
        Debug.Log(collision);
        if (!collision.CompareTag(ABR_Tags.WallTag) && !collision.CompareTag(ABR_Tags.ShipCollisionTag))
        {
            Debug.Log("Collided with something else");
            m_numOfPierced++;
            if (m_numOfPierced >= 3)
            {
                gameObject.SetActive(false);
            }
        }
		base.OnTriggerEnter2D(collision);
	}

}
