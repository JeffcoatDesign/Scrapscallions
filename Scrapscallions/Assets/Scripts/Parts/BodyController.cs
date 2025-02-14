using Scraps.AI.GOAP;
using Scraps.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

namespace Scraps.Parts
{
    public class BodyController : PartController
    {
        public RobotPartBody body;
        [SerializeField] private GameObject m_bodyVisual;
        [SerializeField] private Transform m_headAttachPoint;
        [SerializeField] private Transform m_leftArmAttachPoint;
        [SerializeField] private Transform m_rightArmAttachPoint;
        public Transform HeadAttachPoint { get => m_headAttachPoint; }
        public Transform LeftArmAttachPoint { get => m_leftArmAttachPoint; }
        public Transform RightArmAttachPoint { get => m_rightArmAttachPoint; }
        override public void Break()
        {
            isBroken = true;
            m_bodyVisual.SetActive(false);
            body.IsBroken = true;
            body.Break?.Invoke();
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
            int currentHP = body.CurrentHP - damage;

            if (currentHP <= 0)
            {
                currentHP = 0;
                Break();
            }

            body.CurrentHP = currentHP;
            PartHit?.Invoke(damage);
        }

        override public void Repair(int amount)
        {
            throw new System.NotImplementedException();
        }
    }
}