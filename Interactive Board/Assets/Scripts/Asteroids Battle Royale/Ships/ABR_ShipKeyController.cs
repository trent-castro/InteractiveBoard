using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that allows for a different control scheme for ship maneuvering.
/// </summary>
[RequireComponent(typeof(ABR_Ship))]
public class ABR_ShipKeyController : MonoBehaviour
{
    // Private internal data members
    /// <summary>
    /// The ship that the controls are affecting.
    /// </summary>
    private ABR_Ship m_ship = null;

    /// <summary>
    /// Whether or not the ship is attempting to move horizontally.
    /// </summary>
    private bool horizontal = false;
    /// <summary>
    /// Whether or not the ship is attempting to move vertically.
    /// </summary>
    private bool vertical = false;

    private void Awake()
    {
        m_ship = GetComponent<ABR_Ship>();
    }

    private void Update()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            m_ship.TurnClockwise();
            horizontal = true;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            m_ship.TurnCounterClockWise();
            horizontal = true;
        }
        else if (horizontal)
        {
            horizontal = false;
            m_ship.StopTurn();
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            m_ship.Thrust(1);
            vertical = true;
        }
        else if (vertical)
        {
            vertical = false;
            m_ship.StopThrust();
        }

        if (Input.GetButtonDown("Fire"))
        {
            m_ship.GetComponentInChildren<ABR_Turret>().FireBullet();
        }
    }
}