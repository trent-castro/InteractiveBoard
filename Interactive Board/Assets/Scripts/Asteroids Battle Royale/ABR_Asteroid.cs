using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ABR_Asteroid : MonoBehaviour
{
	const float c_maxXDirection = 130;
	const float c_maxYDirection = 48;
	[SerializeField] Vector2 m_startLocation = Vector2.zero;
	[SerializeField] Vector2 m_endLocation = Vector2.zero;
	[SerializeField] Vector2 velocity = Vector2.zero;
	[SerializeField] Camera boy = null;
	ParticleSystem m_shards = null;
	Rigidbody2D m_rb = null;
	float speed = 3;
	[Range(0, 100)] int health = 100;

	private void Start()
	{
		m_startLocation = new Vector2(Random.Range(-c_maxXDirection, c_maxXDirection),c_maxYDirection);
		m_endLocation = new Vector2(Random.Range(-c_maxXDirection, c_maxXDirection),-c_maxYDirection);
		m_rb = GetComponent<Rigidbody2D>();
		m_rb.velocity = (m_startLocation * m_endLocation).normalized * speed;
		velocity = m_rb.velocity;
		transform.position = m_startLocation;
		m_shards = GetComponentInChildren<ParticleSystem>();
	}

	/// <summary>
	/// Sets a target and start location for the object to be moved between via rigidbody velocity.
	/// </summary>
	/// <param name="currentLocation">The starting location of the object.</param>
	/// <param name="targetLocation">The intended end location of the object.</param>
	public void Target(Vector2 currentLocation, Vector2 targetLocation)
	{
		//Clamps Target to playable area
		currentLocation = new Vector2(Mathf.Clamp(currentLocation.x, -c_maxXDirection, c_maxXDirection), Mathf.Clamp(currentLocation.y, -c_maxYDirection, c_maxYDirection));
		targetLocation = new Vector2(Mathf.Clamp(targetLocation.x, -c_maxXDirection, c_maxXDirection), Mathf.Clamp(targetLocation.y, -c_maxYDirection, c_maxYDirection));

		m_startLocation = currentLocation;
		m_endLocation = targetLocation;

		//Sets velocity as a linear path between the start and end locations
		m_rb.velocity = (m_endLocation - m_startLocation) * speed;
		m_rb.velocity = new Vector2(Mathf.Clamp(m_rb.velocity.x, -10, 10), Mathf.Clamp(m_rb.velocity.y, -10, 10));

		//Resets other factors of object
		m_rb.angularVelocity = 0;
		transform.position = m_startLocation;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		m_shards.Play();
		if (collision.gameObject.CompareTag("Bullet"))
		{
			health--;
			if (health <= 0)
			{
				OnKillObject();
			}
		}
	}

	void OnKillObject()
	{
		//Reset Object
		//Return Object to Pool
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Wall")
		{
			m_startLocation = new Vector2(Mathf.Clamp(boy.ScreenToWorldPoint(Input.mousePosition).x, -c_maxXDirection, c_maxXDirection), c_maxYDirection);
			m_endLocation   = new Vector2(Mathf.Clamp(boy.ScreenToWorldPoint(Input.mousePosition).x, -c_maxXDirection, c_maxXDirection), -c_maxYDirection);
			m_rb.angularVelocity = 0;
			m_rb.velocity = ( m_endLocation - m_startLocation) * speed;
			m_rb.velocity = new Vector2(Mathf.Clamp(m_rb.velocity.x, -10, 10), Mathf.Clamp(m_rb.velocity.y, -10, 10));
			transform.position = m_startLocation;
		}
	}
}
