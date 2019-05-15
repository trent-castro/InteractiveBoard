using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class ABR_Border : MonoBehaviour
{
    private Collider2D m_playArea = null;
    private List<ABR_Ship> runaways = new List<ABR_Ship>();

    // Start is called before the first frame update
    void Start()
    {
        m_playArea = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (ABR_Ship ship in runaways)
        {
            Vector2 target = m_playArea.bounds.ClosestPoint(ship.m_rigidBody.position);

            if (ship != null)
            {
                float targetRotation = ship.m_rigidBody.position.ZAngleTo(target);
                if (Mathf.Abs(Mathf.DeltaAngle(ship.transform.eulerAngles.z, targetRotation)) > 30)
                {
                    ship.TurnTo(targetRotation, true);
                } else
                {
                    ship.Thrust(1, true);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ABR_Ship ship = other.GetComponent<ABR_Ship>();
        if (ship != null)
        {
            runaways.Add(ship);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ABR_Ship ship = other.GetComponent<ABR_Ship>();
        if (ship != null)
        {
            runaways.Remove(ship);
            ship.StopThrust(true);
            ship.StopTurnTo(true);
        }
    }
}
