using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FollowPointer : MonoBehaviour
{
    [SerializeField]
    private CompositeTouchArea m_touchArea = null;
    private TouchInfo m_touchInfo = null;

    private SpriteRenderer m_spriteRenderer = null;

    private void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        MultiTouchManager.Instance.ListenForTouchesOnOverlapWithEvents(m_touchArea.OverlapPoint, CheckForTouch);
    }

    private void CheckForTouch(TouchInfo touch, ETouchEventType eventType)
    {
        if (eventType != ETouchEventType.EXIT && m_touchInfo == null)
        {
            Debug.Log("Touch Start");

            m_touchInfo = touch;
            m_touchInfo.ListenForMove(OnMove);
            m_touchInfo.ListenForEnd(OnEnd);
            transform.position = m_touchInfo.WorldPosition();

            m_spriteRenderer.enabled = true;
        }
        else if (eventType == ETouchEventType.EXIT && m_touchInfo.FingerId == touch.FingerId)
        {
            Debug.Log("Touch Exit");

            OnEnd(touch);
        }
    }

    private void OnMove(TouchInfo touch)
    {
        Debug.Log("Touch Move");

        transform.position = m_touchInfo.WorldPosition();
    }

    private void OnEnd(TouchInfo touch)
    {
        Debug.Log("Touch End");
        m_touchInfo.StopListenForMove(OnMove);
        m_touchInfo.StopListenForEnd(OnEnd);
        m_touchInfo = null;
        m_spriteRenderer.enabled = false;
    }
}
