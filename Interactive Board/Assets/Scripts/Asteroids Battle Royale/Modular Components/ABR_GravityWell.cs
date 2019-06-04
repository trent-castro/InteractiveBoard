using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that causes an object to function as a gravity well.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class ABR_GravityWell : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("The pull strength of the gravity well.")]
	[SerializeField]
    private float m_gravityPull = .78f;

	public static float m_gravityRadius = 1f;

	void Awake()
	{
		m_gravityRadius = GetComponent<CircleCollider2D>().radius;
	}

	/// <summary>
	/// Attract objects towards an area when they come within the bounds of a collider.
	/// This function is on the physics timer so it won't necessarily run every frame.
	/// </summary>
	/// <param name="other">Any object within reach of gravity's collider</param>
	void OnTriggerStay2D(Collider2D other)
	{
		if (other.attachedRigidbody && !other.gameObject.CompareTag(ABR_Tags.AsteroidTag))
		{
			float gravityIntensity = Vector3.Distance(transform.position, other.transform.position) / m_gravityRadius;
			other.attachedRigidbody.AddForce((transform.position - other.transform.position) * gravityIntensity * other.attachedRigidbody.mass * m_gravityPull * Time.smoothDeltaTime);
		}
	}
}