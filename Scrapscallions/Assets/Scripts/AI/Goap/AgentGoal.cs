using Scraps.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI.GOAP
{
    public class AgentGoal : GoapScriptableObject
    {
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public float Priority { get; private set; }
        [field: SerializeField] public SerializableHashSet<AgentBelief> DesiredEffects { get; private set; }

        internal void Initialize(string name, float priority = 1f, HashSet<AgentBelief> desiredEffects = null)
        {
            this.name = name;
            Name = name;
            Priority = priority;
            if (desiredEffects != null)
                DesiredEffects = new(desiredEffects);
            else
                DesiredEffects = new();
        }

        public class Builder
        {
            readonly AgentGoal goal;
            public Builder(string name)
            {
                goal = CreateInstance<AgentGoal>();
                goal.Initialize(name);
            }

            public Builder WithPriority(float priority)
            {
                goal.Priority = priority;
                return this;
            }

            public Builder WithDesiredEffect(AgentBelief effect)
            {
                goal.DesiredEffects.Add(effect);
                return this;
            }

            public AgentGoal Build() => goal;
        }
    }
}