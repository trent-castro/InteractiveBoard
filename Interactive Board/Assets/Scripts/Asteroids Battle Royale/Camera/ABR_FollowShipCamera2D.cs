using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A camera that follows a game object.
/// </summary>
public class ABR_FollowShipCamera2D : MonoBehaviour
{
    [Header("External References")]
    [Tooltip("A reference to the target to follow's rigid body component.")]
    [SerializeField]
    private Rigidbody2D m_target = null;
    
    // Private internal data members
    /// <summary>
    /// A self-managing "history" of the velocity the camera has been at for smoother movement.
    /// </summary>
    private Vector2 m_velocity = Vector2.zero;
    /// <summary>
    /// An offset of the target while traveling to allow for more vision in a particular direction.
    /// </summary>
    private Vector2 m_targetOffset = Vector2.zero;

    /// <summary>
    /// Modifies the position of the camera to the given spawnpoint.
    /// </summary>
    /// <param name="spawnPoint">The place at which the camera is attempting to look.</param>
    internal void ResetTo(Transform spawnPoint)
    {
        transform.position = spawnPoint.position;
        transform.position += Vector3.forward * -10;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check for missing a target.
        if (m_target == null)
        {
            return;
        }

        // Capture the new location that the camera is attempting to trace.
        Vector2 targetPosition = new Vector2(m_target.transform.position.x, m_target.transform.position.y);
        m_targetOffset = Vector2.Lerp(m_targetOffset, m_target.velocity * 0.3f + (Vector2)m_target.transform.up * 1.5f, Time.deltaTime * 5);

        // Smoothly modify the position of the camera to the new location.
        Vector2 oldPosition = transform.position;
        transform.position = Vector2.SmoothDamp(transform.position, targetPosition + m_targetOffset, ref m_velocity, .15f);
        transform.position += Vector3.forward * -10;
    }
}