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

    public Vector2 Axes { get; private set; } = Vector2.zero;
    public float X { get { return Axes.x; } }
    public float Y { get { return Axes.y; } }
    public bool PointerDown { get; private set; }

    Vector3 m_StartPos;
    bool m_UseX; // Toggle for using the x axis
    bool m_UseY; // Toggle for using the Y axis

    [SerializeField]
    GameObject m_Indicator = null;

    void OnEnable()
    {
        SetAxesToUse();
    }

    void Start()
    {
        m_StartPos = transform.position;
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
            newPos.x = data.position.x - m_StartPos.x;
        }

        if (m_UseY)
        {
            newPos.y = data.position.y - m_StartPos.y;
        }

        float magnitude = newPos.magnitude;
        magnitude = Mathf.Clamp(magnitude, 0, MovementRange);
        newPos = newPos.normalized * magnitude;

        if (m_Indicator != null)
        {
            m_Indicator.transform.position = m_StartPos + newPos;
        }
        else
        {
            transform.position = m_StartPos + newPos;
        }
        Axes = newPos;
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (m_Indicator != null)
        {
            m_Indicator.transform.position = m_StartPos;
        }
        else
        {
            transform.position = m_StartPos;
        }
        Axes = Vector2.zero;
        PointerDown = false;
    }


    public void OnPointerDown(PointerEventData data)
    {
        PointerDown = true;
        OnDrag(data);
    }
}