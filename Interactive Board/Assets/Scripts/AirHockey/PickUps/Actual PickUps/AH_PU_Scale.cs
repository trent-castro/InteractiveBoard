using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_PU_Scale : AH_PickUp
{
    [Header("Pick Up Configuration")]
    [SerializeField] [Tooltip("New size to make the puck.")]
    private float m_puckScale = 0.0f;

    public override void OnEffectBegin()
    {
        afflictedPuck.transform.localScale = afflictedPuck.transform.localScale * m_puckScale;
    }

    public override void OnEffectEnd()
    {
        afflictedPuck.transform.localScale = afflictedPuck.transform.localScale / m_puckScale;
    }
}
