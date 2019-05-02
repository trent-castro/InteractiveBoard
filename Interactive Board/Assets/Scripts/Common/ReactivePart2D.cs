using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eReactionType
{
    ANGULAR_VELOCITY,
    ANGULAR_ACCELERATION,
    LINEAR_VELOCITY,
    LINEAR_ACCELERATION
}

[Serializable]
public struct Reaction2D
{
    public float max;
    public float min;
    public SplineComponent path;
    public Vector2 normal;
    public float rotationAtMax;
    public float rotationAtMin;

    public eReactionType reactionType;

    public bool UsePosition { get { return path != null; } }
    public bool UseRotation { get { return !(rotationAtMax == 0 && rotationAtMin == 0); } }

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

    public Vector3 GetPosition(float t)
    {
        return path.GetPoint(t);
    }

    public float GetRotation(float t)
    {
        return Mathf.Lerp(rotationAtMin, rotationAtMax, t);
    }
}

public class ReactivePart2D : MonoBehaviour
{
    private Vector2 m_lastLinearVelocity = Vector3.zero;
    private float m_lastAngularVelocity = 0;

    [SerializeField]
    private Rigidbody2D m_rigidbody2D = null;

    [SerializeField]
    private Reaction2D[] reactions = new Reaction2D[0];

    void Update()
    {
        int positions = 0;
        Vector3 position = Vector3.zero;
        int rotations = 0;
        float rotation = 0;


        Vector2 velocity = Quaternion.Inverse(transform.rotation) * m_rigidbody2D.velocity;
        Debug.Log(velocity);
        Vector3 acceleration = Quaternion.Inverse(transform.rotation) * m_rigidbody2D.GetComponent<ABR_Ship>().m_acceleration;
        Debug.Log(acceleration);
        m_lastLinearVelocity = velocity;
        float angularAcceleration = (m_rigidbody2D.angularVelocity - m_lastAngularVelocity);
        m_lastAngularVelocity = m_rigidbody2D.angularVelocity;


        float t = 0;
        foreach (Reaction2D reaction in reactions)
        {
            switch (reaction.reactionType)
            {
                case eReactionType.ANGULAR_VELOCITY:
                    t = reaction.GetT(m_rigidbody2D.angularVelocity);
                    break;
                case eReactionType.ANGULAR_ACCELERATION:
                    t = reaction.GetT(angularAcceleration);
                    break;
                case eReactionType.LINEAR_VELOCITY:
                    t = reaction.GetT(velocity);
                    break;
                case eReactionType.LINEAR_ACCELERATION:
                    t = reaction.GetT(acceleration);
                    break;
            }

            //if (t < 0 || t > 1)
            //{
            //    continue;
            //}

            if (reaction.UsePosition)
            {
                position += reaction.GetPosition(t);
                positions++;
            }

            if (reaction.UseRotation)
            {
                rotation += reaction.GetRotation(t);
                rotations++;
            }
        }

        position /= positions;
        rotation /= rotations;

        transform.localPosition = Vector3.Lerp(transform.localPosition, position, Time.deltaTime * 5);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, rotation), Time.deltaTime * 5);
    }
}
