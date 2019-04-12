using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_PU_Resize : AH_PickUp
{
    public override void OnEffectBegin()
    {
        afflictedPuck.transform.localScale = afflictedPuck.transform.localScale * 2;
    }

    public override void OnEffectEnd()
    {
        afflictedPuck.transform.localScale = afflictedPuck.transform.localScale / 2;
    }
}
