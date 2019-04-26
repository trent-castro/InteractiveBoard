using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_ShipKeyController : MonoBehaviour
{
    [SerializeField]
    private ABR_Ship m_ship = null;

    // Update is called once per frame
    void Update()
    {
        int turn = 0;
        bool doThrust = false;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            ++turn;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            --turn;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            doThrust = true;
        }

        m_ship.Turn = turn;
        m_ship.DoThrust = doThrust;
    }
}
