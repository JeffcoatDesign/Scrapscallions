using Assets.Scripts.Parts;
using Scraps.AI.GOAP;
using Scraps.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    [RequireComponent(typeof(DetachOnBreak))]
    public abstract class PartController : MonoBehaviour, IPartController
    {
        public static event Action AnyPartHit;
        protected Robot m_robot { get; private set; }
        internal bool isBroken;
        internal Action<int> PartHit;
        internal Action PartBroken;
        public Action Broke;

        virtual public void Break()
        {
            isBroken = true;
            Broke.Invoke();
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
            AnyPartHit?.Invoke();
        }

        public virtual void Initialize(Robot robot)
        {
            m_robot = robot;
            m_robot.agent.Died += OnDied;
        }

        virtual public void Repair(int amount)
        {
            throw new System.NotImplementedException();
        }

        protected void OnDied()
        {
            Broke?.Invoke();
        }
    }
}