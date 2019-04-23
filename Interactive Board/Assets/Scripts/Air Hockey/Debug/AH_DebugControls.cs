using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_DebugControls : MonoBehaviour
{
    Rigidbody2D rigidBody;
    public Vector2 velocity;

    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rigidBody.velocity += velocity;
    }
}
