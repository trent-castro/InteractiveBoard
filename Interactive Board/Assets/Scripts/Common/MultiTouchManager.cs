using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiTouchManager : MonoBehaviour
{
    public static MultiTouchManager Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Collider2D m_captureArea = null;
    public TouchInfo[] currentTouches;

    private Dictionary<int, TouchInfo> m_touches { get; set; } = new Dictionary<int, TouchInfo>();

    private event HandleTouchInfo HandleNewTouches;
    private event HandleTouchInfo HandleCurrentTouches;

    public void ListenForNewTouches(HandleTouchInfo handler)
    {
        HandleNewTouches += handler;
    }

    public void ListenForNewTouchesOnCollider(Collider2D collider, HandleTouchInfo handler)
    {
        ListenForNewTouches(touch =>
        {
            if (collider.OverlapPoint(Camera.main.ScreenToWorldPoint(touch.Position)))
            {
                handler(touch);
            }
        });
    }

    public void ListenForCurrentTouchesOnCollider(Collider2D collider, HandleTouchInfo handler)
    {
        HandleCurrentTouches += touch =>
        {
            if (collider.OverlapPoint(Camera.main.ScreenToWorldPoint(touch.Position)))
            {
                handler(touch);
            }
        };
    }

    public void ListenForMultiTapsOnCollider(Collider2D collider, int tapCount, float maxTime, HandleTouchInfo handler)
    {
        int count = 0;
        TouchInfo[] taps = new TouchInfo[tapCount];

        HandleNewTouches += touch =>
        {
            if (collider.OverlapPoint(Camera.main.ScreenToWorldPoint(touch.Position)))
            {
                while (touch.startTime - taps[0].startTime > maxTime)
                {
                    for (int i = 1; i < count; i++)
                    {
                        taps[i - 1] = taps[i];
                    }
                }

                if (count < tapCount - 1)
                {
                    taps[count] = touch;
                }
                else
                {
                    count = 0;
                    handler(touch);
                }
            }
        };
    }

    void Update()
    {
        foreach (Touch t in Input.touches)
        {
            Touch touch = t;

            if (m_captureArea != null)
            {
                if (!m_captureArea.OverlapPoint(Camera.main.ScreenToWorldPoint(touch.position)))
                {
                    touch.phase = TouchPhase.Ended;
                }
            }

            if (touch.phase != TouchPhase.Began && m_touches.ContainsKey(touch.fingerId))
            {
                m_touches[touch.fingerId].UpdateTouchInfo(touch);

                if (touch.phase == TouchPhase.Ended)
                {
                    m_touches.Remove(touch.fingerId);
                }
                else
                {
                    HandleCurrentTouches?.Invoke(m_touches[touch.fingerId]);
                }
            }
            else if (!m_touches.ContainsKey(touch.fingerId))
            {
                TouchInfo newTouch = new TouchInfo(touch);

                m_touches.Add(touch.fingerId, newTouch);

                HandleNewTouches?.Invoke(newTouch);
            }
        }

        int[] keys = m_touches.Keys.ToArray();

        for (int i = 0; i < keys.Length; i++)
        {
            if (!Input.touches.Any(t => t.fingerId == keys[i]))
            {
                Touch t = m_touches[keys[i]].touch;

                t.phase = TouchPhase.Ended;

                m_touches[keys[i]].UpdateTouchInfo(t);

                m_touches.Remove(keys[i]);
            }
        }
    }
}
