using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_BasicBullet : ABR_Bullet
{
    public override void Fire(Vector2 direction)
    {
        movementDirection = direction;
    }
}
