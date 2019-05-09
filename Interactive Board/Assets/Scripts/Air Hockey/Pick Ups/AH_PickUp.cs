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

    [Header("Animation")]
    [Tooltip("The amount of time it takes for the animation to complete")]
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float m_spawnAnimationTime = 0.5f;
    [Tooltip("The starting scale of the pick up object upon beginning the animation")]
    [SerializeField]
    private Vector3 m_startingScale = new Vector3(3, 3, 3);

    [Header("Basic Configuration")]
    [Tooltip("Whether or not a pick up can be removed based on lifespan")]
    [SerializeField]
    private bool m_timeoutEnabled = true;
    [Tooltip("The duration of the pick up being active on a field")]
    [SerializeField]
    private float m_lifeSpan = 8.0f;
    [Tooltip("The duration of the pick up's effect")]
    [SerializeField]
    [Range(0.0f, 30.0f)]
    protected float m_powerUpDuration = 5.0f;

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
    private AH_BitFlagBroadcaster m_bitFlagBroadcaster;

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
    /// Float describing the amount of time elapsed since the pick up spawn animation was enabled.
    /// </summary>
    private float m_animationTimer = 0.0f;
    
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
        m_bitFlagBroadcaster = GetComponent<AH_BitFlagBroadcaster>();
    }

    // Update is called once per frame
    void Update()
    {
        // Whether or not the pick up has been interacted with
        if (m_timeoutEnabled)
        {
            // Increment the lifespan timer
            m_lifespanTimer += Time.deltaTime;

            ValidateLifespanDuration();
        }
        else
        {
            // Increment the effect timer
            m_effectTimer += Time.deltaTime;

            ValidateEffectDuration();
        }
    }

    /// <summary>
    /// Check if the pick up life span has expired before it has been picked up.
    /// </summary>
    private void ValidateLifespanDuration()
    {
        if (m_lifespanTimer >= m_lifeSpan)
        {
            DisablePickUp();
        }
    }

    /// <summary>
    /// Checks if the pick up effect has expired.
    /// </summary>
    private void ValidateEffectDuration()
    {
        // Check if the effect has expired
        if (m_effectTimer >= m_powerUpDuration)
        {
            // If the afflicted puck exists, start the end 
            if (afflictedPuck)
            {
                if (afflictedPuck.GetComponent<AH_BitFlagReceiver>()
                    .Contains(m_bitFlagBroadcaster.Broadcast()))
                {
                    OnEffectEnd();
                    afflictedPuck.GetComponent<AH_BitFlagReceiver>()
                        .RemoveFlag(m_bitFlagBroadcaster.Broadcast());
                }
                DisablePickUp();
            }
        }
    }

    /// <summary>
    /// Triggers when a pick up is spawned
    /// </summary>
    /// <param name="collision"></param>
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

        if(ValidateOnPickUpEffects())
        {
            ActivateOnPickUpEffects();
        }
        else
        {
            PlayParticleEffect("Pick Up Failure");
        }
    }

    /// <summary>
    /// Validates that the pick up is not currently being affected by an equivalent pick up effect.
    /// </summary>
    /// <returns>A bool describing whether or not the effect is already in play for this puck.</returns>
    private bool ValidateOnPickUpEffects()
    {
        return !afflictedPuck.GetComponent<AH_BitFlagReceiver>().Contains(m_bitFlagBroadcaster.Broadcast());
    }

    /// <summary>
    /// Starts the on pick up effect event.
    /// </summary>
    private void ActivateOnPickUpEffects()
    {
        PlayParticleEffect("Pick Up Success");
        OnEffectBegin();
        afflictedPuck.GetComponent<AH_BitFlagReceiver>().AddFlag(m_bitFlagBroadcaster.Broadcast());
    }

    /// <summary>
    /// Plays particle effects for pick ups if they exist.
    /// </summary>
    private void PlayParticleEffect(string particleSystemTag)
    {
        GameObject pickUpParticles = AH_ParticlePools.instance.GetTaggedParticleSystem(particleSystemTag);

        if (pickUpParticles)
        {
            pickUpParticles.transform.position = transform.position;
            pickUpParticles.GetComponent<ParticleSystem>().Play();
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

        // Enable animation
        StartCoroutine(OnEnableStartAnimation());

        // Debug
        DebugLog(gameObject.name + " has been enabled.");
    }

    public IEnumerator OnEnableStartAnimation()
    {
        m_collider2D.enabled = false;
        while(m_animationTimer < m_spawnAnimationTime)
        {
            m_animationTimer += Time.deltaTime;
            float t = m_animationTimer / m_spawnAnimationTime;
            t = Interpolation.BounceOut(t);
            transform.localScale = Vector3.LerpUnclamped(m_startingScale, Vector3.one, t);
            yield return null;
        }
        m_collider2D.enabled = true;
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