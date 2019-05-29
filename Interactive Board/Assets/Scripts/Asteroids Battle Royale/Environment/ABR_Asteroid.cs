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
	GameObject m_item;
	[SerializeField] Camera boy = null;
	ParticleSystem m_shards = null;
	Rigidbody2D m_rb = null;
	public float speed = 3;
	bool isMoving = false;


	//Controls for perish animation
	float m_perishTimer = 0;
	float m_perishMaxTime = 1.5f;
	Vector3 m_normalScale = Vector2.zero;

	float m_decayTimer = 0;
	float m_decayMaxTime = 30;

	private void Start()
	{
		m_normalScale = transform.localScale;
		if (isMoving)
		{
			m_startLocation = new Vector2(Random.Range(-c_maxXDirection, c_maxXDirection), c_maxYDirection);
			m_endLocation = new Vector2(Random.Range(-c_maxXDirection, c_maxXDirection), -c_maxYDirection);
			m_rb = GetComponent<Rigidbody2D>();
			m_rb.velocity = (m_startLocation * m_endLocation).normalized * speed;
			velocity = m_rb.velocity;
			transform.position = m_startLocation;
		}
		m_shards = GetComponentInChildren<ParticleSystem>();
	}

	private void OnEnable()
	{
		m_decayTimer = 0;
	}

	private void Update()
	{
		m_decayTimer += Time.deltaTime;

		if (m_decayTimer >= m_decayMaxTime)
		{
			Perish();
		}
	}

	/// <summary>
	/// Adds an item to this object that it will carry around until broken
	/// </summary>
	/// <param name="item"></param>
	public void AddItem(GameObject item)
	{
		m_item = item;
		m_item.transform.localPosition = Vector3.zero;
		m_item.SetActive(false);
	}

	/// <summary>
	/// Releases the item being carried by this object
	/// </summary>
	/// <returns></returns>
	public GameObject ReleaseItem()
	{
		if (m_item)
		{
			m_item.transform.position = this.transform.position;
			m_item.SetActive(true);
			GameObject item = m_item;
			m_item = null;
			return item;
		}
		return null;
	}

	/// <summary>
	/// Sets a target and start location for the object to be moved between via rigidbody velocity.
	/// </summary>
	/// <param name="currentLocation">The starting location of the object.</param>
	/// <param name="targetLocation">The intended end location of the object.</param>
	public void Target(Vector2 currentLocation, Vector2 targetLocation)
	{
		if (isMoving)
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
	}

	public void Perish()
	{
		m_shards.Play();
		m_perishTimer = 0;
		StartCoroutine("Perishing");
	}

	private IEnumerator Perishing()
	{
		while (m_perishTimer <= m_perishMaxTime)
		{
			
			m_perishTimer += Time.deltaTime;
			float t = m_perishTimer / m_perishMaxTime;
			t = Interpolation.SineInOut(t);
			transform.localScale = Vector3.LerpUnclamped(m_normalScale, Vector3.zero, t);
			yield return null;
		}

		this.gameObject.SetActive(false);
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (isMoving && collision.gameObject.CompareTag(ABR_Tags.WallTag))
		{
			Target(new Vector2(boy.ScreenToWorldPoint(Input.mousePosition).x, c_maxYDirection), new Vector2(boy.ScreenToWorldPoint(Input.mousePosition).x, -c_maxYDirection));
		}
	}
}
