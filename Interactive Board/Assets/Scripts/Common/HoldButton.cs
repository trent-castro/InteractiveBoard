using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldButton : Button, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private UnityEvent onDown = new UnityEvent();
    [SerializeField]
    private UnityEvent onUp = new UnityEvent();


    public override void OnPointerDown(PointerEventData eventData)
    {
        onDown?.Invoke();

        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        onUp?.Invoke();

        base.OnPointerUp(eventData);
    }
}
