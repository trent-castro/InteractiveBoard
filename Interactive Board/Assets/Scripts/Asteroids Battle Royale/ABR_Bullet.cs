using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_Bullet : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("How long it will take for the bullets to time out")]
    [SerializeField] float m_lifeTime = 10.0f;
    [Tooltip("The Speed at which the bullets are traveling  ")]
    [SerializeField] float m_speed = 5.0f;
    [Header("Debug")]
    [SerializeField] bool DebugMode = false;

    private Vector2 movementDirection = Vector2.zero;
    void Update()
    {
        m_lifeTime -= Time.deltaTime;
        if (m_lifeTime <= 0.0f)
            gameObject.SetActive(false);
         
        gameObject.transform.position += (Vector3)(movementDirection * m_speed);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO put damage and or hit detection logic here
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the bullets movement direction to the passed in direction
    /// </summary>
    /// <param name="direction">desired movement direction</param>
    public void Fire(Vector2 direction)
    {
        movementDirection = direction;
    }
}
