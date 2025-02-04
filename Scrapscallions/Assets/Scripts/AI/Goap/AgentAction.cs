using Scraps.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scraps.AI.GOAP
{
    /// <summary>
    /// Uses the strategy pattern
    /// </summary>
    [CreateAssetMenu(fileName = "Test", menuName = "Scraps/GOAP/Action")]
    public class AgentAction : GoapScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public float Cost { get; private set; }

        //TODO MAKE SERIALIZABLE AND ADD SERIALIZABLE DICTIONARIES
        [field: SerializeField] public SerializableHashSet<AgentBelief> Preconditions { get; set; }
        [field: SerializeField] public SerializableHashSet<AgentBelief> Effects { get; set; }

        [field: SerializeField] IActionStrategy m_strategy;
        public bool IsComplete => m_strategy.IsComplete;

        AgentAction(string name)
        {
            Name = name;
        }

        public override bool Equals(object other)
        {
            if (other is AgentAction otherAction)
            {
                return GetInstanceID() == otherAction.GetInstanceID();
            }
            return false;
        }

        public override int GetHashCode()
        {
            return GetInstanceID();
        }

        public void Initialize(string name, float cost = 0, List<AgentBelief> preconditions = null, List<AgentBelief> effects = null, IActionStrategy strategy = null)
        {
            Name = name;
            Cost = cost;
            m_strategy = strategy;

            if (preconditions != null)
                Preconditions = new(preconditions.ToHashSet());
            else
                Preconditions = new();

            if (effects != null)
                Effects = new(effects.ToHashSet());
            else
                Effects = new();
        }

        public void Start() => m_strategy.Begin();
        public void Stop() => m_strategy.Stop();

        public void Tick(float deltaTime)
        {
            //Check if the action can be performed and update the strategy
            if (m_strategy.CanPerform)
            {
                m_strategy.Tick(deltaTime);
            }

            if (!m_strategy.IsComplete) return;

            foreach (var effect in Effects)
            {
                effect.Evaluate();
            }
        }

        public class Builder
        {
            readonly AgentAction action;

            public Builder(string name)
            {
                action = CreateInstance<AgentAction>();
                Debug.Log(action.GetInstanceID());
                action.name = name;
                action.Initialize(name);
            }

            public Builder WithCost(float cost)
            {
                action.Cost = cost;
                return this;
            }

            public Builder WithStrategy(IActionStrategy strategy)
            {
                action.m_strategy = strategy;
                return this;
            }

            public Builder WithPrecondition(AgentBelief precondition)
            {
                action.Preconditions.Add(precondition);
                return this;
            }

            public Builder AddEffect(AgentBelief effect)
            {
                action.Effects.Add(effect);
                return this;
            }

            public AgentAction Build() => action;
        }
    }
}