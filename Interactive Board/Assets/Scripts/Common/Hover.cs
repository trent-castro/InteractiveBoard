using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
	public Vector2 direction;
	[SerializeField] float frequency = 0f;
	[SerializeField] float amplitude = 0f;
	[SerializeField] float offset = 0f;

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
