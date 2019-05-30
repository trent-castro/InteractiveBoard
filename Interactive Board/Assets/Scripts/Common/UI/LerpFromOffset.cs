using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpFromOffset : MonoBehaviour
{

    private enum Effect
    {
        BOUNCE_OUT,
        ELASTIC_OUT,
        LINEAR,
        CIRCULAR_OUT
    }

    [SerializeField] Vector3 m_offsetDirection = default;
    [SerializeField] Effect m_effect = default;
    [SerializeField] float m_offsetDistance = 1.0f;
    [SerializeField] float m_timeToReachPosition = 1.5f;

    private Vector3 m_offset = Vector3.zero;
    private Vector3 m_desiredPos = Vector3.zero;
    private float m_currentTime = 0.0f;

    void Awake()
    {
        m_offset = m_offsetDirection.normalized * m_offsetDistance;
        m_desiredPos = transform.position;
        transform.position = m_desiredPos + m_offset;
    }

    void Update()
    {
        m_currentTime += Time.deltaTime;
        if (m_currentTime < m_timeToReachPosition)
        {
            switch (m_effect)
            {
                case Effect.BOUNCE_OUT:
                    transform.position = Vector3.LerpUnclamped(m_desiredPos + m_offset, m_desiredPos, Interpolation.BounceOut(m_currentTime / m_timeToReachPosition));
                    break;
                case Effect.ELASTIC_OUT:
                    transform.position = Vector3.LerpUnclamped(m_desiredPos + m_offset, m_desiredPos, Interpolation.ElasticOut(m_currentTime / m_timeToReachPosition));
                    break;
                case Effect.LINEAR:
                    transform.position = Vector3.LerpUnclamped(m_desiredPos + m_offset, m_desiredPos, Interpolation.Linear(m_currentTime / m_timeToReachPosition));
                    break;
                case Effect.CIRCULAR_OUT:
                    transform.position = Vector3.LerpUnclamped(m_desiredPos + m_offset, m_desiredPos, Interpolation.CircularOut(m_currentTime / m_timeToReachPosition));
                    break;
            }

            return;
        }
        transform.position = m_desiredPos;
    }
}
