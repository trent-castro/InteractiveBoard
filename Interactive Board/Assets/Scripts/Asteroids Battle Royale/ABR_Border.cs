﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_Border : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerExit2D(Collider2D collision)
	{
		Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
		rb.velocity = rb.velocity * -2;
	}
}
