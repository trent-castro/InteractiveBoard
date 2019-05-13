using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABR_Health : MonoBehaviour
{
	[SerializeField] float m_health = 0;
	[SerializeField] float m_maxHealth = 0;
	[SerializeField] bool m_isAlive = true;
	[SerializeField] float m_touchDamage = 0;

    // Start is called before the first frame update
    void Awake()
    {
    }

	public void Init(float health, float maxHealth)
	{
		m_health = health;
		m_maxHealth = maxHealth;
		m_isAlive = true;

	}

	public void Death()
	{
		//Perish
	}

	public void OnHit(float damage)
	{
		//DO a thing
	}
}
