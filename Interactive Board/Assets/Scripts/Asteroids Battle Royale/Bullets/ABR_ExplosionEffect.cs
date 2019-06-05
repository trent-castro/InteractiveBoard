using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Child object of an explosion bullet has a particle system to play
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class ABR_ExplosionEffect : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("How long the explosion particle will last")]
    [SerializeField]
    float m_lifetime = 0.25f;

    //sibling components
    private ParticleSystem m_particleSystem = default;
    private ABR_ExplosionBullet m_bulletParent = default;


    private void Awake()
    {
        GetSiblingComponents();
        StartCoroutine(lifeTimeCoroutine());   
    }

    private void GetSiblingComponents()
    {
        m_particleSystem = GetComponentInChildren<ParticleSystem>();
        m_bulletParent = GetComponentInParent<ABR_ExplosionBullet>();
    }

    //Coroutine that will wait for the lifetime of the object and then reset it's parent to be placed back in the pool
    IEnumerator lifeTimeCoroutine()
    {
        m_particleSystem.Play();
        yield return new WaitForSeconds(m_lifetime);
        m_bulletParent.ResetExplosionBullet();

    }
}