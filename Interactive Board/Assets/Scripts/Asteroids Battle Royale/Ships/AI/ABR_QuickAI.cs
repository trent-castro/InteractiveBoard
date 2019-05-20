using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABR_QuickAI : ABR_StateAI
{
    [Header("Configuration")]
    [Tooltip("The maximum detection space of the state AI.")]
    [SerializeField]
    [Range(0.0f, 1000.0f)]
    private float maxDetectionDistance = 250.0f;

    [Header("Exposed Debug Variables")]
    [SerializeField] private State<ABR_QuickAI> state;

    // Private data members
    protected StackStateMachine<ABR_QuickAI> m_stateMachine;
    private float m_distanceToClosestPlayer = float.MaxValue;

    protected override void DisableAI()
    {
        m_stateMachine.Clear();
        m_currentTarget = null;
        gameObject.SetActive(false);
    }

    protected override void EnableAI()
    {
        m_stateMachine.PushState("WANDER");
        m_distanceToClosestPlayer = float.MaxValue;
        gameObject.SetActive(true);
    }

    protected new void Awake()
    {
        m_stateMachine = new StackStateMachine<ABR_QuickAI>();
        m_stateMachine.AddState("APPROACH", new ApproachState<ABR_QuickAI>(this));
        m_stateMachine.AddState("WANDER", new WanderState<ABR_QuickAI>(this));
        m_stateMachine.PushState("WANDER");

        base.Awake();
    }

    protected new void Update()
    {
        m_stateMachine.Update();

        base.Update();
    }


    class ApproachState<T> : State<T> where T : ABR_QuickAI
    {
        ABR_QuickAI quickAI;

        public ApproachState(T owner)
        {
            m_owner = owner;
            quickAI = m_owner.GetComponent<ABR_QuickAI>();
        }

        public override void Enter()
        {
            quickAI.DebugLog(quickAI.name + " is approaching "
                + quickAI.m_currentTarget.transform.name);
        }

        public override void Update()
        {
            float targetRotation = quickAI.m_currentTarget.transform.position.ZAngleTo(quickAI.transform.position);
            quickAI.TurnTo(targetRotation);

            float distance = Vector3.Distance(m_owner.transform.position, quickAI.m_currentTarget.transform.position);
            if(distance < quickAI.maxDetectionDistance)
            {
                quickAI.Thrust(quickAI.rotationSpeed);
            }
            else
            {
                quickAI.m_stateMachine.PushState("WANDER");
            }
        }

        public override void Exit()
        {
        }
    }

    class WanderState<T> : State<T> where T : ABR_QuickAI
    {
        ABR_QuickAI quickAI;

        public WanderState(T owner)
        {
            m_owner = owner;
            quickAI = m_owner.GetComponent<ABR_QuickAI>();
        }

        public override void Enter()
        {
            quickAI.DebugLog(quickAI.name + " is wandering aimlessly.");
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
            GameObject closest = null;
            float distance = float.MaxValue;
            foreach(GameObject player in quickAI.players)
            {
                float curDistance = (player.transform.position - quickAI.transform.position).sqrMagnitude;
                if(curDistance < distance && player.activeSelf)
                {
                    closest = player;
                    distance = curDistance;
                }
            }
            quickAI.m_distanceToClosestPlayer = distance;
            quickAI.m_currentTarget = closest;

            if(distance < quickAI.maxDetectionDistance)
            {
                m_owner.m_stateMachine.PushState("APPROACH");
            }
        }
    }
}