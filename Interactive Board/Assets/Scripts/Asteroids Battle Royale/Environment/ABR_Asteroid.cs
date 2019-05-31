using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ABR_Asteroid : MonoBehaviour
{
	ParticleSystem m_shards = null;

	GameObject m_item;
	public float speed = 3;

	//Controls for perish animation
	float m_perishTimer = 0;
	float m_perishMaxTime = 1.5f;
	Vector3 m_normalScale = new Vector3(5f, 5f, 5f);
	bool isPerishing = false;

	//Decay Timer
	float m_decayTimer = 0;
	float m_decayMaxTime = 30;

	private void Start()
	{
		m_shards = GetComponentInChildren<ParticleSystem>();
	}

	/// <summary>
	/// Resets the aestroid upon regeneration.
	/// </summary>
	private void OnEnable()
	{
		m_decayTimer = 0;
		m_perishTimer = 0;
		transform.localScale = m_normalScale;
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
	/// Releases the item being carried by this object at the current location of the aestroid.
	/// </summary>
	/// <returns>The GameObject this aestroid is holding as a droppable item.</returns>
	public GameObject ReleaseItem()
	{
		if (m_item)
		{
			GameObject item = m_item;
			m_item = null;

			item.transform.position = this.transform.position;
			item.SetActive(true);
			return item;
		}
		return null;
	}

	public void Perish()
	{
		m_shards.Play();
		if (!isPerishing) StartCoroutine("Perishing");
	}

	private IEnumerator Perishing()
	{
		while (m_perishTimer <= m_perishMaxTime)
		{
			isPerishing = true;
			m_perishTimer += Time.deltaTime;
			float t = m_perishTimer / m_perishMaxTime;
			t = Interpolation.SineInOut(t);
			transform.localScale = Vector3.LerpUnclamped(m_normalScale, Vector3.zero, t);
			yield return null;
		}

		this.gameObject.SetActive(false);
		isPerishing = false;
	}
}
