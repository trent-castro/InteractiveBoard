using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterGrowEffect : MonoBehaviour
{
    [SerializeField] float m_desiredScale = 1.0f;
    [SerializeField] float m_timeToReachDesiredScale = 1.0f;
    [SerializeField] float m_rotationSpeed = 2.0f;
    private float m_currentTime = 0.0f;
    private Quaternion startingRotation = Quaternion.identity;

    private void Awake()
    {
        transform.localScale = Vector3.zero;
        startingRotation = transform.rotation;
    }

    void Update()
    {
        m_currentTime += Time.deltaTime;
        if (m_currentTime <= m_timeToReachDesiredScale)
        {
            gameObject.transform.Rotate(Vector3.forward, (10 * (m_rotationSpeed * Time.deltaTime)));
            gameObject.transform.localScale = (Vector3.one * Mathf.LerpUnclamped(0, m_desiredScale, Interpolation.ElasticInOut(m_currentTime/m_timeToReachDesiredScale)));
            return;
        }
        else
        {
            gameObject.transform.rotation = startingRotation;
        }
    }
}
