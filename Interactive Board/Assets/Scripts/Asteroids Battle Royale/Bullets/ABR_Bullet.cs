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

    protected Vector2 movementDirection = Vector2.zero;
    protected float m_elapsedLifeTime = 0.0f;

    private void Update()
    {
        m_elapsedLifeTime += Time.deltaTime;
        if (m_elapsedLifeTime >= m_lifeTime)
            gameObject.SetActive(false);

        gameObject.transform.position += (Vector3)(movementDirection * m_speed);
        Debug.Log("Bullet Update");
    }
    private void OnDisable()
    {
        movementDirection = Vector2.zero;
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
    abstract public void Fire(Vector2 direction);

}
