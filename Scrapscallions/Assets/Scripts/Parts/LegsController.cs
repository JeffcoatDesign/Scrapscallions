using Scraps.AI.GOAP;
using Scraps.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    public class LegsController : PartController
    {
        [SerializeField] private Transform m_bodyAttachPoint;
        public Transform BodyAttachPoint { get => m_bodyAttachPoint; }
        override public void Break()
        {
            isBroken = true;
        }

        override public void GetActions(GoapAgent agent, SerializableHashSet<AgentAction> actions, Dictionary<string, AgentBelief> agentBeliefs)
        {
            throw new System.NotImplementedException();
        }

        override public void GetBeliefs(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs)
        {
            //NOOP
        }

        override public void GetGoals(GoapAgent agent, SerializableHashSet<AgentGoal> goals, Dictionary<string, AgentBelief> agentBeliefs)
        {
            throw new System.NotImplementedException();
        }

        override public Robot GetRobot()
        {
            return m_robot;
        }

        override public void Hit(int damage)
        {
            PartHit?.Invoke(damage);
        }

        override public void Repair(int amount)
        {
            throw new System.NotImplementedException();
        }
    }
}