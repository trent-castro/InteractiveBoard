using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AH_PU_Velocity : AH_PickUp
{
    [Header("Pick Up Configuration")]
    [SerializeField] [Tooltip("New velocity scale the puck.")]
    private float m_velocityScale = 0.0f;

    public override void OnEffectBegin()
    {
        afflictedPuck.GetComponent<AH_Puck>().SetMaxSpeed(afflictedPuck.GetComponent<AH_Puck>().GetMaxSpeed() * m_velocityScale);
    }

    public override void OnEffectEnd()
    {
        afflictedPuck.GetComponent<AH_Puck>().SetMaxSpeed(afflictedPuck.GetComponent<AH_Puck>().GetMaxSpeed() / m_velocityScale);
    }
}
