using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera2D : MonoBehaviour
{
    public Transform m_target = null;
    private Vector3 m_velocity = new Vector3();

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_target == null) return;

        Vector3 targetPosition = new Vector3(m_target.transform.position.x, m_target.transform.position.y, transform.position.z);

        Vector3 oldPosition = transform.position;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref m_velocity, .05f);
    }
}
