using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_ShipInitialization : MonoBehaviour
{
	int m_playerCount = 1;
	[SerializeField] ABR_PlayerSpawner spawner = null;
	[SerializeField] GameObject[] m_players = null;
	[SerializeField] GameObject[] m_UI = null;
    // Start is called before the first frame update
    void Start()
    {
		if (PlayerPrefs.HasKey("ABR_PlayerCount"))
		{
			m_playerCount = PlayerPrefs.GetInt("ABR_PlayerCount");
		}
		for (int i = 0; i < m_playerCount; i++)
		{
			if (i < m_UI.Length) m_UI[i].SetActive(false);
			if (i < m_players.Length) m_players[i].SetActive(true);
			spawner.Respawn(i);
		}
	}


}
