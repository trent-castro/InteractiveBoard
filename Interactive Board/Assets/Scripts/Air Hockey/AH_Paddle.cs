using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AH_Paddle : MonoBehaviour
{
    [Header("Animation")]
    [Tooltip("The amount of time it takes for the animation to complete")]
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float m_spawnAnimationTime = 0.5f;
    [Tooltip("The starting scale of the pick up object upon beginning the animation")]
    [SerializeField]
    private Vector3 m_startingScale = new Vector3(3, 3, 3);

    private float m_animationTimer = 0.0f;



    private AudioSource m_audioSource = null;
    private Collider2D m_collider2D;


    private void Start()
    {
        if(m_audioSource == null)
        {
            m_audioSource = GetComponent<AudioSource>();
        }
        m_collider2D = GetComponent<CircleCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_audioSource.Play();
    }

    public void StartDropAnimation()
    {
        StartCoroutine(StartAnimationCoroutine());
    }

    public IEnumerator StartAnimationCoroutine()
    {
        m_collider2D.enabled = false;
        while (m_animationTimer < m_spawnAnimationTime)
        {
            m_animationTimer += Time.deltaTime;
            float t = m_animationTimer / m_spawnAnimationTime;
            t = Interpolation.BounceOut(t);
            transform.localScale = Vector3.LerpUnclamped(m_startingScale, Vector3.one, t);
            yield return null;
        }
        m_collider2D.enabled = true;
    }
}
