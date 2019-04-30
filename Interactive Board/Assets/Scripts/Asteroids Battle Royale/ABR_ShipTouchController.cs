using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_ShipTouchController : ABR_Ship
{
    private Transform m_target = null;
    new void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            TurnCounterClockWise();
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            TurnClockwise();
        }

        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            StopTurn();
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            Thrust(1);
        }
        else
        {
            StopThrust();
        }

        base.Update();
    }
}
