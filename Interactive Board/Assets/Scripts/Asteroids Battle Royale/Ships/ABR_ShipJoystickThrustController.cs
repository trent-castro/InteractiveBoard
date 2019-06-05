using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A script that allows for a different control scheme for ship maneuvering.
/// </summary>
[RequireComponent(typeof(ABR_Ship))]
public class ABR_ShipJoystickThrustController : MonoBehaviour
{
    [Header("External References")]
    [Tooltip("A reference to the joystick in the UI.")]
    [SerializeField]
    private Joystick m_input = null;
    [Tooltip("")]
    [SerializeField]
    private GameObject m_touchVisual = null;

    [Header("Configuration")]
    [Tooltip("The extent from which thrust will increase, clamped from zero to one. Anything more is maximum thrust.")]
    [SerializeField]
    private float m_thrustRadius = 40.0f;
    [Tooltip("The extent from which the ship will not thrust, only turn.")]
    [SerializeField]
    private float m_turnOnlyRadius = 20.0f;

    // Private internal data members
    /// <summary>
    /// A reference to the affected ship.
    /// </summary>
    private ABR_Ship m_ship = null;
    /// <summary>
    /// Whether or not the ship is receiving input.
    /// </summary>
    private bool started = false;

    private void OnEnable()
    {
        if (started) { StartTouch(); }
    }

    private void Awake()
    {
        m_ship = GetComponent<ABR_Ship>();
    }

    private void StartTouch()
    {
        started = true;
        m_touchVisual.transform.localPosition = Vector3.up * 5;
        m_touchVisual.transform.localRotation = Quaternion.identity;
        ToggleTouchVisual(true);
    }

    private void EndTouch()
    {
        m_ship.StopThrust();
        ToggleTouchVisual(false);
        started = false;
    }

    private void ToggleTouchVisual(bool visible)
    {
        if (m_touchVisual != null)
        {
            m_touchVisual.gameObject.SetActive(visible);
        }
    }

    private void Update()
    {
        if (m_input.PointerDown)
        {
            if (!started)
            {
                StartTouch();
            }

            Vector2 input = m_input.Axes;

            float distanceFromCenter = input.magnitude;

            m_ship.Thrust(Mathf.Clamp01((distanceFromCenter - m_turnOnlyRadius) / (m_thrustRadius - m_turnOnlyRadius)));
        }
        else
        {
            if (started)
            {
                EndTouch();
            }
        }
    }
}