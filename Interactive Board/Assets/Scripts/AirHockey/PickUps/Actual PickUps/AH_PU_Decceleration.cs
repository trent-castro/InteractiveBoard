using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_PU_Decceleration : AH_PickUp
{
    public override void OnEffectBegin()
    {
        afflictedPuck.GetComponent<Rigidbody2D>().velocity = afflictedPuck.GetComponent<Rigidbody2D>().velocity / 2;
    }

    public override void OnEffectEnd()
    {
        afflictedPuck.GetComponent<Rigidbody2D>().velocity = afflictedPuck.GetComponent<Rigidbody2D>().velocity * 2;
    }
}