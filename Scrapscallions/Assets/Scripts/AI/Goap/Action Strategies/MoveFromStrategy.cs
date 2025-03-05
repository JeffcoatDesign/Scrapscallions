using System;
using UnityEngine;

namespace Scraps.AI.GOAP
{
    [CreateAssetMenu(fileName = "New Move From Strategy", menuName = "GOAP/Action Strategies/Move From Strategy")]
    public class MoveFromStrategy : ScriptableObject, IActionStrategy
    {
        [SerializeField] private RobotState m_state;
        [SerializeField] Func<Vector3> destination;
        [SerializeField] float minDistance;
        Func<Vector3> moveFromPoint;
        public bool CanPerform => !IsComplete;
        public bool IsComplete
        {
            get
            {
                return Vector3.Distance(moveFromPoint(), m_state.Position) >= minDistance;
            }
        }

        public MoveFromStrategy Initialize(RobotState state, Func<Vector3> moveFromPoint, float minDistance = 2f)
        {
            m_state = state;
            this.moveFromPoint = moveFromPoint;
            destination = () => GetPoint(state);
            this.minDistance = minDistance;
            return this;
        }

        private Vector3 GetPoint(RobotState state) 
        {
            Vector3 direction = state.Position - moveFromPoint();
            return moveFromPoint() + direction * minDistance;
        }

        public void Begin()
        {
            m_state.SetDestination(destination);
        }

        public void Stop() => m_state.ResetPath();
    }
}