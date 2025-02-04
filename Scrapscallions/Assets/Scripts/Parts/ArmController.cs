using Scraps.AI.GOAP;
using Scraps.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    public class ArmController : MonoBehaviour, IPartController
    {
        public Side side;
        [SerializeField] private Sensor m_attackRangeSensor;
        private bool m_isAttacking = false;

        virtual public void Break()
        {
            throw new System.NotImplementedException();
        }

        virtual public void Hit(int damage)
        {
            throw new System.NotImplementedException();
        }

        virtual public void Repair(int amount)
        {
            throw new System.NotImplementedException();
        }

        virtual public void SetParent(Transform parent)
        {
            throw new System.NotImplementedException();
        }

        public virtual void GetBeliefs(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs)
        {
            BeliefFactory beliefFactory = new BeliefFactory(agent, agentBeliefs);
            beliefFactory.AddBelief(side.ToString() + "ArmAttacking", () => m_isAttacking);
            beliefFactory.AddSensorBelief(side.ToString() + "ArmInAttackRange", m_attackRangeSensor);
        }

        public virtual SerializableHashSet<AgentAction> GetActions(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs)
        {
            SerializableHashSet<AgentAction> actions = new()
            {
                new AgentAction.Builder(side.ToString() + "ArmAttack").WithStrategy(ScriptableObject.CreateInstance<AttackStrategy>().Initialize(1))
                .WithPrecondition(agentBeliefs[side.ToString()+"ArmInAttackRange"])
                .AddEffect(agentBeliefs["AttackingOpponent"])
                .AddEffect(agentBeliefs[side.ToString() + "ArmAttacking"])
                .Build()
            };
            return actions;
        }

        public virtual SerializableHashSet<AgentGoal> GetGoals(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs)
        {
            return new();
        }

        public enum Side
        {
            Left,
            Right
        }
    }
}