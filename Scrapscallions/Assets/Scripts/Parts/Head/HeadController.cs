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
        private Dictionary<string, GameObject> m_steeringBehaviors;
        private void Awake()
        {
            m_steeringBehaviors = new Dictionary<string, GameObject>();

            //m_idleBehavior = m_idleBehavior.Clone();
            //TO DO THE OTHERS AND PUTTING INTO DICT
        }

        override public void GetActions(GoapAgent agent, SerializableHashSet<AgentAction> actions, Dictionary<string, AgentBelief> agentBeliefs)
        {
            //NOOP
            actions.Add(new AgentAction.Builder("Idle Wander").AddEffect(agentBeliefs["Nothing"]).WithStrategy(ScriptableObject.CreateInstance<WanderStrategy>().Initialize(m_robot.State, 5f)).Build());
        }

        override public void GetBeliefs(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs)
        {
            //NOOP
        }

        override public void GetGoals(GoapAgent agent, SerializableHashSet<AgentGoal> goals, Dictionary<string, AgentBelief> agentBeliefs)
        {
            //NOOP
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

        public override void Break()
        {
            tagTransform.parent = transform.parent;

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