using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum eReactionType
{
    ANGULAR_VELOCITY,
    ANGULAR_ACCELERATION,
    LINEAR_VELOCITY,
    LINEAR_ACCELERATION,
    REACTION_VECTOR,
    REACTION_VALUE
}

[Serializable]
public struct Reaction
{
    public eReactionType reactionType;

    public float max;
    public float min;

    public Vector3 normal;

    public bool usePosition;
    public bool useRotation;
    public bool useScale;

    [Space]
    [Header("Position Reaction")]
    public SplineComponent path;

    [Space]
    [Header("Rotation Reaction")]
    public Vector3 maxRotation;
    public Vector3 minRotation;

    [Space]
    [Header("Scale Reaction")]
    public Vector3 maxScale;
    public Vector3 minScale;

    public float GetT(float scalar)
    {
        return Mathf.Clamp01((scalar - min) / (max - min));
    }

    public float GetT(Vector3 vector)
    {
        vector = Vector3.Project(vector, normal);
        float magnitude = vector.magnitude * (Vector3.Dot(vector, normal) > 0 ? 1 : -1);

        return GetT(magnitude);
    }

    public Vector3 GetPosition(float t, bool localSpace = true)
    {
        return path.GetNonUniformPoint(t, localSpace);
    }

    public Vector3 GetRotation(float t)
    {
        return Vector3.Lerp(minRotation, maxRotation, t);
    }

    public Vector3 GetScale(float t)
    {
        return Vector3.Lerp(minScale, maxScale, t);
    }
}