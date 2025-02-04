using Scraps.AI.GOAP;
using Scraps.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    public class BodyController : MonoBehaviour, IPartController
    {
        [SerializeField] private Transform m_headAttachPoint;
        [SerializeField] private Transform m_leftArmAttachPoint;
        [SerializeField] private Transform m_rightArmAttachPoint;
        public Transform HeadAttachPoint { get => m_headAttachPoint; }
        public Transform LeftArmAttachPoint { get => m_leftArmAttachPoint; }
        public Transform RightArmAttachPoint { get => m_rightArmAttachPoint; }
        virtual public void Break()
        {
            //TODO Break the part
            throw new System.NotImplementedException();
        }

        public SerializableHashSet<AgentAction> GetActions(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs)
        {
            throw new System.NotImplementedException();
        }

        public void GetBeliefs(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs)
        {
            throw new System.NotImplementedException();
        }

        public SerializableHashSet<AgentGoal> GetGoals(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs)
        {
            throw new System.NotImplementedException();
        }

        virtual public void Hit(int damage)
        {
            throw new System.NotImplementedException();
        }

        virtual public void Repair(int amount)
        {
            throw new System.NotImplementedException();
        }

        virtual public void SetParent(Transform parent)
        {
            throw new System.NotImplementedException();
        }
    }
}