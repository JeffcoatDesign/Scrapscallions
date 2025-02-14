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
        [SerializeField] float maxPitch = 360, maxRoll = 360, maxYaw = 360;
        [SerializeField] float minPitch = 0, minRoll = 0, minYaw = 0;
        public override void Attack()
        {
            m_isAttacking = true;
            m_isReady = false;

            Aim();
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
            Vector3 right = transform.right;
            while (m_isAttacking)
            {
                Vector3 directionToTarget = (m_robot.State.target().transform.position.With(y:1.5f) - transform.position).normalized;

                Vector3 up = Vector3.Cross(directionToTarget, right);
                Quaternion rotation = Quaternion.LookRotation(up, -directionToTarget);

                transform.rotation = rotation;

                ConstrainRotation();

                Debug.DrawLine(transform.position, transform.position + directionToTarget, Color.blue);
                Debug.DrawLine(transform.position, transform.position + up, Color.yellow);
                yield return new WaitForFixedUpdate();
            }
            yield return null;
        }

        protected void ConstrainRotation()
        {
            /*Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.x = Clamp360(eulerAngles.z, minPitch, maxPitch);
            eulerAngles.y = Clamp360(eulerAngles.y, minYaw, maxYaw);
            eulerAngles.z = Clamp360(eulerAngles.z, minRoll, maxRoll);
            transform.rotation = Quaternion.Euler(eulerAngles);*/
        }

        float Clamp360(float value, float min, float max)
        {
            if (value < min || value > max)
            {
                if (360 - (value % 360) - min < (value % 360) - max)
                    return min;
                else
                    return max;
            }
            else return value;
        }

        private void FireWeapon()
        {
            Fire?.Invoke();
        }

        public override void GetBeliefs(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs)
        {
            BeliefFactory beliefFactory = new(agent, agentBeliefs);

            beliefFactory.AddBelief(side.ToString() + "ArmNotTooClose", () => !m_opponentCloseSensor.IsTargetInRange);
            beliefFactory.AddBelief(side.ToString() + "ArmAttacking", () => m_isAttacking);
            beliefFactory.AddBelief(side.ToString() + "ArmReady", () => m_isReady);
            beliefFactory.AddBelief(side.ToString() + "ArmWorking", () => !isBroken);
            beliefFactory.AddSensorBelief(side.ToString() + "ArmInAttackRange", m_attackRangeSensor);
            beliefFactory.AddBelief(side.ToString() + "ArmFacingOpponent", () => m_facingOpponent);
        }

        public override void GetActions(GoapAgent agent, SerializableHashSet<AgentAction> actions, Dictionary<string, AgentBelief> agentBeliefs)
        {
            actions.Add(
                new AgentAction.Builder(side.ToString() + "ArmAttack")
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
        }

        public override void GetGoals(GoapAgent agent, SerializableHashSet<AgentGoal> goals, Dictionary<string, AgentBelief> agentBeliefs)
        {
            base.GetGoals(agent, goals, agentBeliefs);
        }
    }
}