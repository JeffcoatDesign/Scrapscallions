using Scraps.AI;
using Scraps.AI.GOAP;
using Scraps.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Scraps.Parts
{
    public class HeadController : PartController
    {
        public RobotPartHead head;
        public Transform tagTransform;
        [SerializeField] private SteeringBehavior m_idleBehavior, m_fleeBehavior, m_pursueBehavior;
        [SerializeField] private float m_lookSpeed = 2.5f;
        [SerializeField] private PowerUpController m_powerUpController;
        public Dictionary<string, SteeringBehavior> SteeringBehaviors { get; private set; }
        public int hp = 1;
        public override void Initialize(Robot robot)
        {
            base.Initialize(robot);
            if (m_powerUpController != null) m_powerUpController.Initialize(this);
        }

        private void Awake()
        {
            SteeringBehaviors = new();

            //m_idleBehavior = m_idleBehavior.Clone();
            //m_fleeBehavior = m_fleeBehavior.Clone();
            //m_pursueBehavior = m_pursueBehavior.Clone();

            //SteeringBehaviors.Add("idle", m_idleBehavior);
            //SteeringBehaviors.Add("flee", m_fleeBehavior);
            //SteeringBehaviors.Add("pursue", m_pursueBehavior);
        }

        override public void GetActions(GoapAgent agent, SerializableHashSet<AgentAction> actions, Dictionary<string, AgentBelief> agentBeliefs)
        {
            //NOOP
            actions.Add(new AgentAction.Builder("Idle Wander").AddEffect(agentBeliefs["Nothing"]).WithStrategy(ScriptableObject.CreateInstance<WanderStrategy>().Initialize(m_robot.State, 5f)).Build());

            if (m_powerUpController != null && !m_robot.State.isPlayer)
            {
                actions.Add(new AgentAction.Builder(m_powerUpController.ActionName)
                    .AddEffect(agentBeliefs["Using" + m_powerUpController.ActionName])
                    .AddEffect(agentBeliefs["AttackingOpponent"])
                    .WithPrecondition(agentBeliefs[m_powerUpController.ActionName + "Ready"])
                    .WithStrategy(ScriptableObject.CreateInstance<PowerUpStrategy>().Initialize(m_powerUpController))
                    .Build());
            }
        }

        override public void GetBeliefs(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs)
        {
            BeliefFactory factory = new BeliefFactory(agent,agentBeliefs);

            if (m_powerUpController != null) {
                factory.AddBelief("Using" + m_powerUpController.ActionName, () => m_powerUpController.IsTakingAction);
                factory.AddBelief(m_powerUpController.ActionName + "Ready", () => m_powerUpController.IsReady);
            }
        }

        override public void GetGoals(GoapAgent agent, SerializableHashSet<AgentGoal> goals, Dictionary<string, AgentBelief> agentBeliefs)
        {
            if (m_powerUpController != null)
            {
                goals.Add(new AgentGoal.Builder("Use" + m_powerUpController.ActionName).WithDesiredEffect(agentBeliefs["Using"+ m_powerUpController.ActionName]).Build());
            }
        }

        override public void Hit(int damage)
        {
            base.Hit(damage);

            int currentHP = head.CurrentHP - damage;
            if (currentHP <= 0)
            {
                currentHP = 0;
                Break();
            }
            head.CurrentHP = currentHP;

            PartHit?.Invoke(damage);
        }

        private void Update()
        {
            if (!isInitialized) return;
            if (!isBroken && m_robot != null && m_robot.State.target() != null) {
                Vector3 dir = m_robot.State.target().transform.position.With(y: transform.position.y) - transform.position;
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, m_lookSpeed * Time.deltaTime);
            }
            hp = head.CurrentHP;
        }

        public override void Break()
        {
            tagTransform.parent = transform.parent;

            isBroken = true;
            head.CurrentHP = 0;
            base.Break();
        }

        override public void Repair(int amount)
        {
            throw new System.NotImplementedException();
        }

        internal SteeringBehavior GetSteeringBehavior(string key)
        {
            throw new NotImplementedException();
        }
    }
}