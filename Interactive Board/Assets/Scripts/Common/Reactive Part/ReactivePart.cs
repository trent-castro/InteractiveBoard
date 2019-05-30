using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ECombineStrategy
{
    ADD,
    AVERAGE,
    HIGHEST,
    LOWEST
}

public class ReactivePart : MonoBehaviour
{
    [SerializeField]
    private Reaction[] m_reactions = null;

    [SerializeField]
    private Reactable m_reactable = null;

    [SerializeField]
    private float m_reactionTime = 5;

    private Vector3[] m_positions = null;
    private Vector3[] m_rotations = null;
    private Vector3[] m_scales = null;

    [SerializeField]
    private ECombineStrategy m_positionStrategy = ECombineStrategy.AVERAGE;

    [SerializeField]
    private ECombineStrategy m_rotationStrategy = ECombineStrategy.AVERAGE;

    [SerializeField]
    private ECombineStrategy m_scaleStrategy = ECombineStrategy.AVERAGE;

    private void Start()
    {
        m_positions = new Vector3[m_reactions.Length];
        m_rotations = new Vector3[m_reactions.Length];
        m_scales = new Vector3[m_reactions.Length];
    }

    void Update()
    {
        int positionCount = 0;

        int rotationCount = 0;

        int scaleCount = 0;

        float t = 0;
        foreach (Reaction reaction in m_reactions)
        {
            switch (reaction.reactionType)
            {
                case EReactionType.ANGULAR_VELOCITY:
                    t = reaction.GetT(m_reactable.AngularVelocity);
                    break;
                case EReactionType.ANGULAR_ACCELERATION:
                    t = reaction.GetT(m_reactable.AngularAcceleration);
                    break;
                case EReactionType.LINEAR_VELOCITY:
                    t = reaction.GetT(m_reactable.LocalVelocity);
                    break;
                case EReactionType.LINEAR_ACCELERATION:
                    t = reaction.GetT(m_reactable.LocalAcceleration);
                    break;
                case EReactionType.REACTION_VECTOR:
                    t = reaction.GetT(m_reactable.ReactionVecter);
                    break;
                case EReactionType.REACTION_VALUE:
                    t = reaction.GetT(m_reactable.ReactionValue);
                    break;
            }

            AddNext(reaction.usePosition, reaction.GetPosition(t), m_positions, ref positionCount);
            AddNext(reaction.useRotation, reaction.GetRotation(t), m_rotations, ref rotationCount);
            AddNext(reaction.useScale, reaction.GetScale(t), m_scales, ref scaleCount);
        }

        Vector3 position = DoStrategy(m_positionStrategy, m_positions, positionCount, Vector3.zero);
        Vector3 rotation = DoStrategy(m_rotationStrategy, m_rotations, rotationCount, Vector3.zero);
        Vector3 scale = DoStrategy(m_scaleStrategy, m_scales, scaleCount, Vector3.one);

        transform.localPosition = Vector3.Lerp(transform.localPosition, position, Time.deltaTime * m_reactionTime);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rotation), Time.deltaTime * m_reactionTime);
        transform.localScale = Vector3.Lerp(transform.localScale, scale, Time.deltaTime * m_reactionTime);
    }

    private void AddNext(bool useReactionType, Vector3 value, Vector3[] values, ref int count)
    {
        if (useReactionType)
        {
            values[count] = value;
            count++;
        }
    }

    private Vector3 DoStrategy(ECombineStrategy strategy, Vector3[] values, int count, Vector3 defaultResult)
    {
        if (count == 0) return defaultResult;

        Vector3 result = Vector3.zero;

        switch (strategy)
        {
            case ECombineStrategy.ADD:
                for (int i = 0; i < count; i++)
                {
                    result += values[i];
                }
                break;
            case ECombineStrategy.AVERAGE:
                for (int i = 0; i < count; i++)
                {
                    result += values[i];
                }
                result /= count;
                break;
            case ECombineStrategy.HIGHEST:
                result = values[0];
                for (int i = 1; i < count; i++)
                {
                    if (values[i].sqrMagnitude > result.sqrMagnitude)
                    {
                        result = values[i];
                    }
                }
                break;
            case ECombineStrategy.LOWEST:
                result = values[0];
                for (int i = 1; i < count; i++)
                {
                    if (values[i].sqrMagnitude < result.sqrMagnitude)
                    {
                        result = values[i];
                    }
                }
                break;
        }

        return result;
    }
}
