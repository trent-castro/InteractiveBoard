using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

public class ABR_PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] m_spawnPoints = null;

    [SerializeField]
    private GameObject[] m_playerObjects = null;

    [SerializeField]
    private float m_PuppyGuardRadius = 25.0f;

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
        //m_spawnPoints.TakeWhile(sp => m_players.All(p => Vector3.Distance(p.ship.transform.position, sp.position) > m_PuppyGaurdRadius || !p.ship.isActiveAndEnabled)).ToArray();

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