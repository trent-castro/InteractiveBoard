using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the management of pick ups.
/// </summary>
public class AH_PickUpManager : MonoBehaviour
{
    /// <summary>
    /// Singleton paradigm
    /// </summary>
    public static AH_PickUpManager instance;

    /// <summary>
    /// A struct describing the location of the pick up pool as well as the relative weight of the pick ups.
    /// </summary>
    [System.Serializable]
    struct PickUpPool
    {
        public ObjectPool ObjectPool = null;
        public float PoolWeight;
    }

    [Header("Debug Mode")]
    [SerializeField]
    [Tooltip("Enable or Disable Debug Mode")]
    private bool m_debugMode = false;

    [Header("Configuration")]
    [SerializeField]
    [Tooltip("Set a delay for the initial delay before pick ups begin spawning")]
    private float m_spawnStartDelay = 0.0f;
    [SerializeField]
    [Tooltip("Set a delay between each pick up spawning")]
    private float m_spawnDelay = 0.0f;
    [SerializeField]
    [Tooltip("Set spawn variance between each delay")]
    private float m_spawnDelayVariance = 0.0f;
    [SerializeField]
    [Tooltip("Maximum amount of pick ups on the field")]
    private int m_maxFieldPickUps;
    [SerializeField]
    [Tooltip("List of all possible power ups and pick up spawning weights")]
    private PickUpPool[] m_pickUpPools;
    [SerializeField]
    [Tooltip("Vector describing the x bounds and y bounds of the spawning area")]
    private Vector2 m_spawningArea;

    /// <summary>
    /// A timer to track when objects should spawn (Objects spawn when this hits zero).
    /// </summary>
    private float m_timer;
    /// <summary>
    /// A tracking of the current on-field pick ups.
    /// </summary>
    private int m_pickUpCount = 0;
    /// <summary>
    /// A reorganization of all pick up spawning weights, set at threshold percentages.
    /// </summary>
    private float[] m_spawnThresholds;

    // Start is called before the first frame update
    void Start()
    {
        // Set singleton.
        instance = this;

        // Fix the object pool weightings.
        m_spawnThresholds = new float[m_pickUpPools.Length];

        float total = m_pickUpPools[0].PoolWeight;
        for (int x = 1; x < m_pickUpPools.Length; ++x)
        {
            total += m_pickUpPools[x].PoolWeight;
        }

        m_spawnThresholds[0] = m_pickUpPools[0].PoolWeight / total;
        for (int x = 1; x < m_pickUpPools.Length; ++x)
        {
            m_spawnThresholds[x] = m_spawnThresholds[x - 1] + (m_pickUpPools[x].PoolWeight / total);
        }

        // Initialize the timers.
        m_timer = m_spawnStartDelay + Random.Range(-m_spawnDelayVariance, m_spawnDelayVariance);
    }

    /// <summary>
    /// [DEBUG MODE] Records a message if debug mode is enabled.
    /// </summary>
    /// <param name="debugLog">The message to record</param>
    private void DebugLog(string message)
    {
        if (m_debugMode)
        {
            Debug.Log(message);
        }
    }

    /// <summary>
    /// Gets a reference to a random pick up pool within the array of pick up pools
    /// </summary>
    /// <returns>A pick up pool of a particular type</returns>
    private PickUpPool GetRandomPickUpPool()
    {
        // Select a random percentage.
        float roll = Random.Range(0.0f, 1.0f);

        // Check against which percentage the randomized roll checks out to.
        for (int x = 0; x < m_pickUpPools.Length; ++x)
        {
            if (roll <= m_spawnThresholds[x])
            {
                return m_pickUpPools[x];
            }
        }

        // Debug the program if unreachable code is detected.
        DebugLog("Unreachable code detected @ PickUpManager.cs/GetRandomPickUpPool");
        return m_pickUpPools[m_pickUpPools.Length - 1];
    }

    /// <summary>
    /// Spawns a random object somewhere on the board.
    /// </summary>
    private void SpawnRandomObject()
    {
        PickUpPool pickUpPool = GetRandomPickUpPool();
        GameObject nextPickUp = pickUpPool.ObjectPool.GetNextPooledObject();
        SpawnObjectAtRandomLocation(nextPickUp);
    }

    /// <summary>
    /// Places a game object acquired from an object pool at an approximate location specified by the user.
    /// </summary>
    /// <param name="objectToSpawn">A pick up selected from a pool of pick ups.</param>
    private void SpawnObjectAtRandomLocation(GameObject objectToSpawn)
    {
        float x = Random.Range(-m_spawningArea.x, m_spawningArea.x);
        float y = Random.Range(-m_spawningArea.y, m_spawningArea.y);
        Vector3 targetLocation = new Vector3(x, y, 0.0f);
        DebugLog("Spawning " + objectToSpawn.name + " @ " + targetLocation);
        objectToSpawn.GetComponent<AH_PickUp>().EnablePickUp(targetLocation);
    }


    // Update is called once per frame
    void Update()
    {
        // Update timers
        m_timer -= Time.deltaTime;

        // Check if the timer has expired while one can place a pick up.
        if (m_pickUpCount != m_maxFieldPickUps)
        {
            if (m_timer <= 0.0f)
            {
                m_timer += m_spawnDelay;
                SpawnRandomObject();
            }
        }
    }

    /// <summary>
    /// Increments the current pick up count for pick up count capping.
    /// </summary>
    public void IncreaseCurrentPickUpCount()
    {
        ++m_pickUpCount;
    }

    /// <summary>
    /// Decrements the current pick up count for pick up count capping.
    /// </summary>
    public void DecreaseCurrentPickUpCount()
    {
        --m_pickUpCount;
    }
}
