﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_FollowMouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
