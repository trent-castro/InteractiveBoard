using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIScaleTransition : MonoBehaviour
{
	[SerializeField] Vector3 m_endScale = Vector3.zero;
	[SerializeField] float m_timer = 0;
	[SerializeField] float m_timeOffset = 0;
	RectTransform m_transform = null;
	Vector3 m_startScale = Vector3.zero;
	float m_time = 0;
	IEnumerator c_Transition = null;

	Vector3 m_start = Vector3.zero;
	Vector3 m_end = Vector3.zero;
	bool m_reverse = false;

	// Start is called before the first frame update
	void Awake()
	{
		m_transform = GetComponent<RectTransform>();
		m_startScale = m_transform.localScale;
		m_start = m_startScale;
		m_end = m_endScale;
	}

	private void OnEnable()
	{
		StartTransformation();
		//StartCoroutine("StartTransformation");
	}
	public void Reverse()
	{
		m_end = m_startScale;
		m_start = m_endScale;
		m_time = 0;
		m_reverse = true;
		c_Transition = UpdateOnFixedInterval();
		StartCoroutine(c_Transition);
	}
	public void StartTransformation()
	{
		m_time = 0 - m_timeOffset;
		m_reverse = false;
		m_transform.localScale = m_startScale;
		m_start = m_startScale;
		m_end = m_endScale;
		c_Transition = UpdateOnFixedInterval();
		StartCoroutine(c_Transition);
	}

	public IEnumerator UpdateOnFixedInterval()
	{
		while (m_time < m_timer)
		{
			m_time += Time.unscaledDeltaTime;
			float t = m_time / m_timer;
			t = Interpolation.ExpoInOut(t);
			m_transform.localScale = Vector3.LerpUnclamped(m_start, m_end, t);
			yield return null;
		}
		c_Transition = null;
		if (!m_reverse)Reverse();
	}
}