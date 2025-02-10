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
        [SerializeField] protected Sensor m_attackRangeSensor;
        [SerializeField] protected float m_timeBetweenAttacks = 3f;

        protected bool m_isAttacking = false;
        protected bool m_isReady = true;

        private CountdownTimer m_attackReadyTimer;

        virtual public void Attack()
        {
            m_isAttacking = true;
            m_isReady = false;
            transform.localRotation = Quaternion.Euler(270f, 0f, 0f);
        }

        virtual public void Idle()
        {
            m_isAttacking = false;
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            m_attackReadyTimer = new(m_timeBetweenAttacks);
            m_attackReadyTimer.OnTimerStop += Ready;
            m_attackReadyTimer.Start();
        }

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
            beliefFactory.AddBelief(side.ToString() + "ArmReady", () => m_isReady);
            beliefFactory.AddSensorBelief(side.ToString() + "ArmInAttackRange", m_attackRangeSensor);
        }

        public virtual void GetActions(GoapAgent agent, SerializableHashSet<AgentAction> actions, Dictionary<string, AgentBelief> agentBeliefs)
        {
            actions.Add(
                new AgentAction.Builder(side.ToString() + "ArmAttack")
                .WithStrategy(ScriptableObject.CreateInstance<AttackStrategy>().Initialize(this, 2))
                .WithPrecondition(agentBeliefs[side.ToString()+"ArmInAttackRange"])
                .WithPrecondition(agentBeliefs[side.ToString()+"ArmReady"])
                .AddEffect(agentBeliefs["AttackingOpponent"])
                .AddEffect(agentBeliefs[side.ToString() + "ArmAttacking"])
                .Build()
            );
            actions.Add(
                new AgentAction.Builder("Move Into " + side.ToString() + "Arm Attack Range")
                .WithStrategy(ScriptableObject.CreateInstance<MoveToStrategy>().Initialize(agent.robot, () => agent.robot.State.target().transform.position))
                .AddEffect(agentBeliefs[side.ToString() + "ArmInAttackRange"])
                .Build()
            );
        }

        public virtual void GetGoals(GoapAgent agent, SerializableHashSet<AgentGoal> goals, Dictionary<string, AgentBelief> agentBeliefs)
        {
            return;
        }

        public enum Side
        {
            Left,
            Right
        }

        private void Ready()
        {
            m_isReady = true;
        }
        private void Update()
        {
            if(m_attackReadyTimer != null)
            {
                m_attackReadyTimer.Tick(Time.deltaTime);
            }
        }
    }
}