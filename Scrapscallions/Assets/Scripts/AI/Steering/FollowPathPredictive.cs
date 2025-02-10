using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Scraps.AI
{
    [CreateAssetMenu(fileName = "New Follow Path Predictive", menuName = "Steering/Follow Path Predictive")]
    public class FollowPathPredictive : Seek
    {
        public float pathOffset = 1f;
        public float predictTime = 0.1f;

        private Path path;
        private float currentParam;
        private Vector3 targetPos;

        protected override Vector2 GetTargetPosition(GameObject target)
        {
            return new(targetPos.x, targetPos.z);
        }

        public override SteeringOutput GetSteering(RobotState robotState)
        {
            path = robotState.Path;
            if (path != null)
            {
                Vector3 futurePos = robotState.character.transform.position + robotState.character.linearVelocity * predictTime;

                currentParam = path.GetParam(futurePos, robotState.character.transform.position);

                float targetParam = currentParam + pathOffset;

                targetPos = path.GetPosition(targetParam);
            }
            return base.GetSteering(robotState);
        }
    }
}