using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions
{
    public static float ZAngleTo(this Vector2 center, Vector2 target)
    {
        Vector2 toTarget = target - center;
        return Mathf.Rad2Deg * Mathf.Atan2(toTarget.y, toTarget.x) - 90;
    }
    
    public static float ZAngleTo(this Vector3 center, Vector3 target)
    {
        Vector2 toTarget = target - center;
        return Mathf.Rad2Deg * Mathf.Atan2(toTarget.y, toTarget.x) - 90;
    }

    public static float ZAngle(this Vector2 direction)
    {
        return Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) - 90;
    }

    public static float ZAngle(this Vector3 direction)
    {
        return Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) - 90;
    }
}
