using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Bullet that will cause and explosion to appear at the point of collision 
/// that does explosion damage and pushes stuff away from the explosion
/// </summary>
public class ABR_ExplosionBullet : ABR_Bullet
{
    [Header("Explosion Bullet Configuration")]
    [Tooltip("Reference to the explosion Particle")]
    [SerializeField] GameObject m_explosionParticle = null;
    [Tooltip("Reference to the parent bullet object")]
    [SerializeField] GameObject m_bulletObject = null;
    [Tooltip("How far out the explosion will effect")]
    [SerializeField] float m_explosionRaduis = 1.5f;
    [Tooltip("How powerful the explosion pushback will be")]
    [SerializeField] float m_explosionForceModifier = 1.5f;

    //private member variables
    private bool m_isExploding = false;

    /// <summary>
    /// Override of Abstract Fire Method from base bullet class
    /// </summary>
    /// <param name="direction">The direction that this is going to fire</param>
    /// <param name="shipVelocity">the current ship velocity</param>
    public override void Fire(Vector2 direction, Vector2 shipVelocity)
    {
        m_rigidbody.velocity = (direction * m_speed) + shipVelocity;
    }

    private void Update()
    {
        m_elapsedLifeTime += Time.deltaTime;
        //if this reaches the end of its lifetime explode
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
        m_isExploding = false;
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
        m_isExploding = true;

        //get what is in the explosion radius
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, m_explosionRaduis);
        foreach (var collision in collisions)
        {
            //Get the objects health componenet
            ABR_Health health = collision.GetComponent<ABR_Health>();
            if (health)
            {
                //if it finds a health component do damage to it.
                health.TakeDamage(m_damage);
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
            if (!m_isExploding)
                Explode();
        }
    }

}
