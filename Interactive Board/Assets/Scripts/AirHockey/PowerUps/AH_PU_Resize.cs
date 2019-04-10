using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_PU_Resize : AH_PowerUp
{
    public override void CauseEffect(AH_Puck puck)
    {
        DebugLog("Size Increase");
        puck.transform.localScale = new Vector3(10.0f, 10.0f);
        DebugLog(puck.transform.localScale.ToString());
    }
}
