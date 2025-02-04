using Scraps.AI.GOAP;
using Scraps.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    public class LegsController : MonoBehaviour, IPartController
    {
        [SerializeField] private Transform m_bodyAttachPoint;
        public Transform BodyAttachPoint { get => m_bodyAttachPoint; }
        virtual public void Break()
        {
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