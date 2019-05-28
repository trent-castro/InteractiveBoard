using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ABR_Ship))]
public class ABR_ShipTouchDirectionController : MonoBehaviour
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
    private float m_tolerance = 5f;

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
