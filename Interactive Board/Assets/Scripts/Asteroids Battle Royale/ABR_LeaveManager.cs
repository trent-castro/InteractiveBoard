using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ABR_LeaveManager : MonoBehaviour
{
	[HideInInspector] public bool[] m_isReadyToLeave = null;
	public ABR_ShipHealth[] m_shipHealth = null;
	public GameObject[] m_LeaveUI = null;

    // Start is called before the first frame update
    void Awake()
    {
        m_isReadyToLeave = new bool[]{ false, false, false, false };
		foreach (var item in m_shipHealth)
		{
			item.m_isAlive = false;
		}
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
		m_shipHealth[player].m_isAlive = false;
		m_LeaveUI[player].SetActive(true);
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
		m_shipHealth[player].m_isAlive = true;
		m_LeaveUI[player].SetActive(false);
	}

	private void LeaveGame()
	{
		Debug.Log("End Game");
		SceneManager.LoadScene("Arcade Menu");
	}

	/// <summary>
	/// Returns an array containing the alive state of all player ships. 
	/// If a player has not joined the game, it is considered dead. If a player is attempting to end the game early, they are still considered alive.
	/// </summary>
	/// <returns></returns>
    public bool[] GetStatesOfShips()
    {
		bool[] isAliveStates = new bool[m_shipHealth.Length];
		for (int i = 0; i < m_shipHealth.Length; i++)
		{
			isAliveStates[i] = m_shipHealth[i].m_isAlive;
		}

        return isAliveStates;
    }
}
