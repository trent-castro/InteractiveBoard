using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_Gameloop : MonoBehaviour
{
    public static ABR_Gameloop instance;
    [Header("External References")]
    [Tooltip("A reference to the player spawner.")]
    [SerializeField]
    private ABR_PlayerSpawner m_playerSpawner;
    [Tooltip("A reference to the leave manager.")]
    [SerializeField]
    private ABR_LeaveManager m_leaveManager;

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
        // send back to screen
        Debug.Log("HEY BY THE WAY THE GAME SHOULD HAVE ENDED");
    }
}