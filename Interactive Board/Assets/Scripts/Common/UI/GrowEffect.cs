using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowEffect : MonoBehaviour
{
    private enum Effect
    {
        BOUNCE_IN_OUT,
        ELASTIC_IN_OUT,
        LINEAR,
        CIRCULAR_IN_OUT
    }

    [SerializeField] float m_desiredScale = 1.0f;
    [SerializeField] float m_timeToReachDesiredScale = 1.0f;
    [SerializeField] float m_timeToDelayStart = 0.0f;
    [SerializeField] Effect m_interpolationEffect = Effect.ELASTIC_IN_OUT;
    private float m_currentTime = 0.0f;

    private void Awake()
    {
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        if (m_timeToDelayStart <= 0.0)
        {
            m_currentTime += Time.deltaTime;
            if (!(m_currentTime > m_timeToReachDesiredScale))
            {
                switch (m_interpolationEffect)
                {
                    case Effect.BOUNCE_IN_OUT:
                        gameObject.transform.localScale = (Vector3.one * Mathf.LerpUnclamped(0, m_desiredScale, Interpolation.BounceInOut(m_currentTime / m_timeToReachDesiredScale)));
                        break;
                    case Effect.ELASTIC_IN_OUT:
                        gameObject.transform.localScale = (Vector3.one * Mathf.LerpUnclamped(0, m_desiredScale, Interpolation.ElasticInOut(m_currentTime / m_timeToReachDesiredScale)));
                        break;
                    case Effect.LINEAR: 
                        gameObject.transform.localScale = (Vector3.one * Mathf.LerpUnclamped(0, m_desiredScale, Interpolation.Linear(m_currentTime / m_timeToReachDesiredScale)));
                        break;
                    case Effect.CIRCULAR_IN_OUT:
                        gameObject.transform.localScale = (Vector3.one * Mathf.LerpUnclamped(0, m_desiredScale, Interpolation.CircularInOut(m_currentTime / m_timeToReachDesiredScale)));
                        break;
                }
            }
        }
        else
            m_timeToDelayStart -= Time.deltaTime;
    }
}
