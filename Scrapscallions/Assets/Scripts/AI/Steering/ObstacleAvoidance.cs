using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI
{
    [CreateAssetMenu(fileName = "New Obstacle Avoidance", menuName = "Steering/Obstacle Avoidance")]
    public class ObstacleAvoidance : Seek
    {
        public float rayLength = 1f;
        public float avoidanceRadius = 3f;
        public LayerMask layermask;

        private Vector3 targetPos;

        public override SteeringOutput GetSteering(RobotState robotState)
        {
            this.robotState = robotState;
            Ray ray = new()
            {
                direction = robotState.character.linearVelocity,
                origin = robotState.character.transform.position
            };
            Debug.DrawRay(robotState.character.transform.position, robotState.character.transform.position + robotState.character.linearVelocity);
            if (Physics.Raycast(ray, out RaycastHit hit, rayLength, layermask))
            {
                targetPos = hit.point + hit.normal * avoidanceRadius;
                Debug.Log("Hit");
            }
            else
            {
                targetPos = GetTargetPosition(robotState.target());
            }

            return base.GetSteering(robotState);
        }
        protected override Vector2 GetTargetPosition(GameObject target)
        {
            return targetPos;
        }
    }
}