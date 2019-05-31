using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_ExplosionBullet : ABR_Bullet
{
    [Header("Explosion Bullet Configuration")]
    [SerializeField] GameObject m_explosionObjects = null;
    [SerializeField] GameObject m_bulletObject = null;
    [SerializeField] float m_explosionRaduis = 1.5f;
    [SerializeField] float m_explosionForceModifier = 1.5f;

    private bool isExploding = false;
    public override void Fire(Vector2 direction, Vector2 shipVelocity)
    {
        m_rigidbody.velocity = (direction * m_speed) + shipVelocity;
    }

    private void Update()
    {
        m_elapsedLifeTime += Time.deltaTime;
        if (m_elapsedLifeTime >= m_lifeTime)
        {
            Explode();
        }
    }

    public void ResetExplosionBullet()
    {
        m_bulletObject.SetActive(true);
        m_explosionObjects.SetActive(false);
        isExploding = false;
        gameObject.SetActive(false);
    }

    private void Explode()
    {
        //Enable Explosion collider
        m_rigidbody.velocity = Vector3.zero;
        m_bulletObject.SetActive(false);
        m_explosionObjects.SetActive(true);
        isExploding = true;

        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, m_explosionRaduis);
        foreach (var collision in collisions)
        {
            ABR_Health health = collision.GetComponent<ABR_Health>();
            if (health)
            {
                DealDamage(ref health);
            }
            Rigidbody2D rigidbody = collision.GetComponent<Rigidbody2D>();
            if (rigidbody)
            {
                Vector2 directionVec = (collision.transform.position - transform.position).normalized;
                rigidbody.AddForce(directionVec * m_explosionForceModifier, ForceMode2D.Impulse);
            }
        }
        
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(ABR_Tags.ShipCollisionTag) && !collision.CompareTag(ABR_Tags.WallTag)
            && !collision.CompareTag(ABR_Tags.GravityTag) && !collision.CompareTag(ABR_Tags.WeaponTag))
        {
            if (!isExploding)
                Explode();
        }
    }

}
