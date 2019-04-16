using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_PU_Invisible : AH_PickUp
{
    public override void OnEffectBegin()
    {
<<<<<<< HEAD
<<<<<<< HEAD
        afflictedPuck.GetComponentInChildren<SpriteRenderer>().enabled = false;
=======
        afflictedPuck.GetComponent<SpriteRenderer>().enabled = false;
>>>>>>> parent of e0f8210... Merge branch 'master' into Lmckamey-ParticleSystems
=======
        afflictedPuck.GetComponent<SpriteRenderer>().enabled = false;
>>>>>>> parent of e0f8210... Merge branch 'master' into Lmckamey-ParticleSystems
    }

    public override void OnEffectEnd()
    {
<<<<<<< HEAD
<<<<<<< HEAD
        afflictedPuck.GetComponentInChildren<SpriteRenderer>().enabled = true;
=======
        afflictedPuck.GetComponent<SpriteRenderer>().enabled = true;
>>>>>>> parent of e0f8210... Merge branch 'master' into Lmckamey-ParticleSystems
=======
        afflictedPuck.GetComponent<SpriteRenderer>().enabled = true;
>>>>>>> parent of e0f8210... Merge branch 'master' into Lmckamey-ParticleSystems
    }
}