using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Vector2 TouchPosition { get; private set; } = Vector2.zero;
    public bool PointerDown { get; private set; }

    public void OnDrag(PointerEventData data)
    {
        Vector3 newPos = data.position;

        TouchPosition = newPos;
    }

    public void OnPointerUp(PointerEventData data)
    {
        TouchPosition = Vector2.zero;
        PointerDown = false;
    }


    public void OnPointerDown(PointerEventData data)
    {
        PointerDown = true;
        OnDrag(data);
    }
}
