using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ABR_Ship))]
public class ABR_ShipTouchThrustController : MonoBehaviour
{
    [SerializeField]
    private DragPanel m_touchArea = null;

    [SerializeField]
    private Camera m_viewport = null;

    [SerializeField]
    private float m_thrustRadius = 5f;
    [SerializeField]
    private float m_turnOnlyRadius = 2.5f;

    [SerializeField]
    private GameObject m_touchVisual = null;

    private ABR_Ship m_ship = null;

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
