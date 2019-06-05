using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public enum AxisOption
    {
        // Options for which axes to use
        Both, // Use both
        OnlyHorizontal, // Only horizontal
        OnlyVertical // Only vertical
    }

    public int MovementRange = 50;
    public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use

    public bool m_isDynamic = false;
    public DragPanel m_dynamicArea = null;

    public Vector2 Axes { get; private set; } = Vector2.zero;
    public float X { get { return Axes.x; } }
    public float Y { get { return Axes.y; } }
    public bool PointerDown { get; private set; }

    Vector3 m_StartPos;
    bool m_UseX; // Toggle for using the x axis
    bool m_UseY; // Toggle for using the Y axis

    [SerializeField]
    GameObject m_Indicator = null;

    public void Toggle(bool on)
    {
        gameObject.SetActive(!m_isDynamic ? on : false);
    }

    public void ToggleDynamic(bool on)
    {
        m_isDynamic = on;
        Toggle(true);
    }

    void OnEnable()
    {
        SetAxesToUse();
    }

    void Awake()
    {
        m_StartPos = transform.position;
        if (m_dynamicArea != null)
        {
            m_dynamicArea.AddPointerDragListener(DynamicOnPointerDrag);
            m_dynamicArea.AddPointerUpListener(DynamicOnPointerUp);
            m_dynamicArea.AddPointerDownListener(DynamicOnPointerDown);
        }
        gameObject.SetActive(false);
    }

    public void KillInput()
    {
        if (m_isDynamic)
        {

            DynamicOnPointerUp(null);
        }
        else
        {
            OnPointerUp(null);
        }
    }

    private void DynamicOnPointerDrag(PointerEventData data)
    {
        if (m_isDynamic)
        {
            OnDrag(data);
        }
    }

    private void DynamicOnPointerUp(PointerEventData data)
    {
        if (m_isDynamic)
        {
            OnPointerUp(data);
            transform.position = m_StartPos;
            gameObject.SetActive(false);
        }
    }

    private void DynamicOnPointerDown(PointerEventData data)
    {
        if (m_isDynamic)
        {
            gameObject.SetActive(true);
            transform.position = data.position;
            OnPointerDown(data);
        }
    }

    void SetAxesToUse()
    {
        m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
        m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);
    }


    public void OnDrag(PointerEventData data)
    {
        Vector3 newPos = Vector3.zero;

        if (m_UseX)
        {
            newPos.x = data.position.x - transform.position.x;
        }

        if (m_UseY)
        {
            newPos.y = data.position.y - transform.position.y;
        }

        float magnitude = newPos.magnitude;
        magnitude = Mathf.Clamp(magnitude, 0, MovementRange);
        newPos = newPos.normalized * magnitude;


        m_Indicator.transform.position = transform.position + newPos;

        Axes = newPos;
    }

    public void OnPointerUp(PointerEventData data)
    {
        m_Indicator.transform.localPosition = Vector2.zero;

        Axes = Vector2.zero;
        PointerDown = false;
    }


    public void OnPointerDown(PointerEventData data)
    {
        PointerDown = true;
        OnDrag(data);
    }
}