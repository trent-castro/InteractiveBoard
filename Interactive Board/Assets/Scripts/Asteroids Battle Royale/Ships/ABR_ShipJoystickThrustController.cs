using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ABR_Ship))]
public class ABR_ShipJoystickThrustController : MonoBehaviour
{
    [SerializeField]
    Joystick m_input = null;

    [SerializeField]
    private float m_thrustRadius = 40f;
    [SerializeField]
    private float m_turnOnlyRadius = 20f;

    [SerializeField]
    private GameObject m_touchVisual = null;

    private ABR_Ship m_ship = null;

    private bool started = false;

    private void OnEnable()
    {
        if (started) { StartTouch(); }
    }

    private void Awake()
    {
        m_ship = GetComponent<ABR_Ship>();
    }

    private void StartTouch()
    {
        started = true;
        m_touchVisual.transform.localPosition = Vector3.up * 5;
        m_touchVisual.transform.localRotation = Quaternion.identity;
        ToggleTouchVisual(true);
    }

    private void EndTouch()
    {
        m_ship.StopThrust();
        ToggleTouchVisual(false);
        started = false;
    }

    private void ToggleTouchVisual(bool visible)
    {
        if (m_touchVisual != null)
        {
            m_touchVisual.gameObject.SetActive(visible);
        }
    }

    private void Update()
    {
        if (m_input.PointerDown)
        {
            if (!started)
            {
                StartTouch();
            }

            Vector2 input = m_input.Axes;

            float distanceFromCenter = input.magnitude;

            m_ship.Thrust(Mathf.Clamp01((distanceFromCenter - m_turnOnlyRadius) / (m_thrustRadius - m_turnOnlyRadius)));
        }
        else
        {
            if (started)
            {
                EndTouch();
            }
        }
    }
}
