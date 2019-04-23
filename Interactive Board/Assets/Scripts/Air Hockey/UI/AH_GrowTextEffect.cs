using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_GrowTextEffect : MonoBehaviour
{
    [SerializeField] float m_desiredScale = 1.0f;
    [SerializeField] float m_timeToReachDesiredScale = 1.0f;
    private float m_currentTime = 0.0f;

    private void Start()
    {
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        m_currentTime += Time.deltaTime;
        if (!(m_currentTime > m_timeToReachDesiredScale))
        {
            float currentscale = m_currentTime * m_desiredScale;
            gameObject.transform.localScale = (Vector3.one * currentscale);
        }
    }
}
