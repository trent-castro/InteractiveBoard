using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class UIAnimator : MonoBehaviour
{
	[SerializeField] Vector3 hoverScale = Vector3.zero;
	[SerializeField] private float m_timer = 0;

	RectTransform m_transform = null;

	private float m_time = 0;

	Vector3 baseScale = Vector3.zero;
	Vector3 m_start = Vector3.zero;
	Vector3 m_end = Vector3.zero;

	IEnumerator c_Transition = null;
	private void Awake()
	{
		m_transform = GetComponent<RectTransform>();
		baseScale = m_transform.localScale;

		EventTrigger trigger = GetComponent<EventTrigger>();

		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener((data) => { MouseEnter(); });

		EventTrigger.Entry leave = new EventTrigger.Entry();
		leave.eventID = EventTriggerType.PointerExit;
		leave.callback.AddListener((data) => { MouseExit(); });

		trigger.triggers.Add(entry);
		trigger.triggers.Add(leave);
	}
	private void OnEnable()
	{
		m_time = 0;
		m_transform.localScale = baseScale;
		m_start = baseScale;
		m_end = hoverScale;
		c_Transition = null;
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
