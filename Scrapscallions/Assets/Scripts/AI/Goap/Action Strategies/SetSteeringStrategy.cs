using System;
using UnityEngine;

namespace Scraps.AI.GOAP
{
    [CreateAssetMenu(fileName = "New Move To Strategy", menuName = "GOAP/Action Strategies/Move To Strategy")]
    public class SetSteeringStrategy : ScriptableObject, IActionStrategy
    {
        [SerializeField] private RobotState m_state;
        [SerializeField] private CustomKinematic m_character;
        [SerializeField] private SteeringBehavior m_steeringBehavior;
        [SerializeField] Func<Vector3> destination;
        [SerializeField] float minDistance;
        public bool CanPerform => !IsComplete;
        public bool IsComplete { get; private set; } = false;

        public SetSteeringStrategy Initialize(RobotState state, SteeringBehavior steeringBehavior)
        {
            m_state = state;
            m_character = state.character as CustomKinematic;
            m_steeringBehavior = steeringBehavior;
            return this;
        }

        public void Begin()
        {
            IsComplete = true;
            m_state.character.SetSteeringBehavior(m_steeringBehavior);
        }

        public void Stop() => m_state.character.SetSteeringBehavior(m_state.GetSteeringBehavior("Idle"));
    }
}