using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// A class that handles the pooling of objects of a specific kind
/// </summary>
public class ObjectPool : MonoBehaviour {

    [Header("Debug Mode")] 
    [SerializeField] [Tooltip("Enables Debug Mode")]
    private bool m_debugMode = false;

    [Header("Configuration")]
    [SerializeField] [Tooltip("The object to pool")]
    private GameObject m_gameObjectPrefab;
    [SerializeField] [Tooltip("The amount of objects to pool")]
    private int m_maxPooledObjects = 400;
    [SerializeField] [Tooltip("Object name or ID")]
    private string m_pooledObjectName = "Pooled Object";
    /// <summary>
    /// The pool of objects
    /// </summary>
    private GameObject[] m_objectPool;
    /// <summary>
    /// An uint tracking the index of the next available object in the pool.
    /// </summary>
    private uint m_objectTracker = 0;

    /// <summary>
    /// Checks the configuration to see if it was set up correctly.
    /// </summary>
    private void DebugConfiguration()
    {
        if(!m_gameObjectPrefab)
            Debug.Log(gameObject.name + " was not given a game object prefab despite being owning an object pool");
        if (m_maxPooledObjects == 0)
            Debug.Log(gameObject.name + " is an object pool but was not given an object pool size");
    }

    /// <summary>
    /// [DEBUG MODE] Records a message if debug mode is enabled.
    /// </summary>
    /// <param name="debugLog">The message to record</param>
    private void DebugLog(string debugLog)
    {
        if(m_debugMode)
        {
            Debug.Log(debugLog);
        }
    }

	// Use this for initialization
	private void Awake () {
        DebugConfiguration();

        m_objectPool = new GameObject[m_maxPooledObjects];

        for (int x = 0; x < m_maxPooledObjects; ++x)
        {
            m_objectPool[x] = Instantiate(m_gameObjectPrefab);
            m_objectPool[x].SetActive(false);
            m_objectPool[x].transform.parent = gameObject.transform;
            m_objectPool[x].transform.name = m_pooledObjectName + " - ID #" + x;
        }
	}
	
    /// <summary>
    /// Returns a reference to the next object for use in the pool and modifies the object tracking. 
    /// [DEBUG MODE] Leaves a warning in console if it returns an active object.
    /// </summary>
    /// <returns>Returns the next pooled object</returns>
    public GameObject GetNextPooledObject() {
        // Acquire next pooled object
        GameObject nextPooledObject = m_objectPool[m_objectTracker];

        // Debug the pooled object
        if (nextPooledObject.activeSelf)
        {
            DebugLog(gameObject.name + "'s object pool returned an active object for use. Consider increasing pool size.");
            return null;
        }

        // Update tracker
        ++m_objectTracker;
        DebugLog("object tracker @ " + m_objectTracker);   

        // Reset the tracker
        if (m_objectTracker == m_maxPooledObjects)
        {
            m_objectTracker = 0;
        }

        return nextPooledObject;
    }
}
