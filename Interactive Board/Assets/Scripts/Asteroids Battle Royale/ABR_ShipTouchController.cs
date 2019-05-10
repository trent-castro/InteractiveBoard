﻿using System.Collections;
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

    private void Start()
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
    }

    new void Update()
    {
        if (m_touchInfo != null)
        {
            Vector2 touchPosition = m_touchInfo.WorldPosition(m_viewport);

            float turnAngle = transform.position.ZAngleTo(touchPosition);

            if (Mathf.Abs(Mathf.DeltaAngle(TurnGoal, turnAngle)) > m_tolerance)
            {
                TurnTo(turnAngle);
            }

            if ((touchPosition - (Vector2)transform.position).magnitude > m_turnOnlyRadius)
            {
                Thrust(1);
            }
            else
            {
                StopThrust();
            }
        }

        //debug controls
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            TurnCounterClockWise();
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            TurnClockwise();
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) != Input.GetKeyUp(KeyCode.RightArrow))
        {
            StopTurn();
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            Thrust(1);
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            StopThrust();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            GetComponentInChildren<ABR_Turret>().FireBullet();
        }

        base.Update();
    }
}
