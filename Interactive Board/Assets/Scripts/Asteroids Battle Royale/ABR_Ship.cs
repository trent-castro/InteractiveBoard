using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class ABR_Ship : MonoBehaviour
{
    public Rigidbody2D m_rigidBody = null;

    private float m_thrustMult = 1;
    private bool m_doThrust = false;
    private float m_turn = 0;

    [SerializeField]
    private float m_maxSpeed = 10.0f;

    [SerializeField]
    private float m_turnPower = 240.0f;

    [SerializeField]
    private float m_turnPowerWhenThrusting = 120.0f;
    public float TurnGoal { get; private set; } = 0;
    private bool m_doTurnTo = false;
    private float m_goalTurn = 0;

    public Vector2 m_acceleration = Vector2.zero;
    public float m_angularAcceleration = 0;

    public void Thrust(float mult)
    {
        m_thrustMult = mult;
        m_doThrust = true;
    }

    public void StopThrust()
    {
        m_doThrust = false;
    }

    public void SetMaxSpeed(float power)
    {
        m_maxSpeed = Mathf.Max(0, power);
    }

    public void TurnClockwise()
    {
        m_turn = Mathf.Clamp(--m_turn, -1, 1);
    }

    public void TurnCounterClockWise()
    {
        m_turn = Mathf.Clamp(++m_turn, -1, 1);
    }

    public void StopTurn()
    {
        m_rigidBody.angularVelocity = 0;
        m_turn = 0;
    }

    public void TurnTo(float degrees)
    {
        TurnGoal = degrees;
        m_doTurnTo = true;
        m_goalTurn = 0;
    }

    public void Turn(float degrees)
    {
        TurnGoal = transform.eulerAngles.z + degrees;
        m_doTurnTo = true;
        m_goalTurn = 0;
    }

    public void StopTurnTo()
    {
        m_rigidBody.angularVelocity = 0;
        m_doTurnTo = false;
    }

    protected void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    protected void Update()
    {
        if (!m_doTurnTo)
        {
            m_rigidBody.angularVelocity = SmoothDampAngularVelocity((m_doThrust ? m_turnPowerWhenThrusting : m_turnPower) * m_turn);
        }
        else
        {
            float lastGoalTurn = m_goalTurn;
            m_goalTurn = Mathf.Clamp(Mathf.DeltaAngle(transform.eulerAngles.z, TurnGoal), -1, 1);

            if (lastGoalTurn != 0 && (lastGoalTurn > 0 && m_goalTurn <= 0 || lastGoalTurn < 0 && m_goalTurn >= 0))
            {
                StopTurnTo();
            }
            m_rigidBody.angularVelocity = SmoothDampAngularVelocity(m_turnPower * m_goalTurn / (m_doThrust ? 2 : 1));
        }

        if (m_doThrust)
        {
            m_rigidBody.velocity = Vector2.SmoothDamp(m_rigidBody.velocity, transform.up * m_maxSpeed * m_thrustMult, ref m_acceleration, .25f);
        }
        else
        {
            m_acceleration = -m_rigidBody.velocity * m_rigidBody.drag;
        }
    }

    private float SmoothDampAngularVelocity(float target)
    {
        return Mathf.SmoothDamp(m_rigidBody.angularVelocity, target, ref m_angularAcceleration, .1f);
    }
}
