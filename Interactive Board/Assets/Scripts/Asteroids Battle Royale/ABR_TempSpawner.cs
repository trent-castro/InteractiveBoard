using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_TempSpawner : MonoBehaviour
{
    [Header("Debug Mode")]
    [SerializeField]
    [Tooltip("Enable or Disable Debug Mode")]
    private bool m_debugMode = false;

    [Header("Configuration")]
    [SerializeField]
    [Tooltip("Enables or disables the ability to spawn objects")]
    public bool m_canSpawn = true;
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
    [Tooltip("List of all possible power ups and pick up spawning weights")]
    private ObjectPool m_objectPool = null;
    [SerializeField]
    [Tooltip("List of all possible power ups and pick up spawning weights")]
    private GameObject[] m_powerUps = null;
    [SerializeField]
    [Tooltip("Vector describing the x bounds and y bounds of the spawning area")]
    private Vector2 m_spawningArea = Vector2.zero;

    /// <summary>
    /// A timer to track when objects should spawn (Objects spawn when this hits zero).
    /// </summary>
    private float m_timer;

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

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the timers.
        m_timer = m_spawnStartDelay + Random.Range(-m_spawnDelayVariance, m_spawnDelayVariance);
    }

    // Update is called once per frame
    void Update()
    {

        // Update timers
        m_timer -= Time.deltaTime;

        if (m_timer <= 0.0f && m_canSpawn)
        {
            m_timer += m_spawnDelay;
            SpawnRandomObject();
        }
    }


    /// <summary>
    /// Spawns a random object somewhere on the board.
    /// </summary>
    private void SpawnRandomObject()
    {
        GameObject nextPickUp = m_objectPool.GetNextPooledObject();
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
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = targetLocation;
        objectToSpawn.tag = ABR_Tags.AsteroidTag;

        ///Change later
        ABR_Asteroid asteroid = objectToSpawn.GetComponent<ABR_Asteroid>();
        if (asteroid)
        {
            GameObject pickup = ABR_GlobalInfo.WeaponPickupManager.GetObjectFromTaggedPool(RandomEnumValue<eBulletType>().ToString());
            asteroid.AddItem(pickup);
        }

    }   

    private T RandomEnumValue<T>()
    {
        var v = System.Enum.GetValues(typeof(T));
        return (T)v.GetValue(Random.Range(0, v.Length));
    }
}
