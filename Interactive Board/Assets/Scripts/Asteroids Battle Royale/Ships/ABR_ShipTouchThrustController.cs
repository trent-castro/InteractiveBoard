using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A script that allows for a different control scheme for ship maneuvering.
/// </summary>
[RequireComponent(typeof(ABR_Ship))]
public class ABR_ShipTouchThrustController : MonoBehaviour
{
    [Header("External References")]
    [Tooltip("A reference to the touch area in which the player can manipulate the ship.")]
    [SerializeField]
    private DragPanel m_touchArea = null;
    [Tooltip("A reference to the view port of the player UI.")]
    [SerializeField]
    private Camera m_viewport = null;
    [Tooltip("A reference to the object that displays the ship forward.")]
    [SerializeField]
    private GameObject m_touchVisual = null;

    [Header("Configuration")]
    [Tooltip("The extent from which thrust will increase, clamped from zero to one. Anything more is maximum thrust.")]
    [SerializeField]
    private float m_thrustRadius = 5f;
    [Tooltip("The extent from which the ship will not thrust, only turn.")]
    [SerializeField]
    private float m_turnOnlyRadius = 2.5f;

    // Private internal data members
    /// <summary>
    /// A reference to the ship that is being affected.
    /// </summary>
    private ABR_Ship m_ship = null;
    /// <summary>
    /// Whether or not the ship has received a command to move or not.
    /// </summary>
    private bool started = false;

    private void Awake()
    {
        m_ship = GetComponent<ABR_Ship>();
    }

    private void StartTouch()
    {
        started = true;
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
        if (m_touchArea.PointerDown)
        {
            if (!started)
            {
                StartTouch();
            }

            Vector2 touchPosition = m_viewport.ScreenToWorldPoint(m_touchArea.TouchPosition);

            float distanceFromCenter = (touchPosition - (Vector2)transform.position).magnitude;

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