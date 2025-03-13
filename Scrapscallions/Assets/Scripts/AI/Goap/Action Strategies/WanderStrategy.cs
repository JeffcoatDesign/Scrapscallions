<<<<<<< Updated upstream
﻿using UnityEngine;
=======
﻿//using Unity.Burst.CompilerServices;
using UnityEngine;
>>>>>>> Stashed changes

namespace Scraps.AI.GOAP
{
    [CreateAssetMenu(fileName = "New Wander Strategy", menuName = "GOAP/Action Strategies/Wander Strategy")]
    public class WanderStrategy : ScriptableObject, IActionStrategy
    {
        Robot agent;
        [SerializeField] float wanderRadius;
        public bool CanPerform => !IsComplete;
        public bool IsComplete => agent.RemainingDistance <= 2f;// && !agent.pathPending;

        public WanderStrategy Initialize(Robot agent, float wanderRadius)
        {
            this.agent = agent;
            this.wanderRadius = wanderRadius;
            return this;
        }

        public void Begin()
        {
            for (int i = 0; i < 5; i++)
            {
                Vector3 randomDirection = (UnityEngine.Random.insideUnitSphere * wanderRadius).With(y: 0);

                //TODO Needs to be corrected for when legs are implmented in ai
                if (Physics.Raycast(agent.State.character.transform.position + randomDirection, Vector3.down, out RaycastHit hit, wanderRadius,1))
                {
                    agent.SetDestination(hit.point);
                    return;
                }
            }
        }
    }
}