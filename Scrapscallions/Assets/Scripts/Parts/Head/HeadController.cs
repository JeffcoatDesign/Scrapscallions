using Scraps.AI.GOAP;
using Scraps.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    public class HeadController : PartController
    {
        public RobotPartHead head;
        [SerializeField] private GameObject m_headVisual;
        override public void Break()
        {
            isBroken = true;
            m_headVisual.SetActive(false);
        }

        override public void GetActions(GoapAgent agent, SerializableHashSet<AgentAction> actions, Dictionary<string, AgentBelief> agentBeliefs)
        {
            //NOOP
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
            int currentHP = head.CurrentHP - damage;
            if (currentHP <= 0)
            {
                currentHP = 0;
                Break();
            }
            head.CurrentHP = currentHP;

            PartHit?.Invoke(damage);
        }

        override public void Repair(int amount)
        {
            throw new System.NotImplementedException();
        }
    }
}