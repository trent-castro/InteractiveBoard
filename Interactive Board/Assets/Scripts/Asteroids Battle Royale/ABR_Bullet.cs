using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_Bullet : MonoBehaviour
{
    [SerializeField] float lifeTime = 10.0f;
    [SerializeField] float speed = 5.0f;
    [SerializeField] bool DebugMode = false;

    private Vector2 movementDirection = Vector2.zero;
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0.0f)
            gameObject.SetActive(false);

        gameObject.transform.position += (Vector3)(movementDirection * speed);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO put damage and or hit detection logic here
        gameObject.SetActive(false);
    }

    public void Fire(Vector2 direction)
    {
        movementDirection = direction;
    }
}
