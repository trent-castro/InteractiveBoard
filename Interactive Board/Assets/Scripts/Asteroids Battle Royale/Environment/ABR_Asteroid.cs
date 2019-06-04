using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script for a destructible object that players can acquire different weapons through.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class ABR_Asteroid : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("The speed at which the asteroid is travelling.")]
	public float maxSpeed = 3;

    // Private internal data members
    /// <summary>
    /// The item that an asteroid will drop upon being broken.
    /// </summary>
	private GameObject m_item;
    /// <summary>
    /// A reference to the child particle system component.
    /// </summary>
	private ParticleSystem m_shards = null;

    // Controls for the destruction animation
    /// <summary>
    /// A timer to track time elapsed during animation.
    /// </summary>
	private float m_timer = 0;
    /// <summary>
    /// The total amount of time the animation lasts.
    /// </summary>
	private float m_animationTime = 1.5f;
    /// <summary>
    /// The scale of the asteroid in the game.
    /// </summary>
	private Vector3 m_normalScale = new Vector3(5f, 5f, 5f);
	private bool isPerishing = false;

	//Decay Timer
    /// <summary>
    /// The total amount of time the asteroid has been active since being spawned.
    /// </summary>
	private float m_decayTimer = 0;
    /// <summary>
    /// The total amount of time an asteroid is allowed to be active before self-destructing.
    /// </summary>
	private float m_decayMaxTime = 30;

	private void Start()
	{
		m_shards = GetComponentInChildren<ParticleSystem>();
	}

	/// <summary>
	/// Resets the aestroid upon spawning.
	/// </summary>
	private void OnEnable()
	{
		m_decayTimer = 0;
		m_timer = 0;
		transform.localScale = m_normalScale;
	}

	private void Update()
	{
		m_decayTimer += Time.deltaTime;

		if (m_decayTimer >= m_decayMaxTime)
		{
			Perish();
		}
	}

	/// <summary>
	/// Adds an item for this object to drop upon being destroyed by a player.
	/// </summary>
	/// <param name="item">A reference to a prefab of an item a player can pick up.</param>
	public void AddItem(GameObject item)
	{
		m_item = item;
		m_item.transform.localPosition = Vector3.zero;
		m_item.SetActive(false);
	}

	/// <summary>
	/// Releases the item being carried by this object at the current location of the aestroid.
	/// </summary>
	/// <returns>The GameObject this asteroid is holding as a droppable item.</returns>
	public GameObject ReleaseItem()
	{
		if (m_item)
		{
			GameObject item = m_item;
			m_item = null;

			item.transform.position = this.transform.position;
			item.SetActive(true);
			return item;
		}
		return null;
	}

    /// <summary>
    /// The trigger for the animation upon being destroyed or timing out.
    /// </summary>
	public void Perish()
	{
		m_shards.Play();
        if (!isPerishing)
        {
            StartCoroutine("Perishing");
        }
	}

    /// <summary>
    /// The actual coroutine that handles the animation of the asteroid.
    /// </summary>
	private IEnumerator Perishing()
	{
		while (m_timer <= m_animationTime)
		{
			isPerishing = true;
			m_timer += Time.deltaTime;

			float t = m_timer / m_animationTime;
			t = Interpolation.SineInOut(t);
			transform.localScale = Vector3.LerpUnclamped(m_normalScale, Vector3.zero, t);

			yield return null;
		}
		gameObject.SetActive(false);
		isPerishing = false;
	}
}
