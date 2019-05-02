using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate bool OverlapPointDelegate(Vector2 point);

public enum EListenType
{
    NEW = 1,
    CURRENT = 2,
    BOTH = 3,
}

public enum ETouchEventType
{
    ENTER,
    EXIT,
    IN,
    NEW
}

public delegate void HandleTouchEvent(TouchInfo touch, ETouchEventType eventType);

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

    private event HandleTouchInfo HandleNewTouches;
    private event HandleTouchInfo HandleCurrentTouches;

    public void ListenForNewTouches(HandleTouchInfo handler)
    {
        HandleNewTouches += handler;
    }

    public void ListenForCurrentTouches(HandleTouchInfo handler)
    {
        HandleCurrentTouches += handler;
    }

    public void ListenForTouchesOnOverlap(OverlapPointDelegate overlapPoint, HandleTouchEvent handler, EListenType listenType)
    {
        if (listenType.HasFlag(EListenType.NEW))
        {
            ListenForNewTouches(TouchAreaHandler(overlapPoint, handler));
        }

        if (listenType.HasFlag(EListenType.CURRENT))
        {
            ListenForCurrentTouches(TouchAreaHandler(overlapPoint, handler));
        }
    }

    private static HandleTouchInfo TouchAreaHandler(OverlapPointDelegate overlapPoint, HandleTouchEvent handler)
    {
        

        return touch =>
        {
            if (overlapPoint(Camera.main.ScreenToWorldPoint(touch.Position)))
            {
                handler(touch, ETouchEventType.IN);
            }
        };
    }

    public void ListenForMultiTapsOnCollider(OverlapPointDelegate overlapPoint, int tapCount, float maxTime, HandleTouchInfo handler)
    {
        int count = 0;
        TouchInfo[] taps = new TouchInfo[tapCount];

        ListenForNewTouches(TouchAreaHandler(overlapPoint, (touch, type) =>
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
                    HandleCurrentTouches?.Invoke(Touches[touch.fingerId]);
                }
            }
            else if (!Touches.ContainsKey(touch.fingerId))
            {
                TouchInfo newTouch = new TouchInfo(touch);

                Touches.Add(touch.fingerId, newTouch);

                HandleNewTouches?.Invoke(newTouch);
            }
        }

        int[] keys = Touches.Keys.ToArray();

        for (int i = 0; i < keys.Length; i++)
        {
            if (!Input.touches.Any(t => t.fingerId == keys[i]))
            {
                Touch t = Touches[keys[i]].touch;

                t.phase = TouchPhase.Ended;

                Touches[keys[i]].UpdateTouchInfo(t);

                Touches.Remove(keys[i]);
            }
        }
    }
}
