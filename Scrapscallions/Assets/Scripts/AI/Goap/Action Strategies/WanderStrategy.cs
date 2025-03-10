using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Scraps.AI.GOAP
{
    [CreateAssetMenu(fileName = "New Wander Strategy", menuName = "GOAP/Action Strategies/Wander Strategy")]
    public class WanderStrategy : ScriptableObject, IActionStrategy
    {
        RobotState m_state;
        [SerializeField] float m_wanderRadius;
        [SerializeField] float m_minDistance = 2f;
        Vector3 m_targetPos;
        private CountdownTimer m_maxTimer;
        public bool CanPerform => !IsComplete;
        public bool IsComplete => m_state.RemainingDistance <= m_minDistance || m_maxTimer.IsFinished;// && !agent.pathPending;

        public WanderStrategy Initialize(RobotState state, float wanderRadius, float minDistance = 2f, float maxTime = 10f)
        {
            m_maxTimer = new(maxTime);
            m_state = state;
            m_wanderRadius = wanderRadius;
            m_minDistance = minDistance;
            return this;
        }

        public void Begin()
        {
            Vector3 randomDirection = (Random.insideUnitSphere * m_wanderRadius).With(y: 0);
            m_targetPos = m_state.character.transform.position + randomDirection;
            m_state.SetDestination(() => m_targetPos);
            m_maxTimer.Start();
            //for (int i = 0; i < 5; i++)
            //{
            //    Vector3 randomDirection = (Random.insideUnitSphere * m_wanderRadius).With(y: 0);

            //    //TODO Needs to be corrected for when legs are implemented in ai
            //    if (Physics.Raycast(m_state.character.transform.position.With(y: 0.5f) + randomDirection, Vector3.down, out RaycastHit hit, m_wanderRadius,1))
            //    {
            //        m_state.SetDestination(() => hit.point);
            //        return;
            //    }
            //}
        }

        public void Tick(float deltaTime)
        {
            if (m_maxTimer != null)
                m_maxTimer.Tick(deltaTime);
        }
    }
}