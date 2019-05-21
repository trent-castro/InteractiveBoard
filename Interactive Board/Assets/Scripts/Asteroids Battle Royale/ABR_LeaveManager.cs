using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ABR_LeaveManager : MonoBehaviour
{
	[HideInInspector] public bool[] m_isReadyToLeave = null;
	public ABR_ShipHealth[] m_shipHealth = null;

    // Start is called before the first frame update
    void Awake()
    {
        m_isReadyToLeave = new bool[]{ false, false, false, false };
		//m_isActive = new bool[]{ false, false, false, false };
		if (PlayerPrefs.HasKey("ABR_PlayerCount"))
		{
			int playercount = PlayerPrefs.GetInt("ABR_PlayerCount");

			for (int i = 0; i < playercount; i++)
			{
				m_shipHealth[i].m_isAlive = true;
			}
		}
    }

	public void SetLeaveStateTrue(int player)
	{
		m_isReadyToLeave[player] = true;

		bool leave = true;
		for (int i = 0; i < m_isReadyToLeave.Length; i++)
		{
			if (!m_isReadyToLeave[i] && m_shipHealth[i].m_isAlive)
			{
				leave = false;
				break;
			}
		}

		if (leave) LeaveGame();

	}
	public void SetLeaveStateFalse(int player)
	{
		m_isReadyToLeave[player] = false;

	}

	private void LeaveGame()
	{
		Debug.Log("End Game");
	}
}
