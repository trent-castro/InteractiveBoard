using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_ParticlePools : MonoBehaviour
{
    /// <summary>
    /// Singleton paradigm
    /// </summary>
    public static AH_ParticlePools instance;

    /// <summary>
    /// A struct describing the location of the particle pool as well as the 
    /// associated tag for the particles.
    /// </summary>
    [System.Serializable]
    struct ParticlePool
    {
        public ObjectPool ObjectPool;
        public string AssociatedTag;
    }

    [Header("Debug Mode")]
    [SerializeField]
    [Tooltip("Enable or Disable Debug Mode")]
    private bool m_debugMode = false;

    [Header("Configuration")]
    [SerializeField]
    [Tooltip("An array of ObjectPools for particles.")]
    private ParticlePool[] ParticlePools;

    private void Awake()
    {
        instance = this;
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
    /// Gets a reference to the next available particle pool with the specified tag.
    /// </summary>
    /// <param name="tag">The tag of which pool to pull from.</param>
    /// <returns>A particle system from the pool with the associated tag requested.</returns>
    public GameObject GetTaggedParticleSystem(string tag)
    {
        GameObject particlePool = null;
        for (int x = 0; x < ParticlePools.Length; ++x)
        {
            if(ParticlePools[x].AssociatedTag == tag)
            {
                particlePool = ParticlePools[x].ObjectPool.GetNextPooledObject();
                particlePool.SetActive(true);
                return particlePool;
            }
        }

        DebugLog("Cannot find particle systems with tag [" + tag + "]");
        return null;
    }
}
