using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABR_Health : MonoBehaviour
{
	[SerializeField] float m_health = 100;
	[SerializeField] float m_maxHealth = 100;
	[SerializeField] public bool m_isAlive = false;
    [Tooltip("Duration of invincibility")]
    [SerializeField]
    private float m_invincibilityDuration =0.75f;
    [Tooltip("Invincibility duration delay")]
    [SerializeField]
    private float m_invincibilityDurationDelay = 0.5f;
    [Tooltip("Invincibility Sprite")]
    [SerializeField]
    private GameObject m_invincibilitySprite = null;

    protected bool isInvincible = false;
    protected bool invincibilityCoroutineHasStarted = false;

	public delegate void DeathAction();
	public event DeathAction OnDeath;

    private Vector3 invincibiliySpriteNormalScale = new Vector3();

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
        if(m_invincibilitySprite)
        {
            invincibiliySpriteNormalScale = m_invincibilitySprite.transform.localScale;
        }
	}

	public virtual void Respawn()
	{
		m_isAlive = true;
		m_health = m_maxHealth;
	}
    protected virtual void Die()
    {
        OnDeath?.Invoke();
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
            // start coronary heart exam here
            yield return new WaitForSeconds(m_invincibilityDurationDelay);
            isInvincible = true;
            if (m_invincibilitySprite)
            {
                m_invincibilitySprite.SetActive(true);
                //StartCoroutine(ProjectForceField());
            }
            yield return new WaitForSeconds(m_invincibilityDuration);
            isInvincible = false;
            invincibilityCoroutineHasStarted = false;
            if (m_invincibilitySprite)
            {
                m_invincibilitySprite.SetActive(false);
                //StartCoroutine(RetractForceField());
            }
        }
    }

    protected IEnumerator ProjectForceField()
    {
        float timer = 0;
        while(timer <= m_invincibilityDurationDelay)
        {
            timer += Time.deltaTime;
            float t = timer / m_invincibilityDurationDelay;
            t = Interpolation.Linear(t);
            m_invincibilitySprite.transform.localScale = Vector3.LerpUnclamped(Vector3.zero, invincibiliySpriteNormalScale, t);
            yield return null;
        }
    }

    protected IEnumerator RetractForceField()
    {
        float timer = 0;
        while (timer <= 0.5f)
        {
            timer += Time.deltaTime;
            float t = timer / 0.5f;
            t = Interpolation.Linear(t); 
            m_invincibilitySprite.transform.localScale = Vector3.LerpUnclamped(invincibiliySpriteNormalScale, Vector3.zero, t);
            yield return null;
        }
    }
}