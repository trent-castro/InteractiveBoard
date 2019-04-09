using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_Puck : MonoBehaviour
{
    [SerializeField]
    float m_maxSpeed;

    private Rigidbody2D rgdbody;

    private void Start()
    {
        rgdbody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //This clamps the Puck Speed to 30
        if(rgdbody.velocity != Vector2.zero)
        {
            float magnitude = rgdbody.velocity.magnitude;
            if (magnitude > m_maxSpeed)
            {
                rgdbody.velocity = (rgdbody.velocity / magnitude) * m_maxSpeed;

            }
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Paddle paddle = collision.gameObject.GetComponent<Paddle>();
    //    if (paddle != null)
    //    {
    //        Vector2 normal = collision.GetContact(0).normal;
    //        Vector2 paddleVelocity = paddle.m_velocity;

    //        Vector2 reflected = Vector2.Reflect(paddleVelocity, normal);

    //        rgdbody.velocity = rgdbody.velocity - reflected;
    //    }
    //}
}
