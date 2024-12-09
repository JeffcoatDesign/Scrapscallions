using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI
{
    [CreateAssetMenu(fileName = "New Override Obstacle Avoidance", menuName = "Steering/Override Obstacle Avoidance")]
    public class OverrideAvoidObstacles : ObstacleAvoidance
    {
        public SteeringBehavior baseBehavior;
        protected override Vector2 GetTargetPosition(GameObject target)
        {
            SteeringOutput steeringOutput = baseBehavior.GetSteering(robotState);
            return robotState.Position + steeringOutput.linear;
        }
        public override SteeringBehavior Clone()
        {
            OverrideAvoidObstacles clone = (OverrideAvoidObstacles)base.Clone();
            clone.baseBehavior = baseBehavior.Clone();
            return clone;
        }
    }
}