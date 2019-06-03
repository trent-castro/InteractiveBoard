using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_ExplosionBullet : ABR_Bullet
{
    [Header("Explosion Bullet Configuration")]
    [SerializeField] GameObject m_explosionParticle = null;
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

    /// <summary>
    /// Resets the Explosion Bullet to it's default state
    /// </summary>
    public void ResetExplosionBullet()
    {
        m_bulletObject.SetActive(true);
        m_explosionParticle.SetActive(false);
        isExploding = false;
        gameObject.SetActive(false);
    }

    private void Explode()
    {
        //Stop the explosion from moving
        m_rigidbody.velocity = Vector3.zero;
        //disable the bullet renderer
        m_bulletObject.SetActive(false);
        //enable the explosion particle
        m_explosionParticle.SetActive(true);
        //set isExploding to true
        isExploding = true;

        //get what is in the explosion radius
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, m_explosionRaduis);
        foreach (var collision in collisions)
        {
            //Get the objects health componenet
            ABR_Health health = collision.GetComponent<ABR_Health>();
            if (health)
            {
                //if it finds a health component do damage to it.
                DealDamage(ref health);
            }
            //Get the ojects rigidbodies
            Rigidbody2D rigidbody = collision.GetComponent<Rigidbody2D>();
            if (rigidbody)
            {
                //if it finds rigidbodies, pushes them away from the explosion
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
            //if the bullet collided with something not listed above and is not already exploding, it explodes
            if (!isExploding)
                Explode();
        }
    }

}
