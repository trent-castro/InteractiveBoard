using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script used to activate the UI of active players upon entering the game.
/// </summary>
public class ABR_ShipInitialization : MonoBehaviour
{
    [Header("External References")]
    [Tooltip("A reference to the player spawner script.")]
	[SerializeField]
    private ABR_PlayerSpawner spawner = null;
    [Tooltip("A reference to each of the player UI once active.")]
	[SerializeField]
    private GameObject[] m_players = null;
    [Tooltip("A reference to the join UI of a player's individual screen.")]
	[SerializeField]
    private GameObject[] m_UI = null;

    // Private internal data members
	private int m_playerCount = 1;

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
			spawner.JoinGame(i);
		}
	}


}
