﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ABR_Ship))]
public class ABR_ShipReactable : Reactable
{
    private ABR_Ship m_target = null;

    private void Start()
    {
        m_target = GetComponent<ABR_Ship>();
    }

    public override Vector3 LocalVelocity => transform.InverseTransformDirection(m_target.m_RigidBody.velocity);

    public override Vector3 LocalAcceleration => transform.InverseTransformDirection(m_target.m_acceleration);

    public override Vector3 ReactionVecter => Vector3.zero;

    public override float AngularVelocity => m_target.m_RigidBody.angularVelocity;

    public override float AngularAcceleration => m_target.m_angularAcceleration;

    public override float ReactionValue => m_target.m_ThrustMult;
}
