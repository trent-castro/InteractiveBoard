using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_PickUpManager : MonoBehaviour
{
    public static AH_PickUpManager instance;

    [System.Serializable]
    struct PickUpPool
    {
        public ObjectPool ObjectPool;
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
        instance = this;
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

        m_timer = m_spawnStartDelay;
    }

    private void DebugLog(string message)
    {
        if (m_debugMode)
        {
            Debug.Log(message);
        }
    }

    private PickUpPool GetRandomPickUpPool()
    {
        float roll = Random.Range(0.0f, 1.0f);
        for (int x = 0; x < m_pickUpPools.Length; ++x)
        {
            if (roll <= m_spawnThresholds[x])
            {
                return m_pickUpPools[x];
            }
        }

        DebugLog("Unreachable code detected @ PickUpManager.cs/GetRandomPickUpPool");
        return m_pickUpPools[m_pickUpPools.Length - 1];
    }

    private void SpawnRandomObject()
    {
        PickUpPool pickUpPool = GetRandomPickUpPool();
        GameObject nextPickUp = pickUpPool.ObjectPool.GetNextPooledObject();
        SpawnObjectAtRandomLocation(nextPickUp);
    }

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
        m_timer -= Time.deltaTime;

        if (m_timer <= 0.0f)
        {
            m_timer += m_spawnDelay;
            if (m_pickUpCount != m_maxFieldPickUps)
            {
                SpawnRandomObject();
            }
        }
    }

    public void IncreaseCurrentPickUpCount()
    {
        ++m_pickUpCount;
    }

    public void DecreaseCurrentPickUpCount()
    {
        --m_pickUpCount;
    }
}
