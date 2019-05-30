using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reactable : MonoBehaviour
{
    public abstract Vector3 LocalVelocity { get; }
    public abstract Vector3 LocalAcceleration { get; }
    public abstract Vector3 ReactionVecter { get; }
    public abstract float AngularVelocity { get; }
    public abstract float AngularAcceleration { get; }
    public abstract float ReactionValue { get; }
}
