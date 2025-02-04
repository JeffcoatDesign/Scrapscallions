using Scraps.AI.GOAP;
using Scraps.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    public interface IPartController
    {
        void SetParent(Transform parent);
        void Hit(int damage);
        void Repair(int amount);
        void Break();
        void GetBeliefs(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs);
        SerializableHashSet<AgentAction> GetActions(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs);
        SerializableHashSet<AgentGoal> GetGoals(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs);
    }
}