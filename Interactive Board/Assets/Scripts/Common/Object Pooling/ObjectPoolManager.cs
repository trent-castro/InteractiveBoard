using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the management of object pools. This does not conform to a singleton paradigm because 
/// one could have object pools for specific subtypes of game objects.
/// </summary>
public class ObjectPoolManager : MonoBehaviour
{
    /// <summary>
    /// A struct describing the location of the object pool as well as the relative weight of the pick ups, while
    /// providing an associated tag to choose from a specific pool if requested.
    /// </summary>
    [System.Serializable]
    struct ObjectPoolConfiguration
    {
        /// <summary>
        /// A reference to the object pool location.
        /// </summary>
        public ObjectPool objectPool;
        /// <summary>
        /// A float describing the relationship in terms of frequency compared to other object pool configurations.
        /// </summary>
        public float poolWeight;
        /// <summary>
        /// A string that helps one find a subset object pool game object from a specific object pool configuration.
        /// </summary>
        public string associatedTag;

        /// <summary>
        /// A default contructor with full overloading.
        /// </summary>
        /// <param name="objectpool">A reference to the object pool location.</param>
        /// <param name="poolweight">A float describing the relationship in terms of frequency compared to other object pool configurations.</param>
        /// <param name="associatedtag">A string that helps one find a subset object pool game object from a specific object pool configuration.</param>
        public ObjectPoolConfiguration(ObjectPool objectpool, float poolweight, string associatedtag)
        {
            objectPool = objectpool;
            poolWeight = poolweight;
            associatedTag = associatedtag;
        }
    }

    [Header("Debug Mode")]
    [Tooltip("Enable or Disable Debug Mode")]
    [SerializeField]
    private bool m_debugMode = false;

    [Header("Configuration")]
    [Tooltip("An array of ObjectPoolConfigurations")]
    [SerializeField]
    private ObjectPoolConfiguration[] objectPoolConfigurations = null;

    /// Private Data Members
    /// <summary>
    /// A reorganization of all spawning weights, set at threshold percentages.
    /// </summary>
    private float[] m_spawnThresholds;
    
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
    /// Initialization of class
    /// </summary>
    private void Awake()
    {
        // Fix the object pool configuration weightings.
        FixSpawnWeightConfiguration();
    }
    
    /// <summary>
    /// Recalculates all pool spawn thresholds in relation to each other for random prefab acquisition.
    /// </summary>
    private void FixSpawnWeightConfiguration()
    {
        m_spawnThresholds = new float[objectPoolConfigurations.Length];

        float total = objectPoolConfigurations[0].poolWeight;
        for (int x = 1; x < objectPoolConfigurations.Length; ++x)
        {
            total += objectPoolConfigurations[x].poolWeight;
        }

        m_spawnThresholds[0] = objectPoolConfigurations[0].poolWeight / total;
        for (int x = 1; x < objectPoolConfigurations.Length; ++x)
        {
            m_spawnThresholds[x] = m_spawnThresholds[x - 1] + (objectPoolConfigurations[x].poolWeight / total);
        }
    }

    /// <summary>
    /// Gets a reference to the next available object from a pool with the specified tag.
    /// </summary>
    /// <param name="tag">The tag of which pool to pull from.</param>
    /// <returns>An object dervied the pool with the associated tag requested.</returns>
    public GameObject GetObjectFromTaggedPool(string tag)
    {
        GameObject gameObject = null;
        for (int x = 0; x < objectPoolConfigurations.Length; ++x)
        {
            if (objectPoolConfigurations[x].associatedTag == tag)
            {
                gameObject = objectPoolConfigurations[x].objectPool.GetNextPooledObject();
                gameObject.SetActive(true);
                return gameObject;
            }
        }

        DebugLog("Cannot find ObjectPoolConfiguration with tag [" + tag + "]");
        return null;
    }

    /// <summary>
    /// Gets a reference to a random object pool configuration within the array of object pool configurations.
    /// </summary>
    /// <returns>An object pool configuration of a particular type.</returns>
    private GameObject GetRandomGameObjectFromSelectedPool()
    {
        // Select a random percentage.
        float roll = Random.Range(0.0f, 1.0f);

        // Check against which percentage the randomized roll checks out to.
        for (int x = 0; x < objectPoolConfigurations.Length; ++x)
        {
            if (roll <= m_spawnThresholds[x])
            {
                return objectPoolConfigurations[x].objectPool.GetNextPooledObject();
            }
        }

        // Debug the program if unreachable code is detected.
        DebugLog("Unreachable code detected @ ObjectPoolManager.cs/GetRandomGameObjectFromSelectedPool.");
        return objectPoolConfigurations[objectPoolConfigurations.Length - 1].objectPool.GetNextPooledObject();
    }
}