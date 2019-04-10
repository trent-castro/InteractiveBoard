using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_Puck : MonoBehaviour
{
    [SerializeField]
    float m_maxSpeed;
    [SerializeField]
    GameObject m_hitParticle;

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

        Debug.DrawLine(rgdbody.position, rgdbody.position + rgdbody.velocity, Color.black);

    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        AH_Paddle paddle = collision.gameObject.GetComponent<AH_Paddle>();
        if (paddle != null)
        {
            Vector2 normal = collision.GetContact(0).normal;
            Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + normal, Color.yellow, 2);
            Vector2 relativeVelocity = rgdbody.velocity - paddle.m_Velocity;
            Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + relativeVelocity, Color.blue, 2);

            Vector2 reflected = Vector2.Reflect(relativeVelocity, normal);
            Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + reflected, Color.cyan, 2);

            rgdbody.velocity = reflected + paddle.m_Velocity;
            Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + relativeVelocity, Color.magenta, 2);
        }
        else
        {

        }
        m_hitParticle.transform.position = collision.contacts[0].point;
    }
}
