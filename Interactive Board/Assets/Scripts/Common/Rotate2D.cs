using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate2D : MonoBehaviour
{
    [SerializeField] float m_RotationSpeed = 0.0f;

    void Update()
    {
        transform.Rotate(Vector3.forward * (m_RotationSpeed * Time.deltaTime));
    }
}
