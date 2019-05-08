using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_ExplosionEffect : MonoBehaviour
{
    [SerializeField]
    float m_lifetime = 0.25f;
    
    private void Update()
    {
        m_lifetime -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponentInParent<ABR_ExplosionBullet>().ResetExplosionBullet();   
    }

    private void ResetExplosionEffect()
    {
        m_lifetime = 0.25f;
    }
}
