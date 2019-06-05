using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract Bullet Class, This has all the basic behaviour for a bullet:
/// - Bullet can fire
/// - Bullet will move in a constant direction
/// - Bullet disables itself when it collides with something
/// - if a bullet collides with something that has a health component it does it's damage
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract class ABR_Bullet : MonoBehaviour
{
    [Header("Base Bullet Configuration")]
    [Tooltip("How long it will take for the bullets to time out")]
    [SerializeField] protected float m_lifeTime = 1.0f;
    [Tooltip("The Speed at which the bullets are traveling  ")]
    [SerializeField] protected float m_speed = 1.0f;
    [Tooltip("The amount of damage a bullet will do on collision")]
    [SerializeField] protected float m_damage = 10.0f;
    [Header("Debug")]
    [SerializeField] protected bool DebugMode = false;

    protected float m_elapsedLifeTime = 0.0f;

    //Sibling components
    protected Rigidbody2D m_rigidbody = null;

    private void Awake()
    {
        GetSiblingComponents();
    }

    //Sets references to the private sibling components needed in this Class
    private void GetSiblingComponents()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //increase elapsed lifetime by delta time
        m_elapsedLifeTime += Time.deltaTime;
        //if elapsed time is greater than the lifetime disalbe this object
        if (m_elapsedLifeTime >= m_lifeTime)
            gameObject.SetActive(false);

    }
    private void OnDisable()
    {
        //sets the elapsed lifetime backt to 0;
        m_elapsedLifeTime = 0.0f;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
		if (!collision.gameObject.CompareTag(ABR_Tags.BulletTag) && !collision.gameObject.CompareTag(ABR_Tags.ShipCollisionTag)
            && !collision.CompareTag(ABR_Tags.WallTag) && !collision.CompareTag(ABR_Tags.GravityTag) && !collision.CompareTag(ABR_Tags.WeaponTag))
		{
            //Disable this gameObject 
			gameObject.SetActive(false);
            //get the health component in the collision
			ABR_Health health = collision.GetComponent<ABR_Health>();
            //if the health component exists continue
			if (health)
			{
                //Deal the damage variable to the health component
				health.TakeDamage(m_damage);
			}
		}
    }

    /// <summary>
    /// Sets the bullets movement direction to the passed in direction
    /// </summary>
    /// <param name="direction">desired movement direction</param>
    abstract public void Fire(Vector2 direction, Vector2 shipVelocity);

}
