using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_Puck : MonoBehaviour
{
    [SerializeField]
    float m_maxSpeed;
    [SerializeField]
    GameObject[] m_hitParticle;

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

        //Debug.DrawLine(rgdbody.position, rgdbody.position + rgdbody.velocity, Color.black);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AH_Paddle paddle = collision.gameObject.GetComponent<AH_Paddle>();
        if (paddle != null)
        {
            Vector2 normal = collision.GetContact(0).normal.normalized;
            //Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + normal, Color.yellow, 2);

            Vector2 paddleVelocityProjection = Vector2.Dot(paddle.m_Velocity, normal) * normal;
            //Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + paddleVelocityProjection, Color.blue, 2);


            rgdbody.velocity = rgdbody.velocity + paddleVelocityProjection;
            //Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + rgdbody.velocity, Color.magenta, 2);
        }
        else
        {

        }
        m_hitParticle[0].transform.position = collision.contacts[0].point;
        m_hitParticle[0].GetComponent<ParticleSystem>().Play();
    }

    public GameObject GetHitParticle(int index)
    {
        return m_hitParticle[index];
    }
}
