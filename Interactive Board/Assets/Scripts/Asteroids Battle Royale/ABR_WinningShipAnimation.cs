using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_WinningShipAnimation : MonoBehaviour
{
    [Header("Flying accross Animation Config")]
    [SerializeField] Vector3 m_FlyingAcrossOffsetDirection = default;
    [SerializeField] float m_FlyingAcrossOffsetDistance = 1.0f;
    [SerializeField] float m_FlyingAcrossTimeToReachPosition = 1.5f;

    [Header("Flying in Animation Config")]
    [SerializeField] Transform m_flyingInDesiredPos = default;
    [SerializeField] Vector3 m_flyingInOffsetDirection = default;
    [SerializeField] float m_flyingInOffsetDistance = 1.0f;
    [SerializeField] float m_FlyingInTimeToReachPosition = 1.5f;
    [SerializeField] float m_FlyingInStartDelay = 0.5f;

    [Header("Other Config")]
    [SerializeField] float m_DelayStart = 1.5f;

    private Vector3 m_offset = Vector3.zero;
    private Vector3 m_desiredPos = Vector3.zero;
    private float m_currentTime = 0.0f;

    void Awake()
    {
        m_offset = m_FlyingAcrossOffsetDirection.normalized * m_FlyingAcrossOffsetDistance;
        m_desiredPos = transform.position;
        transform.position = m_desiredPos + m_offset;
        StartCoroutine(FlyAccrossAnimation());
    }


    IEnumerator FlyAccrossAnimation()
    {
        yield return new WaitForSeconds(m_DelayStart);
        while(m_currentTime < m_FlyingAcrossTimeToReachPosition)
        {
            transform.position = Vector3.LerpUnclamped(m_desiredPos + m_offset, m_desiredPos, Interpolation.CircularIn(m_currentTime / m_FlyingAcrossTimeToReachPosition));
            m_currentTime += Time.deltaTime;
            yield return null;
        }
        transform.position = m_desiredPos;
        m_currentTime = 0.0f;
        SetShipToNewLocation();
    }

    private void SetShipToNewLocation()
    {
        m_offset = m_flyingInOffsetDirection.normalized * m_flyingInOffsetDistance;
        transform.position = m_flyingInDesiredPos.position + m_offset;
        m_desiredPos = m_flyingInDesiredPos.position;
        transform.localEulerAngles = new Vector3(0, 0, 25);
        transform.localScale = new Vector3(.66f, .66f, 1.0f);
        StartCoroutine(FlyInAnimation());
    }

    IEnumerator FlyInAnimation()
    {
        yield return new WaitForSeconds(m_FlyingInStartDelay);
        while (m_currentTime < m_FlyingAcrossTimeToReachPosition)
        {
            m_currentTime += Time.deltaTime;
            transform.position = Vector3.LerpUnclamped(m_desiredPos + m_offset, m_desiredPos, Interpolation.CircularOut(m_currentTime / m_FlyingAcrossTimeToReachPosition));
            yield return null;
        }
        transform.position = m_desiredPos;
    }

}
