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
        internal Action Fire;
        [SerializeField] protected Sensor m_opponentCloseSensor;
        [SerializeField] bool m_aimsWithDown;
        public override void Attack()
        {
            m_isAttacking = true;
            m_isReady = false;

            Aim();
            Debug.Log("Firing");
            Invoke("FireWeapon", 1f);

            Attacked?.Invoke();
        }

        public override void Idle()
        {
            base.Idle();
        }

        virtual protected void Aim()
        {
            StartCoroutine(PointWhileAttacking());
        }

        private IEnumerator PointWhileAttacking()
        {
            //TODO Aim at targeted part

            Vector3 right = transform.right;
            while (m_isAttacking)
            {
                if (m_aimsWithDown)
                {
                    Vector3 directionToTarget = (m_robot.State.target().transform.position.With(y: 1.5f) - transform.position).normalized;

                    Vector3 up = Vector3.Cross(directionToTarget, right);
                    Quaternion rotation = Quaternion.LookRotation(up, -directionToTarget);

                    transform.rotation = rotation;

                    Debug.DrawLine(transform.position, transform.position + directionToTarget, Color.blue);
                    Debug.DrawLine(transform.position, transform.position + up, Color.yellow);
                }
                else
                    transform.LookAt(m_robot.State.target().transform.position.With(y: 1.5f), transform.up);

                yield return new WaitForFixedUpdate();
            }
            yield return null;
        }

        private void FireWeapon()
        {
            Fire?.Invoke();
        }

        public override void GetBeliefs(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs)
        {
            BeliefFactory beliefFactory = new(agent, agentBeliefs);

            beliefFactory.AddBelief(side.ToString() + "ArmNotTooClose", () => !m_opponentCloseSensor.IsTargetInRange);
            beliefFactory.AddBelief(side.ToString() + "ArmNotInRange", () => !m_attackRangeSensor.IsTargetInRange);
            beliefFactory.AddBelief(side.ToString() + "ArmAttacking", () => m_isAttacking);
            beliefFactory.AddBelief(side.ToString() + "ArmReady", () => m_isReady);
            beliefFactory.AddBelief(side.ToString() + "ArmWorking", () => !isBroken);
            beliefFactory.AddSensorBelief(side.ToString() + "ArmInAttackRange", m_attackRangeSensor);
            beliefFactory.AddSensorBelief(side.ToString() + "ArmTooClose", m_opponentCloseSensor);
            beliefFactory.AddBelief(side.ToString() + "ArmFacingOpponent", () => m_facingOpponent);
        }

        public override void GetActions(GoapAgent agent, SerializableHashSet<AgentAction> actions, Dictionary<string, AgentBelief> agentBeliefs)
        {
            actions.Add(
                new AgentAction.Builder(side.ToString() + m_actionController.ActionName)
                //.WithStrategy(ScriptableObject.CreateInstance<TakeActionStrategy>().Initialize(m_actionController))
                .WithStrategy(ScriptableObject.CreateInstance<AttackStrategy>().Initialize(this, 2))
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmInAttackRange"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmReady"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmNotTooClose"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmFacingOpponent"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmWorking"])
                .WithPrecondition(agentBeliefs["Alive"])
                .AddEffect(agentBeliefs["AttackingOpponent"])
                .AddEffect(agentBeliefs[side.ToString() + "ArmAttacking"])
                .Build()
            );
            actions.Add(
                new AgentAction.Builder("Move Into " + side.ToString() + "Arm Attack Range")
                .WithStrategy(ScriptableObject.CreateInstance<MoveToStrategy>().Initialize(agent.robot.State, () => agent.robot.State.target().transform.position, m_attackRangeSensor.detectionRadius))
                .AddEffect(agentBeliefs[side.ToString() + "ArmInAttackRange"])
                .WithPrecondition(agentBeliefs["Alive"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmNotInRange"])
                .Build()
            );
            actions.Add(
                new AgentAction.Builder("MoveAwayFrom" + side.ToString() + "ArmCloseRange")
                .WithStrategy(ScriptableObject.CreateInstance<MoveFromStrategy>().Initialize(agent.robot.State, 
                () => agent.robot.State.target().transform.position, m_opponentCloseSensor.detectionRadius))
                .AddEffect(agentBeliefs[side.ToString() + "ArmNotTooClose"])
                .WithPrecondition(agentBeliefs["Alive"])
                .WithPrecondition(agentBeliefs[side.ToString() + "ArmTooClose"])
                .Build()
            );
        }

        public override void GetGoals(GoapAgent agent, SerializableHashSet<AgentGoal> goals, Dictionary<string, AgentBelief> agentBeliefs)
        {
            base.GetGoals(agent, goals, agentBeliefs);
        }
    }
}