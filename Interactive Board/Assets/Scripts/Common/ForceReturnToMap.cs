using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReturnToMap : MonoBehaviour
{
	[Tooltip("The gameobject used as a warning that the person will be forced back to the map.")]
	[SerializeField] GameObject m_timeWarning = null;
	[Tooltip ("The time before a person is forced back to the map screen.")]
	[SerializeField] float m_timer = 30;
	float m_time = 0;
	bool HasOpenedTab = false;

	// Update is called once per frame
	void Update()
	{
		if (Input.anyKeyDown)
		{
			m_time = 0;
		}
		m_time += Time.unscaledDeltaTime;
		if (m_time >= (m_timer - 10))
		{
			TurnOnWarning();
		}
		if (m_time >= m_timer && !HasOpenedTab)
		{
			HasOpenedTab = true;
			TimedReturnToMap();
		}
	}

	private void TurnOnWarning()
	{
		m_timeWarning.SetActive(true);
	}

	public void ResetMapTimer()
	{
		m_time = 0;
		HasOpenedTab = false;
	}

	private void TimedReturnToMap()
	{
		Debug.Log("Return to Map.");
		if (!Application.isEditor)
		{
			Application.OpenURL("https://interactive.neumont.edu/");
		}
	}
}
