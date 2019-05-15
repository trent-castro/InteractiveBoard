using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TouchInfo
{
    public Vector2 DeltaPosition { get { return m_touch.deltaPosition; } }
    public float DeltaTime { get { return m_touch.deltaTime; } }
    public int FingerId { get { return m_touch.fingerId; } }
    public TouchPhase Phase { get { return m_touch.phase; } }
    public Vector2 Position { get { return m_touch.position; } }
    public Vector2 LastPosition { get { return m_touch.position - m_touch.deltaPosition; } }

    public Vector2 WorldPosition(Camera camera = null)
    {
        if (camera == null)
        {
            camera = Camera.main;
        }
        return camera.ScreenToWorldPoint(Position);
    }

    public Vector2 LastWorldPosition(Camera camera = null)
    {
        if (camera == null)
        {
            camera = Camera.main;
        }
        return camera.ScreenToWorldPoint(LastPosition);
    }

    private int m_lastHistoryIndex = 0;
    public Touch[] m_history;
    public Touch m_touch;
    public float m_startTime;
    public float m_endTime;
    public int m_owners = 0;

    private int m_trackCounter = 0;
    private int m_trackOn = 1;

    private event HandleTouchInfoDelegate HandleMove;
    private event HandleTouchInfoDelegate HandleEnd;
    private event HandleTouchInfoDelegate HandleStationary;

    public void ListenForMove(HandleTouchInfoDelegate handler)
    {
        HandleMove += handler;
    }

    public void StopListenForMove(HandleTouchInfoDelegate handler)
    {
        HandleMove -= handler;
    }

    public void ListenForEnd(HandleTouchInfoDelegate handler)
    {
        HandleEnd += handler;
    }

    public void StopListenForEnd(HandleTouchInfoDelegate handler)
    {
        HandleEnd -= handler;
    }

    public void ListenForStationary(HandleTouchInfoDelegate handler)
    {
        HandleStationary += handler;
    }

    public void StopListenForStationary(HandleTouchInfoDelegate handler)
    {
        HandleStationary -= handler;
    }

    public TouchInfo(Touch t, bool track = false)
    {
        m_history = new Touch[100];

        m_startTime = Time.fixedTime;

        m_history[0] = t;
        m_touch = t;

        m_endTime = Time.fixedTime;
    }

    public void UpdateTouchInfo(Touch t)
    {
        m_touch = t;

        m_trackCounter++;
        if (m_trackCounter == m_trackOn)
        {
            m_trackCounter = 0;

            if (m_lastHistoryIndex == 99)
            {
                m_trackOn *= 2;
                for (int i = 0; i < m_lastHistoryIndex; i += 2)
                {
                    m_history[i / 2] = m_history[i];
                }

                m_lastHistoryIndex = 49;
            }

            m_lastHistoryIndex++;
            m_history[m_lastHistoryIndex] = t;
            m_endTime = Time.fixedTime;
        }

        switch (m_touch.phase)
        {
            case TouchPhase.Moved:
                HandleMove?.Invoke(this);
                break;
            case TouchPhase.Stationary:
                HandleStationary?.Invoke(this);
                break;
            case TouchPhase.Ended:
                HandleEnd?.Invoke(this);
                break;
            case TouchPhase.Canceled:

                break;
        }
    }
}