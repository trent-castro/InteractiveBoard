﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HandleTouchInfo(TouchInfo touch);

[Serializable]
public class TouchInfo
{
    public Vector2 DeltaPosition { get { return touch.deltaPosition; } }
    public float DeltaTime { get { return touch.deltaTime; } }
    public int FingerId { get { return touch.fingerId; } }
    public TouchPhase Phase { get { return touch.phase; } }
    public Vector2 Position { get { return touch.position; } }

    public int lastHistoryIndex = 0;
    public Touch[] history;
    public Touch touch;
    public float startTime;
    public float endTime;
    public int owners = 0;

    private int shouldTrack = 0;
    private int trackOn = 1;

    private event HandleTouchInfo HandleMove;
    private event HandleTouchInfo HandleEnd;
    private event HandleTouchInfo HandleStationary;

    public void ListenForMove(HandleTouchInfo handler)
    {
        HandleMove += handler;
    }

    public void StopListenForMove(HandleTouchInfo handler)
    {
        HandleMove -= handler;
    }

    public void ListenForEnd(HandleTouchInfo handler)
    {
        HandleEnd += handler;
    }

    public void StopListenForEnd(HandleTouchInfo handler)
    {
        HandleEnd -= handler;
    }

    public void ListenForStationary(HandleTouchInfo handler)
    {
        HandleStationary += handler;
    }

    public void StopListenForStationary(HandleTouchInfo handler)
    {
        HandleStationary -= handler;
    }

    public TouchInfo(Touch t)
    {
        history = new Touch[100];

        startTime = Time.fixedTime;

        history[0] = t;
        touch = t;

        endTime = Time.fixedTime;
    }

    public void UpdateTouchInfo(Touch t)
    {
        touch = t;

        for (int i = 0; i < lastHistoryIndex; i++)
        {
            Debug.DrawLine(Camera.main.ScreenToWorldPoint((Vector3)history[i].position + Vector3.forward * 10), Camera.main.ScreenToWorldPoint((Vector3)history[i + 1].position + Vector3.forward * 10), Color.blue);
        }

        shouldTrack++;
        if (shouldTrack == trackOn)
        {
            shouldTrack = 0;

            if (lastHistoryIndex == 99)
            {
                trackOn *= 2;
                for (int i = 0; i < lastHistoryIndex; i += 2)
                {
                    history[i / 2] = history[i];
                }

                lastHistoryIndex = 49;
            }

            lastHistoryIndex++;
            history[lastHistoryIndex] = t;
            endTime = Time.fixedTime;
        }

        switch (touch.phase)
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