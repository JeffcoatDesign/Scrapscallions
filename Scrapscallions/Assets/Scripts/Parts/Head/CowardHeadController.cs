using Scraps.AI.GOAP;
using Scraps.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using static Scraps.Parts.ArmController;

namespace Scraps.Parts
{
    public class CowardHeadController : HeadController
    {
        [SerializeField] private float m_fleeDistance = 10f;


        override public void GetActions(GoapAgent agent, SerializableHashSet<AgentAction> actions, Dictionary<string, AgentBelief> agentBeliefs)
        {
            actions.Add(
                new AgentAction.Builder("RunAway")
                .WithStrategy(ScriptableObject.CreateInstance<MoveFromStrategy>().Initialize(agent.robot.State,
                () => agent.robot.State.target().transform.position, 10))
                .AddEffect(agentBeliefs["RanAway"])
                .WithPrecondition(agentBeliefs["IsScared"])
                .Build()
            );
        }

        override public void GetBeliefs(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs)
        {
            BeliefFactory beliefFactory = new(agent, agentBeliefs);
            beliefFactory.AddBelief("IsScared", () => m_robot.body.CurrentHP < m_robot.body.MaxHP * 0.75f);
            beliefFactory.AddBelief("RanAway", () => Vector3.Distance(agent.transform.position, m_robot.State.target().transform.position) > m_fleeDistance);
        }

        override public void GetGoals(GoapAgent agent, SerializableHashSet<AgentGoal> goals, Dictionary<string, AgentBelief> agentBeliefs)
        {
            goals.Add(new AgentGoal.Builder("Flee").WithPriority(4).WithDesiredEffect(agentBeliefs["RanAway"]).Build());
        }

        override public void Hit(int damage)
        {
            int currentHP = head.CurrentHP - damage;
            if (currentHP <= 0)
            {
                currentHP = 0;
                Break();
            }
            head.CurrentHP = currentHP;

            PartHit?.Invoke(damage);
        }
    }
}