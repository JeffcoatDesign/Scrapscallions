using Scraps.AI.GOAP;
using Scraps.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    public interface IPartController
    {
        void Initialize(Robot robot);
        void Hit(int damage);
        void Repair(int amount);
        void Break();
        void GetBeliefs(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs);
        void GetActions(GoapAgent agent, SerializableHashSet<AgentAction> actions, Dictionary<string, AgentBelief> agentBeliefs);
        void GetGoals(GoapAgent agent, SerializableHashSet<AgentGoal> goals, Dictionary<string, AgentBelief> agentBeliefs);
        Robot GetRobot();
    }
}