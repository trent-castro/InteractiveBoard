using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_Puck : MonoBehaviour
{
    [SerializeField] [Tooltip("Clamps the maximum speed for the puck.")]
    private float m_maxSpeed = 30;
    [SerializeField] 
    GameObject[] m_hitParticle = null;

    // ASK SEN ABOUT OBJECT POOLING RIGHT HERE
    public bool delete = false;

	[SerializeField] SpriteRenderer m_image;

    // Private Sibling Components
    private Rigidbody2D rigidBody;

    private void Start()
    {
        GetSiblingComponents();
        Physics2D.IgnoreLayerCollision(8,9, true); 
    }

    /// <summary>
    /// Sets references to sibling components
    /// </summary>
    private void GetSiblingComponents()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

	public void SetImageActive(bool active)
	{
		m_image.enabled = active;
	}

    private void Update()
    {
        if (rigidBody.velocity != Vector2.zero)
        {
            float magnitude = rigidBody.velocity.magnitude;
            if (magnitude > m_maxSpeed)
            {
                rigidBody.velocity = (rigidBody.velocity / magnitude) * m_maxSpeed;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_hitParticle[0].transform.position = collision.contacts[0].point;
        m_hitParticle[0].GetComponent<ParticleSystem>().Play();
    }

    /// <summary>
    /// Modifies the maximum speed of the puck.
    /// </summary>
    /// <param name="newMaxSpeed">The new max speed.</param>
    public void SetMaxSpeed(float newMaxSpeed)
    {
        m_maxSpeed = newMaxSpeed;
    }

    /// <summary>
    /// Returns the maximum speed of the puck.
    /// </summary>
    /// <returns>The max speed of the puck.</returns>
    public float GetMaxSpeed()
    {
        return m_maxSpeed;
    }
    
    public void ResetPuck()
    {
        transform.position = Vector3.zero;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponentInChildren<TrailRenderer>().enabled = true;
        if(delete)
        {
            Destroy(this);
        }
    }
}