using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// A class that handles the pooling of objects of a specific kind
/// </summary>
public class ObjectPool : MonoBehaviour {

    [Header("Debug Mode")]
    [Tooltip("Enables Debug Mode")]
    public bool debugMode = false;

    [Header("Configuration")]
    [Tooltip("The object to pool")]
    public GameObject GameObjectPrefab;
    [Tooltip("The amount of objects to pool")]
    public int MAX_POOLED_OBJECTS = 400;
    [Tooltip("Object name or ID")]
    public string PooledObjectName = "Pooled Object";
    /// <summary>
    /// The pool of objects
    /// </summary>
    private GameObject[] m_objectPool;
    /// <summary>
    /// An uint tracking the index of the next available object in the pool.
    /// </summary>
    private uint m_objectTracker = 0;

    /// <summary>
    /// Checks the configuration to see if it was set up correctly
    /// </summary>
    private void DebugConfiguration()
    {
        if(!GameObjectPrefab)
            Debug.Log(gameObject.name + " was not given a game object prefab despite being owning an object pool");
        if (MAX_POOLED_OBJECTS == 0)
            Debug.Log(gameObject.name + " is an object pool but was not given an object pool size");
    }

	// Use this for initialization
	private void Awake () {
        if (debugMode)
            DebugConfiguration();

        m_objectPool = new GameObject[MAX_POOLED_OBJECTS];

        for (int x = 0; x < MAX_POOLED_OBJECTS; ++x)
        {
            m_objectPool[x] = Instantiate(GameObjectPrefab);
            m_objectPool[x].SetActive(false);
            m_objectPool[x].transform.parent = gameObject.transform;
            m_objectPool[x].transform.name = PooledObjectName + " ID #" + x;
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
            if (debugMode)
            {
                Debug.Log(gameObject.name + "'s object pool returned an active object for use." +
                    " Consider increasing pool size.");
            }
            return null;
        }

        // Update tracker
        ++m_objectTracker;
        Debug.Log("object tracker @ " + m_objectTracker);   

        // Reset the tracker
        if (m_objectTracker == MAX_POOLED_OBJECTS)
        {
            m_objectTracker = 0;
        }

        return nextPooledObject;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
