using System;
using System.Collections;
using System.Collections.Generic;
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

    public TouchInfo[] currentTouches;
    
    private Dictionary<int, TouchInfo> touches { get; set; } = new Dictionary<int, TouchInfo>();

    private event HandleTouchInfo HandleNewTouches;

    public void ListenForNewTouches(HandleTouchInfo handler)
    {
        HandleNewTouches += handler;
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase != TouchPhase.Began && touches.ContainsKey(touch.fingerId))
            {
                touches[touch.fingerId].UpdateTouchInfo(touch);

                if (touch.phase == TouchPhase.Ended)
                {
                    touches.Remove(touch.fingerId);
                }
            }
            else if (!touches.ContainsKey(touch.fingerId))
            {
                TouchInfo newTouch = new TouchInfo(touch);
                touches.Add(touch.fingerId, newTouch);
                HandleNewTouches?.Invoke(newTouch);
            }

        }
    }
}
