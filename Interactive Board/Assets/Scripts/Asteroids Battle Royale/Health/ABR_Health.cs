using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABR_Health : MonoBehaviour
{
    [Header("Configration")]
    [Tooltip("The current health of the object.")]
	[SerializeField]
    protected float m_health = 100;
    [Tooltip("The maximum health of the object.")]
	[SerializeField]
    protected float m_maxHealth = 100;
    [Tooltip("A bool that marks whether or not the object is currently considered dead in the game.")]
	[SerializeField]
    public bool m_isAlive = false;
    [Tooltip("The duration of invincibility.")]
    [SerializeField]
    private float m_invincibilityDuration = 0.75f;
    [Tooltip("The delay before an object becomes invincible after taking damage.")]
    [SerializeField]
    private float m_invincibilityDurationDelay = 0.5f;
    [Tooltip("The sprite that renders while the object is invincible.")]
    [SerializeField]
    private GameObject m_invincibilitySprite = null;

    // Public delegate functions
    /// <summary>
    /// A delegate to handle the death of an object.
    /// </summary>
	public delegate void DeathAction();
    /// <summary>
    /// An event that is used to broadcast the death of an object.
    /// </summary>
	public event DeathAction OnDeath;

    // Protected internal data members
    /// <summary>
    /// Whether or not the object is able to take damage.
    /// </summary>
    protected bool isInvincible = false;
    /// <summary>
    /// Whether or the coroutine that handles invincibility is currently runing or not.
    /// </summary>
    protected bool invincibilityCoroutineHasStarted = false;

    // Private Data members
    /// <summary>
    /// The scale of the invincibility sprite before it is affected by anything.
    /// </summary>
    private Vector3 invincibiliySpriteNormalScale = new Vector3();

    /// <summary>
    /// Public method to access information about the current health of an object.
    /// </summary>
    /// <returns>A float that describes the current health of an object.</returns>
    public float GetHealth()
	{
		return m_health;
	}

    /// <summary>
    /// Public method to access information about the maximum health of an object.
    /// </summary>
    /// <returns>A float that describes the maximum amount of health for an object.</returns>
	public float GetMaxHealth()
	{
		return m_maxHealth;
	}

    /// <summary>
    /// Initializes the health of the object.
    /// </summary>
    /// <param name="health">The desired starting off current health.</param>
    /// <param name="maxHealth">The desired starting off maximum health.</param>
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

    /// <summary>
    /// Resets the internal values for the object's health.
    /// </summary>
	public virtual void Respawn()
	{
		m_isAlive = true;
		m_health = m_maxHealth;
	}
    
    /// <summary>
    /// Causes the object to trigger an on death method as well as stops the object from becoming invincible.
    /// </summary>
    protected virtual void Die()
    {
        m_isAlive = false;
        OnDeath?.Invoke();
        StopCoroutine(Invincibility());
    }

    /// <summary>
    /// A method that allows an object to take a certain amount of damage.
    /// </summary>
    /// <param name="damage">The amount of current health the object will be losing.</param>
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

    /// <summary>
    /// Begins the invincibility coroutine.
    /// </summary>
    /// <returns></returns>
    protected IEnumerator Invincibility()
    {
        if (!invincibilityCoroutineHasStarted)
        {
            invincibilityCoroutineHasStarted = true;
            // start invincible sprite here if the animation works
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

    /// <summary>
    /// A coroutine that displays the force field of an object.
    /// </summary>
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

    /// <summary>
    /// A coroutine that retracts the force field of an object.
    /// </summary>
    /// <returns></returns>
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