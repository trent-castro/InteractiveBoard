using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_PU_Fission : AH_PickUp
{
    [Header("Configuration")]
    [SerializeField]
    [Tooltip("A reference to the puck prefab.")]
    AH_Puck PuckPrefab;

    public override void OnEffectBegin()
    {
        AH_Puck newPuck = Instantiate(PuckPrefab);
        newPuck.delete = true;
        newPuck.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 0, 0);
    }
}
