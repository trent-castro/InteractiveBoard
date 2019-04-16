using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_PU_Invisible : AH_PickUp
{
    public override void OnEffectBegin()
    {
        afflictedPuck.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    public override void OnEffectEnd()
    {
        afflictedPuck.GetComponentInChildren<SpriteRenderer>().enabled = true;
    }
}