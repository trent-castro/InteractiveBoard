using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ABR_Ship))]
public class ABR_ShipSliderThrustController : MonoBehaviour
{

    [SerializeField]
    private Slider m_thrustInput = null;

    [SerializeField]
    private GameObject m_touchVisual = null;

    private ABR_Ship m_ship = null;

    private bool started = false;

    private void Awake()
    {
        m_ship = GetComponent<ABR_Ship>();
    }

    private void OnEnable()
    {
        if (started) { StartTouch(); }
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
        float thrust = m_thrustInput.value / m_thrustInput.maxValue;

        if (thrust > 0)
        {
            if (!started)
            {
                StartTouch();
            }
            m_ship.Thrust(thrust);
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
