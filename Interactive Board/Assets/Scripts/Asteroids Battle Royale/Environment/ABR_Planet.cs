using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_Planet : MonoBehaviour
{
	[SerializeField] private float RotateSpeed = .1f;
	[SerializeField] private float xAxis = 5;
	[SerializeField] private float yAxis = 10;
	private Vector2 _centre = Vector2.zero;
	private float _angle = 0f;


	private void Start()
	{
		_centre = transform.localPosition;
	}

	private void Update()
	{

		_angle += RotateSpeed * Time.deltaTime;
		if (_angle > 360) _angle = _angle % 360;

		var offset = Evaluate(_angle);
		transform.localPosition = _centre + offset;
	}

	public Vector2 Evaluate(float t)
	{
		float angle = Mathf.Deg2Rad * 360 * t;
		float x = Mathf.Sin(angle) * xAxis;
		float y = Mathf.Cos(angle) * yAxis;

		return new Vector2(x, y);
	}
}



