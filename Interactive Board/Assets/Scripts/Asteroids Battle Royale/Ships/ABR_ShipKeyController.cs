using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_ShipKeyController : ABR_Ship
{
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

        if(Input.GetKeyUp(KeyCode.Space))
        {
            GetComponentInChildren<ABR_Turret>().FireBullet();
        }

        base.Update();
    }
}
