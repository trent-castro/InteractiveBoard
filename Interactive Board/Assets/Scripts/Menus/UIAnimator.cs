using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimator : MonoBehaviour
{
	[SerializeField] private float m_timer = 0;
	private float m_time = 0;
	[SerializeField] Vector3 baseScale = Vector3.zero;
	[SerializeField] Vector3 hoverScale = Vector3.zero;
	IEnumerator c_Transition = null;
	float delayTimer = 0;
	RectTransform m_transform = null;

	Vector3 m_start = Vector3.zero;
	Vector3 m_end = Vector3.zero;

	private void Start()
	{
		m_transform = GetComponent<RectTransform>();
		baseScale = m_transform.localScale;
	}

	public void MouseEnter()
	{
		m_time = 0;
		m_start = baseScale;
		m_end = hoverScale;

		if (c_Transition == null)
		{
			c_Transition = UpdateOnFixedInterval();
			StartCoroutine(c_Transition);
		}
	}

	public void MouseExit()
	{
		m_time = 0;
		m_start = hoverScale;
		m_end = baseScale;

		if (c_Transition == null)
		{
			c_Transition = UpdateOnFixedInterval();
			StartCoroutine(c_Transition);
		}
	}
	public IEnumerator UpdateOnFixedInterval()
	{
		while (m_time < m_timer)
		{
			m_time += Time.fixedUnscaledDeltaTime;
			float t = m_time / m_timer;
			//t = Mathf.Clamp01(t);
			t = Interpolation.ElasticOut(t);
			m_transform.localScale = Vector3.LerpUnclamped(m_start, m_end, t);
			yield return null;
		}
		c_Transition = null;

	}

}
