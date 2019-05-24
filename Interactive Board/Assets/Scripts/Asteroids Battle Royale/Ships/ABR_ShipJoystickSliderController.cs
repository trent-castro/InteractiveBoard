using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ABR_ShipJoystickSliderController : ABR_Ship
{
    [SerializeField]
    Joystick m_dirInput = null;

    [SerializeField]
    Slider m_thrustInput = null;

    [SerializeField]
    private float m_thrustRadius = 50f;
    [SerializeField]
    private float m_turnOnlyRadius = 25f;
    [SerializeField]
    private float m_tolerance = 5f;

    [SerializeField]
    private GameObject m_touchVisual = null;

    private bool started = false;

    private void StartTouch()
    {
        started = true;
        ToggleTouchVisual(true);
    }

    private void EndTouch()
    {
        StopThrust();
        StopTurnTo();
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

    new void Update()
    {
        if (m_dirInput.PointerDown)
        {
            if (!started)
            {
                StartTouch();
            }

            Vector2 input = m_dirInput.Axes;

            float turnAngle = input.ZAngle();

            if (Mathf.Abs(Mathf.DeltaAngle(TurnGoal, turnAngle)) > m_tolerance)
            {
                TurnTo(turnAngle);
            }

            Thrust(m_thrustInput.value / m_thrustInput.maxValue);
        }
        else
        {
            if (started)
            {
                EndTouch();
            }
            //debug controls

            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                TurnClockwise();
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                TurnCounterClockWise();
            }
            else if (Input.GetAxisRaw("Horizontal") == 0)
            {
                StopTurn();
            }

            if (Input.GetAxisRaw("Vertical") > 0)
            {
                Thrust(1);
            }
            else if (Input.GetAxisRaw("Vertical") == 0)
            {
                StopThrust();
            }

            if (Input.GetAxisRaw("Fire") == 1)
            {
                GetComponentInChildren<ABR_Turret>().FireBullet();
            }
        }


        base.Update();
    }
}
