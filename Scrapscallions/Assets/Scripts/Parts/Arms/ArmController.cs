using Scraps.AI.GOAP;
using Scraps.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    public class ArmController : PartController
    {
        public Side side;
        [SerializeField] protected Sensor m_attackRangeSensor;
        [SerializeField] protected float m_timeBetweenAttacks = 3f;
        [SerializeField] private float m_facingThreshold = 0.9f;
        [SerializeField] protected ActionController m_actionController;

        public RobotPartArm arm;

        protected bool m_facingOpponent = true;

        private CountdownTimer m_attackReadyTimer;

        //TODO Check if the body breaks through Robot.body.Break action
        // ALSO add as precondition to the attack action

        override public void Hit(int damage)
        {
            int currentHP = arm.CurrentHP - damage;
            if (currentHP <= 0)
            {
                currentHP = 0;
                Break();
            }
            arm.CurrentHP = currentHP;
            PartHit?.Invoke(damage / arm.MaxHP);
        }

        /// <summary>
        /// Generally repairing is not something the arm controller will do, unless a special part has an ability for it.
        /// </summary>
        /// <param name="amount">The amount of HP to gain.</param>
        override public void Repair(int amount)
        {
            //NOOP
        }

        override public void GetBeliefs(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs)
        {
            BeliefFactory beliefFactory = new BeliefFactory(agent, agentBeliefs);
            beliefFactory.AddBelief(side.ToString() + "ArmAttacking", () => m_actionController.IsTakingAction);
            beliefFactory.AddBelief(side.ToString() + "ArmReady", () => m_actionController.IsReady);
            beliefFactory.AddBelief(side.ToString() + "ArmWorking", () => !isBroken);
            beliefFactory.AddBelief(side.ToString() + "ArmFacingOpponent", () => m_facingOpponent);
            //beliefFactory.AddLocationBelief(side.ToString() + "ArmInAttackRange", 3f, () => m_robot.State.target().transform.position);
            beliefFactory.AddSensorBelief(side.ToString() + "ArmInAttackRange", m_attackRangeSensor);
        }

        override public void GetActions(GoapAgent agent, SerializableHashSet<AgentAction> actions, Dictionary<string, AgentBelief> agentBeliefs)
        {
            actions.Add(
                new AgentAction.Builder(side.ToString() + m_actionController.ActionName)
                .WithStrategy(ScriptableObject.CreateInstance<TakeActionStrategy>().Initialize(m_actionController))
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmInAttackRange"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmFacingOpponent"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmReady"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmWorking"])
                .WithPrecondition(agentBeliefs["IsPursuing"])
                .WithPrecondition(agentBeliefs["Alive"])
                .AddEffect(agentBeliefs["AttackingOpponent"])
                .AddEffect(agentBeliefs[side.ToString() + "ArmAttacking"])
                .Build()
            );
            actions.Add(
                new AgentAction.Builder("MoveInto" + side.ToString() + "ArmAttackRange")
                .WithStrategy(ScriptableObject.CreateInstance<MoveToStrategy>().Initialize(agent.robot.State, () => agent.robot.State.target().transform.position, 2))
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmWorking"])
                .WithPrecondition(agentBeliefs["Alive"])
                .AddEffect(agentBeliefs[side.ToString() + "ArmInAttackRange"])
                .AddEffect(agentBeliefs["IsPursuing"])
                .Build()
            );
        }

        override public void GetGoals(GoapAgent agent, SerializableHashSet<AgentGoal> goals, Dictionary<string, AgentBelief> agentBeliefs)
        {
            return;
        }

        public enum Side
        {
            Left,
            Right
        }
        private void Update()
        {
            if (isBroken)
                return;

            if(m_attackReadyTimer != null)
            {
                m_attackReadyTimer.Tick(Time.deltaTime);
            }

            CheckFacing();
        }

        private void CheckFacing()
        {
            Vector3 position = transform.position.With(y: 0);
            Vector3 opponentPosition = m_robot.State.target().transform.position.With(y:0);

            m_facingOpponent = Vector3.Dot(m_robot.State.Agent.transform.forward, opponentPosition - position) > m_facingThreshold;
        }
    }
}