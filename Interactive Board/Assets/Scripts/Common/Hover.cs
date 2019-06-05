using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A MonoBehaviour that adds a small hover animation to a gameobject
/// </summary>
public class Hover : MonoBehaviour
{
	[Header ("Hover Settings")]
	[Tooltip ("The direction the hover will go.")]
	public Vector2 direction;
	[Tooltip ("The frequency/period of the hover effect.")]
	[SerializeField] float frequency = 0f;
	[Tooltip("The amplitude of the hover effect.")]
	[SerializeField] float amplitude = 0f;
	[Tooltip("The offset in the sin equation for the hover")]
	[SerializeField] float offset = 0f;

	/// <summary>
	/// The recorded starting position of the gameobject 
	/// </summary>
	Vector2 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
		startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
		transform.position = amplitude * Mathf.Sin(frequency * (Time.timeSinceLevelLoad - offset)) * direction + startingPosition;
    }
}
