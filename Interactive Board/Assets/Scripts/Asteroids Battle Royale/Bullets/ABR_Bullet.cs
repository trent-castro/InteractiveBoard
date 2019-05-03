using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABR_Bullet : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("How long it will take for the bullets to time out")]
    [SerializeField] protected float m_lifeTime = 1.0f;
    [Tooltip("The Speed at which the bullets are traveling  ")]
    [SerializeField] protected float m_speed = 1.0f;
    [Tooltip("The amount of damage a bullet will do on collision")]
    [SerializeField] protected float m_damage = 10.0f;
    [Header("Debug")]
    [SerializeField] protected bool DebugMode = false;

    protected float m_elapsedLifeTime = 0.0f;
    protected Rigidbody2D m_rigidbody = null;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        m_elapsedLifeTime += Time.deltaTime;
        if (m_elapsedLifeTime >= m_lifeTime)
            gameObject.SetActive(false);

    }
    private void OnDisable()
    {
        m_elapsedLifeTime = 0.0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the bullets movement direction to the passed in direction
    /// </summary>
    /// <param name="direction">desired movement direction</param>
    abstract public void Fire(Vector2 direction, Vector2 shipVelocity);

}
