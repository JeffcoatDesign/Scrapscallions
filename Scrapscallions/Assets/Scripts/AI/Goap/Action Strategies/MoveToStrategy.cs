using System;
using UnityEngine;

namespace Scraps.AI.GOAP
{
    [CreateAssetMenu(fileName = "New Move To Strategy", menuName = "GOAP/Action Strategies/Move To Strategy")]
    public class MoveToStrategy : ScriptableObject, IActionStrategy
    {
        [SerializeField] private RobotState m_state;
        [SerializeField] Func<Vector3> destination;
        [SerializeField] float minDistance;
        public bool CanPerform => !IsComplete && m_state.CanMove;
        public bool IsComplete
        {
            get
            {
                return m_state.RemainingDistance <= minDistance;
            }
        }

        public MoveToStrategy Initialize(RobotState state, Func<Vector3> destination, float minDistance = 2f)
        {
            m_state = state;
            this.destination = destination;
            this.minDistance = minDistance;
            return this;
        }

        public void Begin()
        {
            m_state.SetDestination(destination);
        }

        public void Stop()
        {
            //Debug.Log($"Stopping + {IsComplete} Remaining Distance = {m_state.RemainingDistance} + Min Distance = {minDistance}");
            m_state.ResetPath();
        }
    }
}