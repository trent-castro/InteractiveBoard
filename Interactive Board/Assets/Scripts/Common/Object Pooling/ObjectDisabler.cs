using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that when added onto an object that has collisions will disable anything that collides with it.
/// </summary>
public class ObjectDisabler : MonoBehaviour {
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
        gameObject.transform.position = transform.parent.position;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);
        gameObject.transform.position = transform.parent.position;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }
}
