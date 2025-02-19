using UnityEngine;

namespace Scraps.AI.GOAP
{
    [CreateAssetMenu(fileName = "New Wander Strategy", menuName = "GOAP/Action Strategies/Wander Strategy")]
    public class WanderStrategy : ScriptableObject, IActionStrategy
    {
        RobotState m_state;
        [SerializeField] float wanderRadius;
        public bool CanPerform => !IsComplete;
        public bool IsComplete => m_state.RemainingDistance <= 2f;// && !agent.pathPending;

        public WanderStrategy Initialize(RobotState state, float wanderRadius)
        {
            m_state = state;
            this.wanderRadius = wanderRadius;
            return this;
        }

        public void Begin()
        {
            for (int i = 0; i < 5; i++)
            {
                Vector3 randomDirection = (UnityEngine.Random.insideUnitSphere * wanderRadius).With(y: 0);

                //TODO Needs to be corrected for when legs are implmented in ai
                if (Physics.Raycast(m_state.character.transform.position + randomDirection, Vector3.down, out RaycastHit hit, wanderRadius,1))
                {
                    m_state.SetDestination(() => hit.point);
                    return;
                }
            }
        }
    }
}