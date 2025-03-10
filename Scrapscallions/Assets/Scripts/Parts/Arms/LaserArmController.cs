using Scraps.AI.GOAP;
using Scraps.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    [RequireComponent(typeof(PowerUpController))]
    public class LaserArmController : RangedArmController
    {
        private PowerUpController m_powerUpController;
        private bool m_laserActive = false;
        bool m_isAttacking = false;

        public override void Initialize(Robot robot)
        {
            base.Initialize(robot);
            m_powerUpController = GetComponent<PowerUpController>();
            m_powerUpController.Activated += OnLaserActivated;
            m_powerUpController.ActionCompleted += OnLaserStopped;
        }

        private void OnLaserStopped()
        {
            m_laserActive = false;
            m_isAttacking = false;
        }

        private void OnLaserActivated()
        {
            m_laserActive = true;
            m_isAttacking = true;
        }

        private void Update()
        {
            if (m_laserActive)
            {
                m_actionController.Activate();
            }
        }

        public override void GetBeliefs(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs)
        {
            BeliefFactory beliefFactory = new(agent, agentBeliefs);

            beliefFactory.AddBelief(side.ToString() + "ArmNotTooClose", () => !m_opponentCloseSensor.IsTargetInRange);
            beliefFactory.AddBelief(side.ToString() + "ArmAttacking", () => m_actionController.IsTakingAction);
            beliefFactory.AddBelief(side.ToString() + "ArmReady", () => m_actionController.IsReady);
            beliefFactory.AddBelief(side.ToString() + "ArmWorking", () => !isBroken);
            beliefFactory.AddBelief(side.ToString() + "ArmLaserNotFiring", () => !m_laserActive);
            beliefFactory.AddBelief(side.ToString() + "ArmLaserFiring", () => m_powerUpController.isUsingPowerUp);
            beliefFactory.AddBelief(side.ToString() + "ArmLaserReady", () => m_powerUpController.IsReady);
            beliefFactory.AddSensorBelief(side.ToString() + "ArmInAttackRange", m_attackRangeSensor);
            beliefFactory.AddBelief(side.ToString() + "ArmFacingOpponent", () => m_facingOpponent);
        }

        public override void GetActions(GoapAgent agent, SerializableHashSet<AgentAction> actions, Dictionary<string, AgentBelief> agentBeliefs)
        {
            actions.Add(
                new AgentAction.Builder(side.ToString() + m_actionController.ActionName)
                .WithStrategy(ScriptableObject.CreateInstance<TakeActionStrategy>().Initialize(m_actionController))
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmInAttackRange"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmReady"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmNotTooClose"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmFacingOpponent"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmWorking"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmLaserNotFiring"])
                .WithPrecondition(agentBeliefs["Alive"])
                .AddEffect(agentBeliefs["AttackingOpponent"])
                .AddEffect(agentBeliefs[side.ToString() + "ArmAttacking"])
                .Build()
            );
            actions.Add(
                new AgentAction.Builder("Move Into " + side.ToString() + "Arm Attack Range")
                .WithStrategy(ScriptableObject.CreateInstance<MoveToStrategy>().Initialize(agent.robot.State, () => agent.robot.State.target().transform.position, 9))
                .AddEffect(agentBeliefs[side.ToString() + "ArmInAttackRange"])
                .WithPrecondition(agentBeliefs["Alive"])
                .Build()
            );
            actions.Add(
                new AgentAction.Builder("MoveAwayFrom" + side.ToString() + "ArmCloseRange")
                .WithStrategy(ScriptableObject.CreateInstance<MoveFromStrategy>().Initialize(agent.robot.State, 
                () => agent.robot.State.target().transform.position, 5))
                .AddEffect(agentBeliefs[side.ToString() + "ArmNotTooClose"])
                .WithPrecondition(agentBeliefs["Alive"])
                .Build()
            );

            /* NOT THE PLAYER */
            if (!m_robot.State.isPlayer)
            {
                actions.Add(
                    new AgentAction.Builder(side.ToString() + "ArmFireLaser")
                    .WithStrategy(ScriptableObject.CreateInstance<PowerUpStrategy>().Initialize(m_powerUpController))
                    .WithPrecondition(agentBeliefs[side.ToString() + "ArmInAttackRange"])
                    .WithPrecondition(agentBeliefs[side.ToString() + "ArmReady"])
                    .WithPrecondition(agentBeliefs[side.ToString() + "ArmNotTooClose"])
                    .WithPrecondition(agentBeliefs[side.ToString() + "ArmFacingOpponent"])
                    .WithPrecondition(agentBeliefs[side.ToString() + "ArmWorking"])
                    .WithPrecondition(agentBeliefs[side.ToString() + "ArmLaserNotFiring"])
                    .WithPrecondition(agentBeliefs[side.ToString() + "ArmLaserReady"])
                    .WithPrecondition(agentBeliefs["Alive"])
                    .AddEffect(agentBeliefs["AttackingOpponent"])
                    .AddEffect(agentBeliefs[side.ToString() + "ArmAttacking"])
                    .AddEffect(agentBeliefs[side.ToString() + "ArmLaserFiring"])
                    .Build()
                );
            }
        }

        public override void GetGoals(GoapAgent agent, SerializableHashSet<AgentGoal> goals, Dictionary<string, AgentBelief> agentBeliefs)
        {
            base.GetGoals(agent, goals, agentBeliefs);

            goals.Add(new AgentGoal.Builder("Use" + side.ToString() + "ArmLaser")
                .WithDesiredEffect(agentBeliefs[side.ToString() + "ArmLaserFiring"])
                .WithPriority(4)
                .Build());
        }
    }
}