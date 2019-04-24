using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera2D : MonoBehaviour
{
    public Rigidbody2D m_target = null;
    private Vector2 m_velocity = Vector2.zero;
    private Vector2 m_targetOffset = Vector2.zero;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_target == null) return;

        Vector2 targetPosition = new Vector2(m_target.transform.position.x, m_target.transform.position.y);
        m_targetOffset = Vector2.Lerp(m_targetOffset, m_target.velocity * 0.3f + (Vector2)m_target.transform.up * 1.5f, Time.deltaTime * 5);

        Vector2 oldPosition = transform.position;
        transform.position = Vector2.SmoothDamp(transform.position, targetPosition + m_targetOffset, ref m_velocity, .15f);
        transform.position += Vector3.forward * -10;
    }
}
