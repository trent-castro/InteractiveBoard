﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_PU_Invisible : AH_PickUp
{
    public override void OnEffectBegin()
    {
        afflictedPuck.GetComponent<SpriteRenderer>().enabled = false;
    }

    public override void OnEffectEnd()
    {
        afflictedPuck.GetComponent<SpriteRenderer>().enabled = true;
    }
}