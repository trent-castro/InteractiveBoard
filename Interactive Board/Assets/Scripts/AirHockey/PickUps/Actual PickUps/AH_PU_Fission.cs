using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_PU_Fission : AH_PickUp
{
    public override void OnEffectBegin()
    {
        AH_Puck newPuck = Instantiate(afflictedPuck);
        newPuck.delete = true;
    }
}
