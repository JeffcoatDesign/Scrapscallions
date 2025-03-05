using Scraps.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI.GOAP
{
    public class BeliefFactory
    {
        readonly GoapAgent agent;
        readonly Dictionary<string, AgentBelief> beliefs;

        public BeliefFactory(GoapAgent agent, Dictionary<string, AgentBelief> beliefs)
        {
            this.agent = agent;
            this.beliefs = beliefs;
        }

        public void AddBelief(string key, Func<bool> condition)
        {
            beliefs.Add(key, new AgentBelief.Builder(key)
                .WithCondition(condition)
                .Build());
        }

        public void AddSensorBelief(string key, Sensor sensor)
        {
            beliefs.Add(key, new AgentBelief.Builder(key)
                .WithCondition(() => sensor.IsTargetInRange)
                .WithLocation(() => sensor.TargetPosition)
                .Build());
        }

        public void AddLocationBelief(string key, float distance, Func<Vector3> locationFunc)
        {
            beliefs.Add(key, new AgentBelief.Builder(key)
                .WithCondition(() => InRangeOf(locationFunc, distance))
                .WithLocation(locationFunc)
                .Build()
            );
        }

        public void AddLocationBelief(string key, float distance, Transform locationCondition)
        {
            AddLocationBelief(key, distance, locationCondition.position);
        }

        public void AddLocationBelief(string key, float distance, Vector3 locationCondition)
        {
            beliefs.Add(key, new AgentBelief.Builder(key)
                .WithCondition(() => InRangeOf(locationCondition, distance))
                .WithLocation(() => locationCondition)
                .Build());
        }

        bool InRangeOf(Vector3 pos, float range) => Vector3.Distance(agent.transform.position, pos) < range;
        bool InRangeOf(Func<Vector3> pos, float range) => Vector3.Distance(agent.transform.position, pos()) < range;
    }

    [CreateAssetMenu(fileName = "Agent Belief", menuName = "GOAP/Agent Belief")]
    public class AgentBelief : GoapScriptableObject
    {
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public string ConditionName { get; set; }
        [field: SerializeField] public bool IsLocation { get; set; }
        [field: SerializeField] public string LocationName { get; set; }

        Func<bool> condition = () => false;
        Func<Vector3> observedLocation = () => Vector3.zero;

        public Vector3 Location => observedLocation();

        AgentBelief(string name)
        {
            Name = name;
        }

        public bool Evaluate() => condition();

        public class Builder
        {
            readonly AgentBelief belief;

            public Builder(string name)
            {
                belief = CreateInstance<AgentBelief>();
                belief.Name = name;
                belief.name = name;
            }

            public Builder WithCondition(Func<bool> condition)
            {
                belief.condition = condition;
                return this;
            }

            public Builder WithLocation(Func<Vector3> location)
            {
                belief.observedLocation = location;
                return this;
            }

            public AgentBelief Build()
            {
                return belief;
            }
        }
    }
}