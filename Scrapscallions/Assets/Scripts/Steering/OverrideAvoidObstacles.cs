using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI
{
    [CreateAssetMenu(fileName = "New Override Obstacle Avoidance", menuName = "Steering/Override Obstacle Avoidance")]
    public class OverrideAvoidObstacles : Seek
    {
        public SteeringBehavior baseBehavior;
        
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
                Debug.Log("Hit");
                targetPos = new Vector2(hit.point.x, hit.point.z) + new Vector2(hit.normal.x, hit.normal.z) * avoidanceRadius;
                return base.GetSteering(robotState);
            }
            Debug.Log("Hi");
            return baseBehavior.GetSteering(robotState);
        }
        protected override Vector2 GetTargetPosition(GameObject target)
        {
            return targetPos;
        }
        public override SteeringBehavior Clone()
        {
            OverrideAvoidObstacles clone = (OverrideAvoidObstacles)base.Clone();
            clone.baseBehavior = baseBehavior.Clone();
            return clone;
        }
    }
}