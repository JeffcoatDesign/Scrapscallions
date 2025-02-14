using Scraps.AI.GOAP;
using Scraps.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    public abstract class PartController : MonoBehaviour, IPartController
    {
        protected Robot m_robot;
        internal bool isBroken;
        internal Action<int> PartHit;
        internal Action PartBroken;
        virtual public void Break()
        {
            throw new System.NotImplementedException();
        }

        virtual public void GetActions(GoapAgent agent, SerializableHashSet<AgentAction> actions, Dictionary<string, AgentBelief> agentBeliefs)
        {
            throw new System.NotImplementedException();
        }

        public virtual void GetBeliefs(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs)
        {
            throw new System.NotImplementedException();
        }

        public virtual void GetGoals(GoapAgent agent, SerializableHashSet<AgentGoal> goals, Dictionary<string, AgentBelief> agentBeliefs)
        {
            Debug.LogError("GetGoals() should have been overriden.");
        }

        public virtual Robot GetRobot() => m_robot;

        public virtual void Hit(int damage)
        {
            PartHit?.Invoke(damage);
            throw new System.NotImplementedException();
        }

        public virtual void Initialize(Robot robot) => m_robot = robot;

        virtual public void Repair(int amount)
        {
            throw new System.NotImplementedException();
        }
    }
}