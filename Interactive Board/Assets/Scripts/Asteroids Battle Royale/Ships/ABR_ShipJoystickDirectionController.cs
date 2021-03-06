﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that allows for a different control scheme for ship maneuvering.
/// </summary>
[RequireComponent(typeof(ABR_Ship))]
public class ABR_ShipJoystickDirectionController : MonoBehaviour
{
    [Header("External References")]
    [Tooltip("A reference to the joystick in the UI.")]
    [SerializeField]
    private Joystick m_input = null;

    [Header("Configuration")]
    [Tooltip("The amount of change from the current direction of the ship to the desired direction before it will start turning.")]
    [SerializeField]
    private float m_tolerance = 5.0f;
    
    private ABR_Ship m_ship = null;

    private void Awake()
    {
        m_ship = GetComponent<ABR_Ship>();
    }

    private bool started = false;

    private void StartTouch()
    {
        started = true;
    }

    private void EndTouch()
    {
        m_ship.StopTurnTo();
        started = false;
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

            float turnAngle = input.ZAngle();

            if (Mathf.Abs(Mathf.DeltaAngle(m_ship.TurnGoal, turnAngle)) > m_tolerance)
            {
                m_ship.TurnTo(turnAngle);
            }
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