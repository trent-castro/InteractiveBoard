using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_Puck : MonoBehaviour
{
    [SerializeField]
    float m_maxSpeed = 30;
    [SerializeField]
    GameObject[] m_hitParticle = null;
    [SerializeField]
    AudioClip[] audioClips = null;

    private Rigidbody2D rgdbody;
    private AudioSource m_audioSource;

    private void Start()
    {
        rgdbody = gameObject.GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(8,9, true);
        m_audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //This clamps the Puck Speed to 30
        if (rgdbody.velocity != Vector2.zero)
        {
            float magnitude = rgdbody.velocity.magnitude;
            if (magnitude > m_maxSpeed)
            {
                rgdbody.velocity = (rgdbody.velocity / magnitude) * m_maxSpeed;

            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DetermineRandomSoundToPlay();
        m_audioSource.Play();
        m_hitParticle[0].transform.position = collision.contacts[0].point;
        m_hitParticle[0].GetComponent<ParticleSystem>().Play();
    }

    private void DetermineRandomSoundToPlay()
    {
        int num = Random.Range(0, audioClips.Length);
        m_audioSource.clip = audioClips[num];
    }
}
