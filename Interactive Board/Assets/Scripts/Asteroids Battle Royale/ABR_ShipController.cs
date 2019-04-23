using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ABR_ShipController : MonoBehaviour
{
    private Rigidbody2D m_rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        m_rigidBody.velocity = Vector3.Lerp(m_rigidBody.velocity, new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * 50, Time.deltaTime);
    }
}
