using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_PU_Stop : AH_PickUp
{
    public override void OnEffectBegin()
    {
        afflictedPuck.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }
}
