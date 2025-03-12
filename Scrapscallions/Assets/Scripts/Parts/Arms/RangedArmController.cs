using Scraps.AI.GOAP;
using Scraps.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    public class RangedArmController : ArmController
    {
        [SerializeField] protected Sensor m_opponentCloseSensor;
        [SerializeField] bool m_aimsWithDown;

        public override void GetBeliefs(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs)
        {
            BeliefFactory beliefFactory = new(agent, agentBeliefs);

            beliefFactory.AddBelief(side.ToString() + "ArmNotTooClose", () => !m_opponentCloseSensor.IsTargetInRange);
            beliefFactory.AddBelief(side.ToString() + "ArmNotInRange", () => !m_attackRangeSensor.IsTargetInRange);
            beliefFactory.AddBelief(side.ToString() + "ArmAttacking", () => m_actionController.IsTakingAction);
            beliefFactory.AddBelief(side.ToString() + "ArmReady", () => m_actionController.IsReady);
            beliefFactory.AddBelief(side.ToString() + "ArmNotBroken", () => !isBroken);
            beliefFactory.AddBelief(side.ToString() + "ArmInAttackRange", () => m_attackRangeSensor.IsTargetInRange && !m_opponentCloseSensor.IsTargetInRange);
            beliefFactory.AddSensorBelief(side.ToString() + "ArmTooClose", m_opponentCloseSensor);
            beliefFactory.AddBelief(side.ToString() + "ArmFacingOpponent", () => m_facingOpponent);
        }

        public override void GetActions(GoapAgent agent, SerializableHashSet<AgentAction> actions, Dictionary<string, AgentBelief> agentBeliefs)
        {
            actions.Add(
                new AgentAction.Builder(side.ToString() + m_actionController.ActionName)
                //.WithStrategy(ScriptableObject.CreateInstance<TakeActionStrategy>().Initialize(m_actionController))
                .WithStrategy(ScriptableObject.CreateInstance<TakeActionStrategy>().Initialize(m_actionController))
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmInAttackRange"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmReady"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmNotTooClose"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmFacingOpponent"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmNotBroken"])
                .WithPrecondition(agentBeliefs["Alive"])
                .AddEffect(agentBeliefs["AttackingOpponent"])
                .AddEffect(agentBeliefs[side.ToString() + "ArmAttacking"])
                .Build()
            );
            actions.Add(
                new AgentAction.Builder("Move Into " + side.ToString() + "Arm Attack Range")
                .WithStrategy(ScriptableObject.CreateInstance<MoveToStrategy>().Initialize(agent.robot.State, () => agent.robot.State.target().transform.position, m_attackRangeSensor.detectionRadius))
                .AddEffect(agentBeliefs[side.ToString() + "ArmInAttackRange"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmNotBroken"])
                .WithPrecondition(agentBeliefs["Alive"])
                .WithPrecondition(agentBeliefs["CanMove"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmNotInRange"])
                .Build()
            );
            actions.Add(
                new AgentAction.Builder("MoveAwayFrom" + side.ToString() + "ArmCloseRange")
                .WithStrategy(ScriptableObject.CreateInstance<MoveFromStrategy>().Initialize(agent.robot.State,
                () => agent.robot.State.target().transform.position, m_opponentCloseSensor.detectionRadius))
                .AddEffect(agentBeliefs[side.ToString() + "ArmNotTooClose"])
                .AddEffect(agentBeliefs[side.ToString() + "ArmInAttackRange"])
                .WithPrecondition(agentBeliefs["Alive"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmTooClose"])
                .WithPrecondition(agentBeliefs["CanMove"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmNotBroken"])
                .Build()
            );
        }

        public override void GetGoals(GoapAgent agent, SerializableHashSet<AgentGoal> goals, Dictionary<string, AgentBelief> agentBeliefs)
        {
            base.GetGoals(agent, goals, agentBeliefs);
        }
    }
}