using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterGrowTextEffect : MonoBehaviour
{
    [SerializeField] float m_desiredScale = 1.0f;
    [SerializeField] float m_timeToReachDesiredScale = 1.0f;
    [SerializeField] float m_numOfRotations = 2.0f;
    private float m_currentTime = 0.0f;
    private Vector3 startingRotation = Vector3.zero;
    private float rotationAmount = 0.0f;

    private void Start()
    {
        transform.localScale = Vector3.zero;
        rotationAmount = (360.0f * m_numOfRotations);
        Vector3 rotationVec = Vector3.forward * rotationAmount;
        print(rotationVec);
        gameObject.transform.localEulerAngles = rotationVec;
    }

    void Update()
    {
        m_currentTime += Time.deltaTime;
        if (!(m_currentTime > m_timeToReachDesiredScale))
        {
            rotationAmount = Mathf.LerpUnclamped(0, startingRotation.z, Interpolation.ElasticInOut(m_currentTime / m_timeToReachDesiredScale));
            print(rotationAmount);
            //gameObject.transform.eulerAngles = rotationAmount;
            gameObject.transform.localScale = (Vector3.one * Mathf.LerpUnclamped(0, m_desiredScale, Interpolation.ElasticInOut(m_currentTime/m_timeToReachDesiredScale)));
        }
    }
}
