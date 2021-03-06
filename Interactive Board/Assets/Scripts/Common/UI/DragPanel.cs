﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public delegate void PointerEventDelegate(PointerEventData data);

public class DragPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Vector2 TouchPosition { get; private set; } = Vector2.zero;
    public bool PointerDown { get; private set; }

    private int pointerId = 0;

    public event PointerEventDelegate onPointerDrag;
    public event PointerEventDelegate onPointerUp;
    public event PointerEventDelegate onPointerDown;

    public void AddPointerDragListener(PointerEventDelegate pointerEventDelegate)
    {
        onPointerDrag += pointerEventDelegate;
    }

    public void AddPointerUpListener(PointerEventDelegate pointerEventDelegate)
    {
        onPointerUp += pointerEventDelegate;
    }

    public void AddPointerDownListener(PointerEventDelegate pointerEventDelegate)
    {
        onPointerDown += pointerEventDelegate;
    }

    public void OnDrag(PointerEventData data)
    {
        if (PointerDown && pointerId == data.pointerId)
        {
            Vector3 newPos = data.position;
            TouchPosition = newPos;

            onPointerDrag?.Invoke(data);
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (PointerDown && pointerId == data.pointerId)
        {
            TouchPosition = Vector2.zero;
            PointerDown = false;

            onPointerUp?.Invoke(data);
        }
    }


    public void OnPointerDown(PointerEventData data)
    {
        if (!PointerDown)
        {
            pointerId = data.pointerId;
            PointerDown = true;
            OnDrag(data);

            onPointerDown?.Invoke(data);
        }
    }

    public void KillInput()
    {
        TouchPosition = Vector2.zero;
        PointerDown = false;

        onPointerUp?.Invoke(null);
    }
}
