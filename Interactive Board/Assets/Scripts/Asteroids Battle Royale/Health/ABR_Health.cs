﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABR_Health : MonoBehaviour
{
	[SerializeField] float m_health = 0;
	[SerializeField] float m_maxHealth = 0;
	[SerializeField] protected bool m_isAlive = true;
    [Tooltip("Duration of invincibility")]
    [SerializeField]
    private float m_invincibilityDuration = 0.0f;
    [Tooltip("Invincibility duration delay")]
    [SerializeField]
    private float m_invincibilityDurationDelay = 0.0f;

    protected bool isInvincible = false;
    protected bool invincibilityCoroutineHasStarted = false;


    public float GetHealth()
	{
		return m_health;
	}

	public float GetMaxHealth()
	{
		return m_maxHealth;
	}

	public void InitializeHealth(float health, float maxHealth)
	{
		m_health = health;
		m_maxHealth = maxHealth;
		m_isAlive = true;
	}

	public void Respawn()
	{
		m_isAlive = true;
		m_health = m_maxHealth;
	}
    protected virtual void Die()
    {
        StopCoroutine(Invincibility());
    }

	public virtual void TakeDamage(float damage)
	{
        if (!isInvincible)
        {
            m_health -= damage;
            if (m_health <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(Invincibility());
            }
        }
	}

    protected IEnumerator Invincibility()
    {
        if (!invincibilityCoroutineHasStarted)
        {
            invincibilityCoroutineHasStarted = true;
            yield return new WaitForSeconds(m_invincibilityDurationDelay);
            isInvincible = true;
            yield return new WaitForSeconds(m_invincibilityDuration);
            isInvincible = false;
            invincibilityCoroutineHasStarted = false;
        }
    }
}