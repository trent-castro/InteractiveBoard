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
        float turn = Input.GetAxis("Horizontal");
        bool doThrust = false;

        if (Input.GetAxis("Vertical") == 1)
        {
            doThrust = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_ship.GetComponentInChildren<ABR_Turret>().FireBullet();
        }

        m_ship.Turn = turn;
        m_ship.DoThrust = doThrust;
    }
}