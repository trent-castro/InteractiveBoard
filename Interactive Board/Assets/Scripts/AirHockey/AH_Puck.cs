using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_Puck : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Clamps the maximum speed for the puck.")]
    private float m_maxSpeed = 30;
    [SerializeField]
    GameObject[] m_hitParticle = null;
    [SerializeField]
    AudioClip[] audioClips = null;

    // ASK SEN ABOUT OBJECT POOLING RIGHT HERE
    public bool delete = false;
    private AudioSource m_audioSource;


    // Private Sibling Components
    private Rigidbody2D rigidBody;
    private SpriteRenderer m_spriteRenderer;

    private void Start()
    {
        GetSiblingComponents();
        Physics2D.IgnoreLayerCollision(8, 9, true);
    }

    /// <summary>
    /// Sets references to sibling components
    /// </summary>
    private void GetSiblingComponents()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        m_audioSource = GetComponent<AudioSource>();
        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void SetImageActive(bool active)
    {
        m_spriteRenderer.enabled = active;
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
        DetermineRandomSoundToPlay();
        m_audioSource.Play();
        Debug.Log("Collided with [" + collision.transform.name + "] " +
            "which is a " + collision.gameObject.tag);

        GameObject particleSystemObject = AH_ParticlePools.instance.GetTaggedParticleSystem(collision.gameObject.tag);
        ParticleSystem particleSystem = particleSystemObject.GetComponent<ParticleSystem>();
        if (particleSystem)
        {
            print("Particle found");
            print(particleSystem);
            particleSystem.transform.position = collision.contacts[0].point;
            particleSystem.Play();
        }
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
        if(this)
        {
            transform.position = Vector3.zero;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponentInChildren<TrailRenderer>().enabled = true;
            if (delete)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void DetermineRandomSoundToPlay()
    {
        int num = Random.Range(0, audioClips.Length);
        m_audioSource.clip = audioClips[num];
    }
}