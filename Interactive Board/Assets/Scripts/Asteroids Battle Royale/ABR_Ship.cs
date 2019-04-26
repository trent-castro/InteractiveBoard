using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ABR_Ship : MonoBehaviour
{
    private Rigidbody2D m_rigidBody = null;

    private float m_thrustMult = 1;
    private bool m_doThrust = false;
    private int m_turn = 0;

    private float m_thrustPower = 10.0f;
    private float m_turnPower = 240.0f;

    private Vector2 m_acceleration = Vector2.zero;

    public float ThrustMult { get { return m_thrustMult; } set { m_thrustMult = value; } }
    public bool DoThrust { get { return m_doThrust; } set { m_doThrust = value; } }
    public int Turn { get { return m_turn; } set { m_turn = Mathf.Clamp(value, -1, 1); } }


    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        m_rigidBody.angularVelocity = m_turnPower * m_turn;

        if (m_doThrust)
        {
            m_rigidBody.velocity = Vector2.SmoothDamp(m_rigidBody.velocity, transform.up * m_thrustPower * m_thrustMult, ref m_acceleration, .25f);
        }
    }
}
