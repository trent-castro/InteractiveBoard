using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bullet that does damage continually as it is still colliding with an object
/// </summary>
public class ABR_LaserBullet : ABR_Bullet
{
    //private member variables
    private bool isDealingDamage = false;

    /// <summary>
    /// Override of Abstract Fire Method from base bullet class
    /// </summary>
    /// <param name="direction">The direction that this is going to fire</param>
    /// <param name="shipVelocity">the current ship velocity</param>
    public override void Fire(Vector2 direction, Vector2 shipVelocity)
    {
        m_rigidbody.velocity = (direction * m_speed) + shipVelocity;
    }

    //when something enters this trigger deal continual damage to it
    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(ABR_Tags.BulletTag) && !collision.gameObject.CompareTag(ABR_Tags.ShipCollisionTag)
            && !collision.CompareTag(ABR_Tags.WallTag) && !collision.CompareTag(ABR_Tags.GravityTag) && !collision.CompareTag(ABR_Tags.WeaponTag))
        {
            //set dealing damage boolean to true
            isDealingDamage = true;
            //get the health component to be damaged
            ABR_Health healthComponent = collision.GetComponent<ABR_Health>();
            //if the component exists start the damage coroutine
            if (healthComponent)
                StartCoroutine(DamageCoroutine(healthComponent));
        }
    }

    //stop doing damage to the object
    private void OnTriggerExit2D(Collider2D collision)
    {
        //set the dealing damage boolean to false
        isDealingDamage = false;
    }

    /// <summary>
    /// Coroutine that deals damage every .01 secods
    /// </summary>
    /// <param name="health">refference to a health component to deal damage</param>
    /// <returns></returns>
    private IEnumerator DamageCoroutine(ABR_Health health)
    {   
        while (isDealingDamage)
        {
            //deal damage to the health component
            health.TakeDamage(m_damage);
            //wait for .01 seconds before looping again
            yield return new WaitForSeconds(0.01f);
        }
    }
}