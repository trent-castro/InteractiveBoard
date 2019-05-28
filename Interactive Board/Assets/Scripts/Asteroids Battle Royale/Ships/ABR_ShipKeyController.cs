using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ABR_Ship))]
public class ABR_ShipKeyController : MonoBehaviour
{
    private ABR_Ship m_ship = null;

    private bool horizontal = false;
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
