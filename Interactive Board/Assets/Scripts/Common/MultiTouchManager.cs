using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ETouchEventType
{
    ENTER,
    EXIT,
    IN,
}

public delegate bool OverlapPointDelegate(Vector2 point);

public delegate void HandleTouchInfoDelegate(TouchInfo touch);
public delegate void HandleTouchEventDelegate(TouchInfo touch, ETouchEventType eventType);

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

    public TouchInfo[] currentTouches;

    private Dictionary<int, TouchInfo> Touches { get; set; } = new Dictionary<int, TouchInfo>();

    private event HandleTouchInfoDelegate HandleTouches;

    public void ListenForTouches(HandleTouchInfoDelegate handler)
    {
        HandleTouches += handler;
    }

    public void ListenForTouchesOnOverlapWithEvents(OverlapPointDelegate overlapPoint, HandleTouchEventDelegate handler, Camera camera = null)
    {
        ListenForTouches(TouchAreaHandlerWithEvents(overlapPoint, handler, camera));
    }

    public void ListenForTouchesOnOverlapSimple(OverlapPointDelegate overlapPoint, HandleTouchInfoDelegate handler, Camera camera = null)
    {
        ListenForTouches(TouchAreaHandlerSimple(overlapPoint, handler, camera));
    }

    private static HandleTouchInfoDelegate TouchAreaHandlerWithEvents(OverlapPointDelegate overlapPoint, HandleTouchEventDelegate handler, Camera camera = null)
    {
        return touch =>
        {
            bool overlap = overlapPoint(touch.WorldPosition(camera));
            bool overlapLastFrame = overlapPoint(touch.LastWorldPosition(camera));
            if (overlap && overlapLastFrame)
            {
                handler(touch, ETouchEventType.IN);
            }
            if (overlap && !overlapLastFrame)
            {
                handler(touch, ETouchEventType.ENTER);
            }
            if (!overlap && overlapLastFrame)
            {
                handler(touch, ETouchEventType.EXIT);
            }
        };
    }

    private static HandleTouchInfoDelegate TouchAreaHandlerSimple(OverlapPointDelegate overlapPoint, HandleTouchInfoDelegate handler, Camera camera = null)
    {
        return touch =>
        {
            bool overlap = overlapPoint(touch.WorldPosition(camera));
            if (overlap)
            {
                handler(touch);
            }
        };
    }

    public void ListenForMultiTapsOnCollider(OverlapPointDelegate overlapPoint, int tapCount, float maxTime, HandleTouchInfoDelegate handler)
    {
        int count = 0;
        TouchInfo[] taps = new TouchInfo[tapCount];

        ListenForTouches(TouchAreaHandlerSimple(overlapPoint, (touch) =>
        {
            while (touch.m_startTime - taps[0].m_startTime > maxTime)
            {
                for (int i = 1; i < count; i++)
                {
                    taps[i - 1] = taps[i];
                }
            }

            if (count < tapCount - 1)
            {
                taps[count] = touch;
                count++;
            }
            else
            {
                count = 0;
                handler(touch);
            }
        }));
    }

    void Update()
    {
        foreach (Touch t in Input.touches)
        {
            Touch touch = t;

            if (touch.phase != TouchPhase.Began && Touches.ContainsKey(touch.fingerId))
            {
                Touches[touch.fingerId].UpdateTouchInfo(touch);

                if (touch.phase == TouchPhase.Ended)
                {
                    Touches.Remove(touch.fingerId);
                }
                else
                {
                    HandleTouches?.Invoke(Touches[touch.fingerId]);
                }
            }
            else if (!Touches.ContainsKey(touch.fingerId))
            {
                TouchInfo newTouch = new TouchInfo(touch);

                Touches.Add(touch.fingerId, newTouch);

                HandleTouches?.Invoke(newTouch);
            }
        }

        int[] keys = Touches.Keys.ToArray();

        for (int i = 0; i < keys.Length; i++)
        {
            if (!Input.touches.Any(t => t.fingerId == keys[i]))
            {
                Touch t = Touches[keys[i]].m_touch;

                t.phase = TouchPhase.Ended;

                Touches[keys[i]].UpdateTouchInfo(t);

                Touches.Remove(keys[i]);
            }
        }
    }
}
