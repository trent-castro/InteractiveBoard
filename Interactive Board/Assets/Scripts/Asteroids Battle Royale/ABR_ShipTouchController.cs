using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ABR_ShipTouchController : ABR_Ship
{
    [SerializeField]
    private Collider2D m_touchArea = null;
    [SerializeField]
    private Camera m_viewport = null;

    [SerializeField]
    private float m_thrustRadius = 5f;
    [SerializeField]
    private float m_turnOnlyRadius = 2.5f;
    [SerializeField]
    private float m_tolerance = 5f;

    private TouchInfo m_touchInfo = null;

    [SerializeField]
    private SpriteRenderer m_touchVisual = null;

    [SerializeField]
    private Sprite m_thrustVisual = null;
    [SerializeField]
    private Sprite m_noThrustVisual = null;

    private new void Start()
    {
        MultiTouchManager.Instance.ListenForTouchesOnOverlapWithEvents(m_touchArea.OverlapPoint, CheckNewTouch, m_viewport);

        base.Start();
    }

    private void CheckNewTouch(TouchInfo touchInfo, ETouchEventType eventType)
    {
        if (eventType != ETouchEventType.EXIT && m_touchInfo == null && touchInfo.m_owners == 0)
        {
            touchInfo.m_owners++;
            touchInfo.ListenForEnd(OnEnd);
            m_touchInfo = touchInfo;
            ToggleTouchVisual(true);

        }
        else if (eventType == ETouchEventType.EXIT && m_touchInfo != null && touchInfo.FingerId == m_touchInfo.FingerId)
        {
            OnEnd(touchInfo);
        }
    }

    private void OnEnd(TouchInfo touchInfo)
    {
        touchInfo.m_owners--;
        touchInfo.StopListenForEnd(OnEnd);
        m_touchInfo = null;
        StopThrust();
        StopTurnTo();
        ToggleTouchVisual(false);
    }

    private void ToggleTouchVisual(bool visible)
    {
        if (m_touchVisual != null)
        {
            m_touchVisual.gameObject.SetActive(visible);
        }
    }

    new void Update()
    {
        if (m_touchInfo != null)
        {
            Vector2 touchPosition = m_touchInfo.WorldPosition(m_viewport);
            if (m_touchVisual != null)
            {
                m_touchVisual.transform.position = touchPosition;
                Quaternion visualRotation = Quaternion.Euler(0, 0, transform.position.ZAngleTo(touchPosition));
                m_touchVisual.transform.rotation = visualRotation;
            }

            float turnAngle = transform.position.ZAngleTo(touchPosition);

            if (Mathf.Abs(Mathf.DeltaAngle(TurnGoal, turnAngle)) > m_tolerance)
            {
                TurnTo(turnAngle);
            }

            if ((touchPosition - (Vector2)transform.position).magnitude > m_turnOnlyRadius)
            {
                if (m_touchVisual != null)
                {
                    m_touchVisual.sprite = m_thrustVisual;
                }
                Thrust(1);
            }
            else
            {
                if (m_touchVisual != null)
                {
                    m_touchVisual.sprite = m_noThrustVisual;
                }
                StopThrust();
            }
        }
        else
        {
            //debug controls

            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                TurnClockwise();
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                TurnCounterClockWise();
            }
            else if (Input.GetAxisRaw("Horizontal") == 0)
            {
                StopTurn();
            }

            if (Input.GetAxisRaw("Vertical") > 0)
            {
                Thrust(1);
            }
            else if (Input.GetAxisRaw("Vertical") == 0)
            {
                StopThrust();
            }

            if (Input.GetAxisRaw("Fire") == 1)
            {
                GetComponentInChildren<ABR_Turret>().FireBullet();
            }
        }


        base.Update();
    }
}
