using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_Gameloop : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
=======
    public static ABR_Gameloop instance;
    [Header("External References")]
    [Tooltip("A reference to the player spawner.")]
    [SerializeField]
    private ABR_PlayerSpawner m_playerSpawner;

    // Private data members
    private Player[] m_players;    

    void Awake()
    {
        instance = this;
        m_players = m_playerSpawner.GetListOfPlayers();
    }

    public void CheckGameState()
    {
        // update list of players
        // check if win
    }

    public void EndGame()
    {
        // if multiple, tie
    }
}
>>>>>>> cffd521fb0e101fc6479686606f2e90268433363
