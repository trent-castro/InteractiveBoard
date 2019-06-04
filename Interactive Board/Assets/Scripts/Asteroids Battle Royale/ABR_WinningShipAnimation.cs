using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that animates the winning ship animations.
/// </summary>
public class ABR_WinningShipAnimation : MonoBehaviour
{
    [Header("Flying across")]
    [Header("External Reference")]
    [Tooltip("An offset of the direction for flying across.")]
    [SerializeField]
    private Vector3 m_FlyingAcrossOffsetDirection = default;
    [Header("Configuration")]
    [Tooltip("The delay before any animation starts happening.")]
    [SerializeField]
    private float m_DelayStart = 1.5f;
    [Tooltip("The offset distance for flying across.")]
    [SerializeField]
    private float m_FlyingAcrossOffsetDistance = 1.0f;
    [Tooltip("The time it would take to reach positions.")]
    [SerializeField]
    float m_FlyingAcrossTimeToReachPosition = 1.5f;

    [Header("Flying in")]
    [Header("External Reference")]
    [Tooltip("The flying in animation's end position.")]
    [SerializeField]
    private Transform m_flyingInDesiredPos = default;
    [Tooltip("The offset direction from which the ship will be flying in from.")]
    [SerializeField]
    Vector3 m_flyingInOffsetDirection = default;
    [Header("Configuration")]
    [Tooltip("The flying in animation's offset distance from the end position")]
    [SerializeField]
    private float m_flyingInOffsetDistance = 1.0f;
    [Tooltip("The flying in animation's time to reach end position.")]
    [SerializeField]
    private float m_FlyingInTimeToReachPosition = 1.5f;
    [Tooltip("The flying in animation's starting delay.")]
    [SerializeField]
    private float m_FlyingInStartDelay = 0.5f;
    

    // Private internal data members
    private Vector3 m_offset = Vector3.zero;
    private Vector3 m_desiredPos = Vector3.zero;
    private float m_currentTime = 0.0f;

    /// <summary>
    /// Modifies default values to correct values.
    /// </summary>
    void Awake()
    {
        m_offset = m_FlyingAcrossOffsetDirection.normalized * m_FlyingAcrossOffsetDistance;
        m_desiredPos = transform.position;
        transform.position = m_desiredPos + m_offset;
        StartCoroutine(FlyAcrossAnimation());
    }

    /// <summary>
    /// Moves the ship to a new location.
    /// </summary>
    private void SetShipToNewLocation()
    {
        m_offset = m_flyingInOffsetDirection.normalized * m_flyingInOffsetDistance;
        transform.position = m_flyingInDesiredPos.position + m_offset;
        m_desiredPos = m_flyingInDesiredPos.position;

        transform.localEulerAngles = new Vector3(0, 0, 25);
        transform.localScale = new Vector3(.66f, .66f, 1.0f);

        StartCoroutine(FlyInAnimation());
    }

    /// <summary>
    /// The initial flying animation across the screen.
    /// </summary>
    IEnumerator FlyAcrossAnimation()
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

    /// <summary>
    /// The secondary animation across the screen.
    /// </summary>
    IEnumerator FlyInAnimation()
    {
        yield return new WaitForSeconds(m_FlyingInStartDelay);
        while (m_currentTime < m_FlyingAcrossTimeToReachPosition)
        {
            m_currentTime += Time.deltaTime;
            transform.position = Vector3.LerpUnclamped(m_desiredPos + m_offset, m_desiredPos, Interpolation.CircularOut(m_currentTime / m_FlyingInTimeToReachPosition));
            yield return null;
        }
        transform.position = m_desiredPos;
    }
}