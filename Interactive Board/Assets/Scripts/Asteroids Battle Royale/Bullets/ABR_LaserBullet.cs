﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_LaserBullet : ABR_Bullet
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO damage Logic
    }
    public override void Fire(Vector2 direction)
    {
        movementDirection = direction;
    }
}
