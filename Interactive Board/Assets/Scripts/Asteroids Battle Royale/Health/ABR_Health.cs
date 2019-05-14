using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABR_Health : MonoBehaviour
{
	[SerializeField] float m_health = 0;
	[SerializeField] float m_maxHealth = 0;
	[SerializeField] bool m_isAlive = true;

    // Start is called before the first frame update
    void Awake()
    {
    }

	public void InitializeHealth(float health, float maxHealth)
	{
		m_health = health;
		m_maxHealth = maxHealth;
		m_isAlive = true;
	}

    protected abstract void Die();

	public void TakeDamage(float damage)
	{
        m_health -= damage;
        if (m_health < 0)
        {
            Die();
        }
	}
}
