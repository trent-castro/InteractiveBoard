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
            Vector2 velocity = ship.m_rigidBody.velocity;
            Vector2.SmoothDamp(ship.m_rigidBody.position, target, ref velocity, 0.2f);
            ship.m_rigidBody.velocity = Vector2.SmoothDamp(ship.m_rigidBody.velocity, velocity, ref ship.m_acceleration, .25f);

            if (ship != null)
            {
                Vector2 toTarget = (target - ship.m_rigidBody.position).normalized;
                float targetRotation = Mathf.Rad2Deg * Mathf.Atan2(toTarget.y, toTarget.x) - 90;
                if (Mathf.Abs(Mathf.DeltaAngle(ship.transform.eulerAngles.z, targetRotation)) > 30)
                {
                    ship.TurnTo(targetRotation);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ABR_Ship rb = other.GetComponent<ABR_Ship>();
        if (rb != null)
        {
            runaways.Add(rb);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ABR_Ship rb = other.GetComponent<ABR_Ship>();
        if (rb != null)
        {
            runaways.Remove(rb);
        }
    }
}
