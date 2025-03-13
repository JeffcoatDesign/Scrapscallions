using Scraps.AI.GOAP;
using Scraps.Gameplay;
using Scraps.UI;
using Scraps.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    public class BodyController : PartController
    {
        public RobotPartBody body;
        [SerializeField] private Transform m_headAttachPoint;
        [SerializeField] private Transform m_leftArmAttachPoint;
        [SerializeField] private Transform m_rightArmAttachPoint;
        public Transform HeadAttachPoint { get => m_headAttachPoint; }
        public Transform LeftArmAttachPoint { get => m_leftArmAttachPoint; }
        public Transform RightArmAttachPoint { get => m_rightArmAttachPoint; }
        override public void Break()
        {
            isBroken = true;
            body.IsBroken = true;
            body.Break?.Invoke();
            Broke?.Invoke();
        }

        override public void GetActions(GoapAgent agent, SerializableHashSet<AgentAction> actions, Dictionary<string, AgentBelief> agentBeliefs)
        {
            throw new System.NotImplementedException();
        }

        override public void GetBeliefs(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs)
        {
            throw new System.NotImplementedException();
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

            if (body.IsBroken) return;

            int currentHP = body.CurrentHP - damage;
            if (currentHP <= 0)
            {
                currentHP = 0;
                Break();
            }

            body.CurrentHP = currentHP;
        }

        override public void Repair(int amount)
        {
            throw new System.NotImplementedException();
        }
    }
}