using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ABR_Asteroid : MonoBehaviour
{
	const float c_maxXDirection = 130;
	const float c_maxYDirection = 48;
	[SerializeField] Vector2 m_startLocation;
	[SerializeField] Vector2 m_endLocation;
	[SerializeField] Vector2 velocity;
	[SerializeField] Camera boy;
	ParticleSystem m_shards;
	Rigidbody2D m_rb;
	float speed = 3;
	[Range(0, 100)] int health;

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
			//m_startLocation = new Vector2(Random.Range(-c_maxXDirection, c_maxXDirection), c_maxYDirection);
			m_startLocation = new Vector2(Mathf.Clamp(boy.ScreenToWorldPoint(Input.mousePosition).x, -c_maxXDirection, c_maxXDirection), c_maxYDirection);
			//m_endLocation = new Vector2(Random.Range(-c_maxXDirection, c_maxXDirection), -c_maxYDirection);
			m_endLocation = new Vector2(boy.ScreenToWorldPoint(Input.mousePosition).x, -c_maxYDirection);
			m_rb.angularVelocity = 0;
			m_rb.velocity = (m_startLocation - m_endLocation) * speed;
			m_rb.velocity = new Vector2(Mathf.Clamp(m_rb.velocity.x, -10, 10), Mathf.Clamp(m_rb.velocity.y, -10, 10));
			transform.position = m_startLocation;
		}
	}
}
