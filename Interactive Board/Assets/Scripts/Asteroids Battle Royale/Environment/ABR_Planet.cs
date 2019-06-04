using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script for planets that will orbit a sun.
/// </summary>
public class ABR_Planet : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("The speed of an orbit around the sun")]
	[SerializeField]
    private float OrbitSpeed = .1f;
    [Tooltip("The offset on the x axis from the original position from which a planet will orbit, passing through the x limits.")]
	[SerializeField]
    private float xAxis = 5;
    [Tooltip("The offset on the y axis from the original position from which a planet will orbit, passing through the y limits.")]
    [SerializeField]
    private float yAxis = 10;

    // Private internal data members
    /// <summary>
    /// A tracker for the center of the original transform position.
    /// </summary>
	private Vector2 m_center = Vector2.zero;
    /// <summary>
    /// A tracker for the current angle of rotation, modulated by 360.
    /// </summary>
	private float m_angle = 0f;
    /// <summary>
    /// The offset multiplier of the transform's position.
    /// </summary>
    private float offsetMultiplier = 25.0f;

	private void Start()
	{
		m_center = transform.position;
	}

	private void Update()
	{
        UpdateAngle();
        UpdatePosition();
	}

    /// <summary>
    /// Updates the rotation to describe where in the eliptical path a planet is during orbit.
    /// </summary>
    private void UpdateAngle()
    {
        m_angle += OrbitSpeed * Time.deltaTime;
        if (m_angle > 360) m_angle = m_angle % 360;
    }

    /// <summary>
    /// Updates the position of the planet.
    /// </summary>
    private void UpdatePosition()
    {
        var offset = Evaluate();
        transform.position = m_center + offset * offsetMultiplier;
    }

    /// <summary>
    /// Evaluates the new offset position of the planet after taking into account the time as well as the current position along the eliptical orbit of the planet.
    /// </summary>
    /// <returns>A 2D vector describing the offset location of the ship based on time and angle.</returns>
	public Vector2 Evaluate()
	{
		float angle = Mathf.Deg2Rad * 360 * m_angle;
		float x = Mathf.Sin(angle) * xAxis;
		float y = Mathf.Cos(angle) * yAxis;

		return new Vector2(x, y);
	}
}