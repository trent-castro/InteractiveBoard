using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that allows for a different control scheme for ship maneuvering.
/// </summary>
[RequireComponent(typeof(ABR_Ship))]
public class ABR_ShipTouchDirectionController : MonoBehaviour
{
    [Header("External References")]
    [Tooltip("A reference to the touch area that a player can use to fly the ship.")]
    [SerializeField]
    private DragPanel m_touchArea = null;
    [Tooltip("A reference to the view port of the player UI.")]
    [SerializeField]
    private Camera m_viewport = null;
    [Tooltip("A reference to the object that displays the ship forward.")]
    [SerializeField]
    private GameObject m_touchVisual = null;

    [Header("Configuration")]
    [Tooltip("A float describing the amount of delta needed in position for the ship to change direction.")]
    [SerializeField]
    private float m_tolerance = 5.0f;

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
        m_ship.StopTurnTo();
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

            if (m_touchVisual != null)
            {
                m_touchVisual.transform.position = touchPosition;
                Quaternion visualRotation = Quaternion.Euler(0, 0, transform.position.ZAngleTo(touchPosition));
                m_touchVisual.transform.rotation = visualRotation;
            }

            float turnAngle = transform.position.ZAngleTo(touchPosition);

            if (Mathf.Abs(Mathf.DeltaAngle(m_ship.TurnGoal, turnAngle)) > m_tolerance)
            {
                m_ship.TurnTo(turnAngle);
            }
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