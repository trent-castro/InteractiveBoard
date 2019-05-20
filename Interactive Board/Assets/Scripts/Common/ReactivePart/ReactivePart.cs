using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactivePart : MonoBehaviour
{
    [SerializeField]
    private Reaction[] m_reactions = null;

    [SerializeField]
    private Reactable m_reactable = null;

    [SerializeField]
    private float m_reactionTime = 5;

    void Update()
    {
        int positions = 0;
        Vector3 position = Vector3.zero;

        int rotations = 0;
        Vector3 rotation = Vector3.zero;

        int scales = 0;
        Vector3 scale = Vector3.zero;

        float t = 0;
        foreach (Reaction reaction in m_reactions)
        {
            switch (reaction.reactionType)
            {
                case eReactionType.ANGULAR_VELOCITY:
                    t = reaction.GetT(m_reactable.AngularVelocity);
                    break;
                case eReactionType.ANGULAR_ACCELERATION:
                    t = reaction.GetT(m_reactable.AngularAcceleration);
                    break;
                case eReactionType.LINEAR_VELOCITY:
                    t = reaction.GetT(m_reactable.LocalVelocity);
                    break;
                case eReactionType.LINEAR_ACCELERATION:
                    t = reaction.GetT(m_reactable.LocalAcceleration);
                    break;
                case eReactionType.REACTION_VECTOR:
                    t = reaction.GetT(m_reactable.ReactionVecter);
                    break;
                case eReactionType.REACTION_VALUE:
                    t = reaction.GetT(m_reactable.ReactionValue);
                    break;
            }

            if (reaction.usePosition)
            {
                position += reaction.GetPosition(t);
                positions++;
            }

            if (reaction.useRotation)
            {
                rotation += reaction.GetRotation(t);
                rotations++;
            }

            if (reaction.useScale)
            {
                scale += reaction.GetScale(t);
                scales++;
            }
        }

        if (positions == 0)
        {
            position = Vector3.zero;
        }
        else
        {
            position /= positions;
        }

        if (rotations == 0)
        {
            rotation = Vector3.zero;
        }
        else
        {
            rotation /= rotations;
        }

        if (scales == 0)
        {
            scale = Vector3.one;
        }
        else
        {
            scale /= scales;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, position, Time.deltaTime * m_reactionTime);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rotation), Time.deltaTime * m_reactionTime);
        transform.localScale = Vector3.Lerp(transform.localScale, scale, Time.deltaTime * m_reactionTime);
    }
}
