using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class ABR_Ship : MonoBehaviour
{
    public Rigidbody2D m_RigidBody { get; private set; } = null;
    public float m_ThrustMult { get; private set; } = 1;

    private bool m_doThrust = false;
    private bool m_forceThrust = false;
    private float m_turn = 0;

    [SerializeField]
    private float m_maxSpeed = 10.0f;

    [SerializeField]
    private float m_turnPower = 240.0f;

    [SerializeField]
    private float m_turnPowerWhenThrusting = 120.0f;


    public float TurnGoal { get; private set; } = 0;
    private bool m_doTurnTo = false;
    private bool m_forceTurnTo = false;
    private float m_goalTurn = 0;
    private ABR_Turret m_turret = null;


    public Vector2 m_acceleration = Vector2.zero;
    public float m_angularAcceleration = 0;

    public void Thrust(float mult, bool force = false)
    {
        if (!m_forceThrust || force)
        {
            if (force)
            {
                m_forceThrust = true;
            }
            m_ThrustMult = mult;
            m_doThrust = true;
        }
    }

    public void StopThrust(bool force = false)
    {
        if (!m_forceThrust || force)
        {
            if (force)
            {
                m_forceThrust = false;
            }
            m_doThrust = false;
        }
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

    public void StopTurn(bool force = false)
    {
        if (!m_forceTurnTo || force)
        {
            if (force)
            {
                m_forceTurnTo = false;
            }
            m_RigidBody.angularVelocity = 0;
            m_turn = 0;
        }
    }

    public void TurnTo(float degrees, bool force = false)
    {
        if (!m_forceTurnTo || force)
        {
            if (force)
            {
                m_forceTurnTo = true;
            }
            TurnGoal = degrees;
            m_doTurnTo = true;
            m_goalTurn = 0;
        }
    }

    public void Turn(float degrees, bool force = false)
    {
        if (!m_forceTurnTo || force)
        {
            TurnTo(transform.eulerAngles.z + degrees, force);
        }
    }

    public void StopTurnTo(bool force = false)
    {
        if (!m_forceTurnTo || force)
        {
            if (force)
            {
                m_forceTurnTo = false;
            }
            m_RigidBody.angularVelocity = 0;
            m_doTurnTo = false;
        }
    }

    protected void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_turret = GetComponentInChildren<ABR_Turret>();
    }

    protected void Update()
    {
        if (!m_doTurnTo && !m_forceTurnTo)
        {
            m_RigidBody.angularVelocity = SmoothDampAngularVelocity(Mathf.Lerp(m_turnPower, m_turnPowerWhenThrusting, m_ThrustMult) * m_turn);
        }
        else
        {
            float lastGoalTurn = m_goalTurn;
            m_goalTurn = Mathf.Clamp(Mathf.DeltaAngle(transform.eulerAngles.z, TurnGoal), -1, 1);

            if (lastGoalTurn != 0 && (lastGoalTurn > 0 && m_goalTurn <= 0 || lastGoalTurn < 0 && m_goalTurn >= 0))
            {
                m_forceTurnTo = false;
                StopTurnTo();
            }
            m_RigidBody.angularVelocity = SmoothDampAngularVelocity(Mathf.Lerp(m_turnPower, m_turnPowerWhenThrusting, m_ThrustMult) * m_goalTurn);
        }

        if (m_doThrust || m_forceThrust)
        {

            m_RigidBody.velocity = SmoothDampVelocity(transform.up * m_maxSpeed * m_ThrustMult);
        }
        else
        {
            m_acceleration = -m_RigidBody.velocity * m_RigidBody.drag;
        }
    }

    private float SmoothDampAngularVelocity(float target)
    {
        return Mathf.SmoothDamp(m_RigidBody.angularVelocity, target, ref m_angularAcceleration, .1f);
    }

    private Vector2 SmoothDampVelocity(Vector2 target)
    {
        Vector2 velocityForward = Vector3.Project(m_RigidBody.velocity, transform.up);
        Vector2 accelerationForward = Vector3.Project(m_acceleration, transform.up);

        Vector2 velocitySideways = m_RigidBody.velocity - velocityForward;
        Vector2 accelerationSideways = m_acceleration - accelerationForward;

        Vector2 newVelocityForward = Vector2.SmoothDamp(velocityForward, target, ref accelerationForward, .25f, 10);
        Vector2 newVelocitySideways = Vector2.SmoothDamp(velocitySideways, target, ref accelerationSideways, .25f, 10);

        Vector2 newVelocity = newVelocitySideways;

        m_acceleration = accelerationSideways;

        if (newVelocityForward.sqrMagnitude > velocityForward.sqrMagnitude || Vector3.Dot(transform.up, velocityForward) < 0)
        {
            newVelocity += newVelocityForward;
            m_acceleration += accelerationForward;
        }
        else
        {
            newVelocity += velocityForward;
            m_acceleration += accelerationForward;
        }

        return newVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(ABR_Tags.WeaponTag))
        {
            print("Weapon pickup!");
            //TODO IMPLAMENT THIS PROPERLY!!!!

            //Get collision component of Powerup
            //Case/Switch powerup weapon type
            eBulletType weapontype = collision.gameObject.GetComponent<ABR_WeaponPickup>().m_weaponType;


            m_turret.SwitchWeapons(weapontype);
        }
    }

    public void ResetTo(Transform spawnPoint)
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        //Reset Health

        if (m_turret != null)
        {
            m_turret.SwitchWeapons(eBulletType.BASIC);
        }
    }
}
