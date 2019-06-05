using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bullet that goes through multiple game objects.
/// </summary>
public class ABR_PierceBullet : ABR_Bullet
{
    [Header("Pierce Bullet Configuration")]
    [Tooltip("Maximum number of objects this bullet can pass through")]
    [SerializeField] int m_maxNumOfObjectsToBePierced = 3;
    private int m_numOfPierced = 0;

    /// <summary>
    /// Override of Abstract Fire Method from base bullet class
    /// </summary>
    /// <param name="direction">The direction that this is going to fire</param>
    /// <param name="shipVelocity">the current ship velocity</param>
    public override void Fire(Vector2 direction, Vector2 shipVelocity)
    {
        m_rigidbody.velocity = (direction * m_speed) + shipVelocity;
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(ABR_Tags.BulletTag) && !collision.gameObject.CompareTag(ABR_Tags.ShipCollisionTag)
            && !collision.CompareTag(ABR_Tags.WallTag) && !collision.CompareTag(ABR_Tags.GravityTag) && !collision.CompareTag(ABR_Tags.WeaponTag))
        {
            //attempt to grab health component from the collision object
            ABR_Health health = collision.GetComponent<ABR_Health>();
            //if the compenent is found continue
            if (health)
            {
                int modifiedDamage = (int)m_damage;
                //modified damage is set to itself divided by number of pierced objects
                modifiedDamage = modifiedDamage / (m_numOfPierced + 1);
                //deal the modified damage
                health.TakeDamage(modifiedDamage);
            }
            //increment numOfPierced
            m_numOfPierced++;
            //if the nubmer of pierced is greater than or equal to the maximum
            if (m_numOfPierced >= m_maxNumOfObjectsToBePierced)
            {
                //set this object to not active
                gameObject.SetActive(false);
            }
        }
    }

}
