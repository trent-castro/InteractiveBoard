using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_ExplosionEffect : MonoBehaviour
{
    [SerializeField]
    float m_lifetime = 0.25f;
    [SerializeField]
    GameObject m_siblingRendererGameObject = default;

    private ParticleSystem m_particleSystem = default;
    private ABR_ExplosionBullet m_bulletParent = default;

    private void Awake()
    {
        m_particleSystem = GetComponentInChildren<ParticleSystem>();
        m_bulletParent = GetComponentInParent<ABR_ExplosionBullet>();
        StartCoroutine(lifeTimeCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ABR_Health health = collision.GetComponent<ABR_Health>();
        if (health)
        {
            m_bulletParent.DealDamage(ref health);
        }
    }

    private void ResetExplosionEffect()
    {
        m_lifetime = 0.25f;
    }

    IEnumerator lifeTimeCoroutine()
    {
        m_particleSystem.Play();
        yield return new WaitForSeconds(m_lifetime);
        ResetExplosionEffect();
        m_bulletParent.ResetExplosionBullet();

    }
}
