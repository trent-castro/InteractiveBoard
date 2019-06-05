using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A script that has the properties of a player.
/// Component that gives an object the properties of a ship
/// - can fly
/// - can fight
/// - can crow
[RequireComponent(typeof(Rigidbody2D))]
public class ABR_Ship : MonoBehaviour
{

    [Header("Configuration")]
    [Tooltip("The value that the vertical thrust cap will be multiplied by")]
    [SerializeField]
    private float m_verticalThrustCapMult = 10.0f;
    [Tooltip("The value that the horizontal thrust cap will be multiplied by")]
    [SerializeField]
    private float m_horizontalThrustCapMult = 5.0f;
    [Tooltip("how much time it will take to smooth thrust")]
    [SerializeField]
    private float m_thrustSmoothTime = 0.25f;
    [Tooltip("how much time it will take to smooth turn")]
    [SerializeField]
    private float m_turnSmoothTime = 0.1f;
    [Tooltip("Maximum speed of the ship")]
    [SerializeField]
    private float m_maxSpeed = 10.0f;
    [Tooltip("How fast you can turn the ship")]
    [SerializeField]
    private float m_turnPower = 240.0f;
    [Tooltip("How fas you can turn the ship when you are moving")]
    [SerializeField]
    private float m_turnPowerWhenThrusting = 120.0f;

    //Properties
    public Rigidbody2D m_RigidBody { get; private set; } = null;
    public float m_ThrustMult { get; private set; } = 1;
    public float TurnGoal { get; private set; } = 0;

    //private member variables
    private bool m_forceThrust = false;
    private float m_turn = 0;
    private bool m_doTurnTo = false;
    /// <summary>
    /// Forces the player to turn if they exit the play area.
    /// </summary>
    private bool m_forceThrust = false;
    private float m_turn = 0;
    private bool m_forceTurnTo = false;
    private float m_goalTurn = 0;
    private ABR_Turret m_turret = null;

    //public member variables
    public Vector2 m_acceleration = Vector2.zero;
    public float m_angularAcceleration = 0;
	public OnWeaponPickUp m_weaponPickupEvent = null;

    //Delegate
	public delegate void OnWeaponPickUp();


	public string GetWeaponType()
	{
		return m_turret.GetWeaponType();
	}
    /// <summary>
    /// Method to move the ship forward
    /// </summary>
    /// <param name="mult">From 0 to max thrust</param>
    /// <param name="force">Determines if you are overriding any other controls</param>
    public void Thrust(float mult, bool force = false)
    {
        if (!m_forceThrust || force)
        {
            if (force)
            {
                m_forceThrust = true;
            }
            m_ThrustMult = mult;
        }
    }
    /// <summary>
    /// Stops the ship from moving forward
    /// </summary>
    /// <param name="force">Determines if you are overriding any other controls</param>
    public void StopThrust(bool force = false)
    {
        if (!m_forceThrust || force)
        {
            if (force)
            {
                m_forceThrust = false;
            }
            m_ThrustMult = 0;
        }
    }
    /// <summary>
    /// setting of the maxspeed variable
    /// </summary>
    /// <param name="power">if this is greater than 0 this will be the new max speed</param>
    public void SetMaxSpeed(float power)
    {
        m_maxSpeed = Mathf.Max(0, power);
    }
    /// <summary>
    /// Rotate the Ship Clockwise
    /// </summary>
    public void TurnClockwise()
    {
        if (!m_forceTurnTo)
        {
            m_turn = Mathf.Clamp(--m_turn, -1, 1);
        }
    }
    /// <summary>
    /// Rotate the ship Counterclockwise
    /// </summary>
    public void TurnCounterClockWise()
    {
        if (!m_forceTurnTo)
        {
            m_turn = Mathf.Clamp(++m_turn, -1, 1);
        }
    }
    /// <summary>
    /// stops the ship from turning and reset the turn values
    /// </summary>
    /// <param name="force">Determines if you are overriding any other controls</param>
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
    /// <summary>
    /// Turns the ship to the degree rotation within a unit circle
    /// </summary>
    /// <param name="degrees">What angle the ship will turn to</param>
    /// <param name="force">Determines if you are overriding any other controls</param>
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
            m_turn = 0;
        }
    }
    /// <summary>
    /// turns the ship to the degree rotation within a unit circle relative to the ships rotation
    /// </summary>
    /// <param name="degrees">what angle ths ship will turn to</param>
    /// <param name="force">Determines if you are overriding any other controls</param>
    public void Turn(float degrees, bool force = false)
    {
        if (!m_forceTurnTo || force)
        {
            TurnTo(transform.eulerAngles.z + degrees, force);
        }
    }
    /// <summary>
    /// Stops the ship from turning towards anything and resets respecitve values
    /// </summary>
    /// <param name="force">Determines if you are overriding any other controls</param>
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
            m_turn = 0;
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
            //create a new float of the last goal turn velocity
            float lastGoalTurn = m_goalTurn;
            m_goalTurn = Mathf.Clamp(Mathf.DeltaAngle(transform.eulerAngles.z, TurnGoal), -1, 1);

            if (lastGoalTurn != 0 && (lastGoalTurn > 0 && m_goalTurn <= 0 || lastGoalTurn < 0 && m_goalTurn >= 0))
            {
                //if there is not sufficiant turn data stop turning
                m_forceTurnTo = false;
                StopTurnTo();
            }
            //set the rigidbody angular velocity to the smooth damp angular Velocity
            m_RigidBody.angularVelocity = SmoothDampAngularVelocity(Mathf.Lerp(m_turnPower, m_turnPowerWhenThrusting, m_ThrustMult) * m_goalTurn);
        }

        if (m_ThrustMult != 0 || m_forceThrust)
        {
            //sets the current velocity to the smoothdamped velocity
            m_RigidBody.velocity = SmoothDampVelocity(transform.up * m_maxSpeed * m_ThrustMult);
        }
        else
        {
            //decelerate the ship by negative velocity and the rigidbody drag
            m_acceleration = -m_RigidBody.velocity * m_RigidBody.drag;
        }
    }
    /// <summary>
    /// determines the smoothdamp of angular velocity
    /// </summary>
    /// <param name="target">the target velocity magnitude</param>
    /// <returns></returns>
    private float SmoothDampAngularVelocity(float target)
    {
        return Mathf.SmoothDamp(m_RigidBody.angularVelocity, target, ref m_angularAcceleration, m_turnSmoothTime);
    }
    /// <summary>
    /// Smooths the dampening Veloicity
    /// </summary>
    /// <param name="target">The target position</param>
    /// <returns>the smothed velocity</returns>
    private Vector2 SmoothDampVelocity(Vector2 target)
    {
        Vector2 velocityForward = Vector3.Project(m_RigidBody.velocity, transform.up);
        Vector2 accelerationForward = Vector3.Project(m_acceleration, transform.up);

        Vector2 velocitySideways = m_RigidBody.velocity - velocityForward;
        Vector2 accelerationSideways = m_acceleration - accelerationForward;

        Vector2 newVelocityForward = Vector2.SmoothDamp(velocityForward, target, ref accelerationForward, m_thrustSmoothTime, m_verticalThrustCapMult * m_ThrustMult);
        Vector2 newVelocitySideways = Vector2.SmoothDamp(velocitySideways, target, ref accelerationSideways, m_thrustSmoothTime, m_horizontalThrustCapMult * m_ThrustMult);

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
            eBulletType weapontype = collision.gameObject.GetComponent<ABR_WeaponPickup>().m_weaponType;
            m_turret.SwitchWeapons(weapontype);
			m_weaponPickupEvent.Invoke();
        }
    }
    /// <summary>
    /// Resets the ship to a spawn transform
    /// </summary>
    /// <param name="spawnPoint"></param>
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