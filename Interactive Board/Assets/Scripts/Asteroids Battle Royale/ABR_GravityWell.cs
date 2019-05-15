using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
public class ABR_GravityWell : MonoBehaviour
{
	[SerializeField] public float GRAVITY_PULL = .78f;
	public static float m_GravityRadius = 1f;
	void Awake()
	{
		m_GravityRadius = GetComponent<CircleCollider2D>().radius;
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
			float gravityIntensity = Vector3.Distance(transform.position, other.transform.position) / m_GravityRadius;
			other.attachedRigidbody.AddForce((transform.position - other.transform.position) * gravityIntensity * other.attachedRigidbody.mass * GRAVITY_PULL * Time.smoothDeltaTime);
			//Debug.DrawRay(other.transform.position, transform.position - other.transform.position);
		}
	}
}