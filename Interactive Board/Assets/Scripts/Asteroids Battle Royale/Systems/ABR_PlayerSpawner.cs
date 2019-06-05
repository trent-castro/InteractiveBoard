using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A struct describing a set comprising of a ship and it's camera.
/// </summary>
public struct Player
{
    public ABR_Ship ship;
    public ABR_FollowShipCamera2D camera;

    public void ResetTo(Transform spawnPoint)
    {
        ship.ResetTo(spawnPoint);
        camera.ResetTo(spawnPoint);
    }
}

/// <summary>
/// A script that uses the active players designated in the menu screen to set up the game.
/// </summary>
public class ABR_PlayerSpawner : MonoBehaviour
{
    [Header("External References")]
    [Tooltip("The possible spawnpoints of ships as they join the game.")]
    [SerializeField]
    private Transform[] m_spawnPoints = null;
    [Tooltip("The references to the players.")]
    [SerializeField]
    private GameObject[] m_playerObjects = null;
    [Tooltip("The radius around a spawnpoint that a player cannot spawn from if there is another player active inside.")]
    [SerializeField]
    private float m_PuppyGuardRadius = 25.0f;

    // Private internal data members
    /// <summary>
    /// A reference to all the players.
    /// </summary>
    private Player[] m_players = null;


    void Awake()
    {
        m_players = new Player[m_playerObjects.Length];
        for (int i = 0; i < m_players.Length; i++)
        {
            m_players[i] = new Player()
            {
                ship = m_playerObjects[i].GetComponentInChildren<ABR_Ship>(true),
                camera = m_playerObjects[i].GetComponentInChildren<ABR_FollowShipCamera2D>(true)
            };
        }
    }

    public void JoinGame(int playerIndex)
    {
        if (playerIndex > m_players.Length)
        {
            Debug.Log("Player Doesn't Exist");
            return;
        }

        Transform spawnPoint = GetBestSpawnPoint(playerIndex);

        m_players[playerIndex].ResetTo(spawnPoint);
        ABR_ShipHealth health = m_players[playerIndex].ship.GetComponent<ABR_ShipHealth>();
        if (health)
        {
            health.Respawn();
        }
    }

    public Transform GetBestSpawnPoint(int playerToSpawnIndex)
    {
        List<Transform> validSpawns = new List<Transform>();

        foreach (Transform sp in m_spawnPoints)
        {
            bool valid = true;
            foreach (Player p in m_players)
            {
                if (Vector3.Distance(p.ship.transform.position, sp.position) <= m_PuppyGuardRadius && p.ship.gameObject.activeInHierarchy)
                {
                    valid = false;
                    break;
                }
            }
            if (valid)
            {
                validSpawns.Add(sp);
            }
        }

        return validSpawns.Count > 0 ? validSpawns[UnityEngine.Random.Range(0, validSpawns.Count)] : transform;
    }

    public Player[] GetListOfPlayers()
    {
        return m_players;
    }
}