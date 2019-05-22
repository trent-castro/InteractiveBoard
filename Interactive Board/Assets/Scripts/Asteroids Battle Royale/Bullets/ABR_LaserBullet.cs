using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_LaserBullet : ABR_Bullet
{
    private bool isDealingDamage = false;
    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(ABR_Tags.BulletTag) && !collision.gameObject.CompareTag(ABR_Tags.ShipCollisionTag) && !collision.CompareTag(ABR_Tags.WallTag) && !collision.CompareTag(ABR_Tags.GravityTag))
        {
            isDealingDamage = true;
            ABR_Health healthComponent = collision.GetComponent<ABR_Health>();
            if (healthComponent)
                StartCoroutine(DamageCoroutine(healthComponent));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isDealingDamage = false;
    }
    public override void Fire(Vector2 direction, Vector2 shipVelocity)
    {
        Vector3 directionConversion = Vector3.zero;
        directionConversion.z = direction.x;
        m_rigidbody.velocity = (direction * m_speed) + shipVelocity;

    }

    private IEnumerator DamageCoroutine(ABR_Health health)
    {   
        while (isDealingDamage)
        {
            DealDamage(ref health);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
