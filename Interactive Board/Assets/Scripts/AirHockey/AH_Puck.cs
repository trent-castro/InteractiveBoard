using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_Puck : MonoBehaviour
{
    [SerializeField]
    float m_maxSpeed = 30;
    [SerializeField]
    GameObject[] m_hitParticle = null;

    private Rigidbody2D rgdbody;

    private void Start()
    {
        rgdbody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //This clamps the Puck Speed to 30
        if (rgdbody.velocity != Vector2.zero)
        {
            float magnitude = rgdbody.velocity.magnitude;
            if (magnitude > m_maxSpeed)
            {
                rgdbody.velocity = (rgdbody.velocity / magnitude) * m_maxSpeed;

            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_hitParticle.transform.position = collision.contacts[0].point;
        m_hitParticle.GetComponent<ParticleSystem>().Play();
    }
}
