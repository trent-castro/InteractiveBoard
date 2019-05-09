using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_PU_Invisible : AH_PickUp
{
    public override void OnEffectBegin()
    {
		afflictedPuck.SetImageActive(false);
        afflictedPuck.GetComponentInChildren<TrailRenderer>().enabled = false;
    }

    public override void OnEffectEnd()
    {
		afflictedPuck.SetImageActive(true);
        afflictedPuck.GetComponentInChildren<TrailRenderer>().enabled = true;
    }
}