using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract class for the base functionality of a pick up.
/// </summary>
public abstract class AH_PickUp : MonoBehaviour
{
    [Header("Debug Mode")]
    [SerializeField] [Tooltip("Enables Debug Mode")]
    private bool m_debugMode = false;

    [Header("Basic Configuration")]
    [SerializeField] [Tooltip("Whether or not a pick up can be removed based on lifespan")]
    private bool m_timeoutEnabled = true;
    [SerializeField] [Tooltip("The duration of the pick up being active on a field")]
    private float Lifespan = 8.0f;
    [SerializeField] [Tooltip("The duration of the pick up's effect")]
    protected float m_powerUpDuration = 3.0f;

    // External References
    /// <summary>
    /// The puck that is being affected by the pick up currently.
    /// </summary>
    protected AH_Puck afflictedPuck;

    // Private Sibling Components
    /// <summary>
    /// The sibling sprite renderer component of the pick up.
    /// </summary>
    private SpriteRenderer m_spriteRenderer;
    /// <summary>
    /// The sibling collider 2D component of the pick up.
    /// </summary>
    private Collider2D m_collider2D;
    /// <summary>
    /// The sibling component bit flag broadcaster of the pick up.
    /// </summary>
    private AH_BitFlagBroadcaster bitFlagBroadcaster;

    // Private information
    /// <summary>
    /// Float describing the amount of time elapsed since the pick up was enabled.
    /// </summary>
    private float m_lifespanTimer = 0.0f;
    /// <summary>
    /// Float describing the amount of time elapsed since the pick up effect was set in place.
    /// </summary>
    private float m_effectTimer = 0.0f;
    
    /// <summary>
    /// [DEBUG MODE] Records a message if debug mode is enabled.
    /// </summary>
    /// <param name="debugLog">The message to be recorded</param>
    protected void DebugLog(string debugLog)
    {
        if (m_debugMode)
        {
            Debug.Log(debugLog);
        }
    }

    /// <summary>
    /// Initialization that takes place once.
    /// </summary>
    private void Awake()
    {
        // Set private references to sibling components
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_collider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Whether or not the pick up has been interacted with
        if (m_timeoutEnabled)
        {
            // Increment the lifespan timer
            m_lifespanTimer += Time.deltaTime;
            
            // Check if the pick up life span has expired
            if (m_lifespanTimer >= Lifespan)
            {
                DisablePickUp();
            }
        }
        else
        {
            // Increment the effect timer
            m_effectTimer += Time.deltaTime;
            
            // Check if the effect has expired
            if(m_effectTimer >= m_powerUpDuration)
            {
                OnEffectEnd();
                afflictedPuck.GetComponent<AH_BitFlagReceiver>()
                    .RemoveFlag(bitFlagBroadcaster.Broadcast());
                DisablePickUp();
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Puck"))
        {
            OnPickUp(collision.gameObject.GetComponent<AH_Puck>());
        }
    }

    /// <summary>
    /// Handles the events that occur when a pick up is picked up.
    /// </summary>
    /// <param name="puck">The puck that picked up the pick up.</param>
    public void OnPickUp(AH_Puck puck)
    {
        // Set a reference to the puck
        afflictedPuck = puck;

        // Mark the pick up as interacted
        m_timeoutEnabled = false;

        // Disable pick up from being interacted with in the scene
        DisablePickUpRendering();

        if(!afflictedPuck.GetComponent<AH_BitFlagReceiver>()
            .CheckForFlag(bitFlagBroadcaster.Broadcast()))
        {
            // Begin the pick up effects
            OnEffectBegin();
            afflictedPuck.GetComponent<AH_BitFlagReceiver>()
                .AddFlag(bitFlagBroadcaster.Broadcast());
        }
    }

    /// <summary>
    /// Enables the pick up to be interacted with in the scene.
    /// </summary>
    public void EnablePickUpRendering()
    {
        m_spriteRenderer.enabled = true;
        m_collider2D.enabled = true;
    }

    /// <summary>
    /// Disables the pick up's ability to be interacted with in the scene.
    /// </summary>
    public void DisablePickUpRendering()
    {
        m_spriteRenderer.enabled = false;
        m_collider2D.enabled = false;
    }

    /// <summary>
    /// Handles the effect the pick up has once picked up.
    /// </summary>
    public virtual void OnEffectBegin()
    {
        DebugLog(afflictedPuck.name + "'s [" + transform.name + "] effect has begun.");
    }

    /// <summary>
    /// Handles (most likely) the inverse effect of the effect the pick up has once picked up. 
    /// Does not need to be implemented by the derivative classes.
    /// </summary>
    public virtual void OnEffectEnd()
    {
        DebugLog(afflictedPuck.name + "'s [" + transform.name + "] effect has ended.");
    }

    /// <summary>
    /// Re-enable the power up and resets the internal timer. 
    /// [DEBUG MODE] Records that the object has been enabled.
    /// </summary>
    /// <param name="newLocation">The location at which to spawn the power up at.</param>
    public void EnablePickUp(Vector3 newLocation)
    {
        // Modify external references
        afflictedPuck = null;
        AH_PickUpManager.instance.IncreaseCurrentPickUpCount();

        // Activate the game object and it's components
        gameObject.SetActive(true);
        EnablePickUpRendering();

        // Modify Location
        transform.position = newLocation;

        // Reset timers
        m_timeoutEnabled = true;
        m_lifespanTimer = 0.0f;
        m_effectTimer = 0.0f;

        // Debug
        DebugLog(gameObject.name + " has been enabled.");
    }

    /// <summary>
    /// Disables the power up and returns the position back to the parent transform. 
    /// [DEBUG MODE] Records that the object has been disabled.
    /// </summary>
    public void DisablePickUp()
    {
        // Disable the game object
        gameObject.SetActive(false);

        // Reset object location for a cleaner scene
        transform.position = transform.parent.position;

        // Modify external references
        AH_PickUpManager.instance.DecreaseCurrentPickUpCount();

        // Debug
        DebugLog(gameObject.name + " has been disabled.");
    }
}