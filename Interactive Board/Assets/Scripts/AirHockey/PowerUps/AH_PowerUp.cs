using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AH_PowerUp : MonoBehaviour
{
    [Header("Debug Mode")]
    [SerializeField] [Tooltip("Enables Debug Mode")]
    private bool m_debugMode = false;

    [Header("Basic Configuration")]
    [SerializeField] [Tooltip("Whether or not a power up can be removed based on lifespan")]
    private bool m_timeoutEnabled = true;
    [SerializeField] [Tooltip("The duration of the power up being active on a field")]
    private float Lifespan = 8.0f;

    /// <summary>
    /// Float describing the amount of time elapsed since activation
    /// </summary>
    private float m_timeElapsed = 0.0f;
    
    // Update is called once per frame
    void Update()
    {
        if (m_timeoutEnabled)
        {
            m_timeElapsed += Time.deltaTime;
            if (m_timeElapsed >= Lifespan)
            {
                DisablePowerUp();
            }
        }
    }

    /// <summary>
    /// Handles logic based on the interaction with the puck. 
    /// Disabling the object should be handled in derivative classes, within this method.
    /// </summary>
    /// <param name="puck">The colliding puck.</param>
    public abstract void CauseEffect(AH_Puck puck);
    
    /// <summary>
    /// [DEBUG MODE] Records a message if debug mode is enabled.
    /// </summary>
    /// <param name="debugLog">The message to be recorded</param>
    protected void DebugLog(string debugLog)
    {
        if(m_debugMode)
        {
            Debug.Log(debugLog);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Puck"))
        {
        }
    }

    /// <summary>
    /// Disables the power up and returns the position back to the parent transform. 
    /// [DEBUG MODE] Records that the object has been disabled.
    /// </summary>
    public void DisablePowerUp()
    {
        gameObject.SetActive(false);
        transform.position = transform.parent.position;
        DebugLog(gameObject.name + " has been disabled.");
    }

    /// <summary>
    /// Re-enable the power up and resets the internal timer. 
    /// [DEBUG MODE] Records that the object has been enabled.
    /// </summary>
    public void EnablePowerUp()
    {
        gameObject.SetActive(true);
        m_timeElapsed = 0.0f;
        DebugLog(gameObject.name + " has been enabled.");
    }
}