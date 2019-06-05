using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// A script that handles the logic for winning the game.
/// </summary>
public class ABR_Gameloop : MonoBehaviour
{
    // Singleton paradigm
    public static ABR_Gameloop instance;

    [Header("External References")]
    [Tooltip("A reference to the leave manager.")]
    [SerializeField]
    private ABR_LeaveManager m_leaveManager = null;
    [Tooltip("A reference to the end game UI.")]
	[SerializeField]
	private GameObject m_endGameUI = null;
    [Tooltip("A reference to the winning ship image used in the end game UI.")]
	[SerializeField] 
	private Image m_winingShipImage = null;
    [Tooltip("A reference to all the ship images for use in the end game UI.")]
	[SerializeField]
	private Sprite[] m_playerSprites = null;

    private void Awake()
    {
        instance = this;
    }

    public void CheckGameState()
    {
        bool[] playerIsAlive = m_leaveManager.GetStatesOfShips();
        int aliveCount = 0;
        foreach(bool state in playerIsAlive)
        {
            if(state)
            {
                ++aliveCount;
            }
        }

        if(aliveCount <= 1)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
		bool[] playerIsAlive = m_leaveManager.GetStatesOfShips();

		for (int i = 0; i < playerIsAlive.Length; i++)
		{
			if (playerIsAlive[i]) m_winingShipImage.sprite = m_playerSprites[i];
		}
		m_endGameUI.SetActive(true);
    }

	public void ReturnToArcade()
	{
		SceneManager.LoadScene("Arcade Menu");
	}
}