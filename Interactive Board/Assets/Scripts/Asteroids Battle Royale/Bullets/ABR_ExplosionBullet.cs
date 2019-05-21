using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_ExplosionBullet : ABR_Bullet
{
    [Header("Explosion Bullet Configuration")]
    [SerializeField]
    GameObject m_explosionObjects = null;
    [SerializeField]
    GameObject m_bulletObject = null;

    public float m_explosionTime;


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
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(ABR_Tags.BulletTag) && !collision.gameObject.CompareTag(ABR_Tags.ShipCollisionTag) && !collision.CompareTag(ABR_Tags.WallTag) && !collision.CompareTag(ABR_Tags.GravityTag))
        {
            if (!isExploding)
                Explode();
        }
	}

}
