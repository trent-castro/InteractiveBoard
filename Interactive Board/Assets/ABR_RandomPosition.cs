using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_RandomPosition : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Vector describing the x bounds and y bounds of the spawning area")]
	private Vector2 m_spawningArea = Vector2.zero;
	private ParticleSystem m_particleSystem = null;
	[SerializeField] float m_timeBetweenPlays = 0;
	[SerializeField] float m_spawnVariance = 10;
	private float m_timer = 0;

	private void Start()
	{
		m_particleSystem = GetComponent<ParticleSystem>();
	}

	// Update is called once per frame
	void Update()
    {
		m_timer += Time.deltaTime;

		if (m_timer >= m_timeBetweenPlays)
		{
			m_timer = Random.Range(0f, m_spawnVariance);

			float x = Random.Range(-m_spawningArea.x, m_spawningArea.x);
			float y = Random.Range(-m_spawningArea.y, m_spawningArea.y);
			transform.position = new Vector3(x, y, 0.0f);
			if (m_particleSystem) m_particleSystem.Play();
		}
    }
}
