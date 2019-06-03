using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePanel : MonoBehaviour
{
    [SerializeField]
    private Transform m_outPosition = null;

    [SerializeField]
    private Transform m_inPosition = null;

    [SerializeField]
    private bool m_slideOut = false;

    private float m_timer = 1;

    [SerializeField]
    private float m_timeToSlide = 1;

    void Update()
    {
        if (m_slideOut && m_timer < m_timeToSlide)
        {
            m_timer += Time.deltaTime;
        }
        else if (!m_slideOut && m_timer > 0)
        {
            m_timer -= Time.deltaTime;
        }

        m_timer = Mathf.Clamp(m_timer, 0, m_timeToSlide);
        transform.position = Vector3.LerpUnclamped(m_inPosition.position, m_outPosition.position, Interpolation.SineInOut(m_timer / m_timeToSlide));
    }

    public void SetSlideOut(bool slideOut)
    {
        m_slideOut = slideOut;
    }
}
