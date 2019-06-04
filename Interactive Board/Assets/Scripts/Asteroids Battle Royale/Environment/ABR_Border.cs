using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that constrains the players to a specific play area.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ABR_Border : MonoBehaviour
{
    // Private internal data members
    /// <summary>
    /// A reference to the sibling component of the play area.
    /// </summary>
    private Collider2D m_playArea = null;
    /// <summary>
    /// A list of ships that have exited the play area.
    /// </summary>
    private List<ABR_Ship> runaways = new List<ABR_Ship>();

    void Start()
    {
        GetSiblingComponents();
    }

    /// <summary>
    /// Acquires references to the sibling components of this script.
    /// </summary>
    private void GetSiblingComponents()
    {
        m_playArea = GetComponent<Collider2D>();
    }

    void Update()
    {
        foreach (ABR_Ship ship in runaways)
        {
            Vector2 target = m_playArea.bounds.ClosestPoint(ship.m_RigidBody.position);

            if (ship != null)
            {
                float targetRotation = ship.m_RigidBody.position.ZAngleTo(target);
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
            NotificationReciever notificationReciever = ship.GetComponent<NotificationReciever>();
            if (notificationReciever != null)
            {
                notificationReciever.Notify("Lost Control");
            }
        }
		else
		{
			ABR_Asteroid asteroid = other.GetComponent<ABR_Asteroid>();

			if (asteroid)
			{
				other.gameObject.SetActive(false);
			}
		}
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        ABR_Ship ship = other.GetComponent<ABR_Ship>();
		if (ship != null)
		{
			runaways.Remove(ship);

            NotificationReciever notificationReciever = ship.GetComponent<NotificationReciever>();
            if (notificationReciever != null)
            {
                notificationReciever.Notify("Lost Control", false);
            }

            ship.StopThrust(true);
			ship.StopTurnTo(true);
		}
    }
}
