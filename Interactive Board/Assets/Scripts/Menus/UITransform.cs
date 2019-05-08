using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UITransform : MonoBehaviour
{
	[SerializeField] Vector3 m_endLocation = Vector3.zero;
	[SerializeField] float m_timer = 0;
	[SerializeField] bool repeat = true;
	RectTransform m_transform = null;
	Vector3 m_startLocation = Vector3.zero;
	float m_time = 0;

	IEnumerator c_Transition = null;
	

	// Start is called before the first frame update
	void Awake()
    {
		m_transform = GetComponent<RectTransform>();
		m_startLocation = m_transform.localPosition;
    }

	private void OnEnable()
	{
		//StartTransformation();
		StartCoroutine("RestartHighlight");
	}
	public IEnumerator RestartHighlight()
	{
		while (repeat)
		{
			StartTransformation();
			yield return new WaitForSeconds(4);
		}
	}
	public void StartTransformation()
	{
		m_time = 0;
		m_transform.localPosition = m_startLocation;
		c_Transition = UpdateOnFixedInterval();
		StartCoroutine(c_Transition);
	}

	public IEnumerator UpdateOnFixedInterval()
	{
		while (m_time < m_timer)
		{
			m_time += Time.unscaledDeltaTime;
			float t = m_time / m_timer;
			t = Interpolation.QuarticInOut(t);
			m_transform.localPosition = Vector3.LerpUnclamped(m_startLocation, m_endLocation, t);
			yield return null;
		}
		c_Transition = null;

	}
}
