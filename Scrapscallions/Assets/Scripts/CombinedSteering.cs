using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI
{
    [CreateAssetMenu(fileName = "New Combined Steering", menuName = "Steering/Combined Steering")]
    public class CombinedSteering : SteeringBehavior
    {
        public SteeringBehavior linearSteeringBehavior;
        public SteeringBehavior angularSteeringBehavior;

        public CombinedSteering(SteeringBehavior linearSteeringBehavior, SteeringBehavior angularSteeringBehavior)
        {
            this.linearSteeringBehavior = linearSteeringBehavior;
            this.angularSteeringBehavior = angularSteeringBehavior;
        }

        public override SteeringOutput GetSteering(RobotState robotState)
        {
            SteeringOutput result = new();

            if (linearSteeringBehavior != null)
                result.linear = linearSteeringBehavior.GetSteering(robotState).linear;
            else if (angularSteeringBehavior != null)
                result.linear = angularSteeringBehavior.GetSteering(robotState).linear;
            else
                result.linear = Vector3.zero;

            if (angularSteeringBehavior != null)
                result.angular = angularSteeringBehavior.GetSteering(robotState).angular;
            else if (linearSteeringBehavior != null) result.angular = linearSteeringBehavior.GetSteering(robotState).angular;
            else result.angular = 0;

            return result;
        }

        public override SteeringBehavior Clone()
        {
            CombinedSteering clone = (CombinedSteering)base.Clone();

            if (linearSteeringBehavior != null)
                clone.linearSteeringBehavior = linearSteeringBehavior.Clone();
            else if (angularSteeringBehavior != null)
                clone.angularSteeringBehavior = angularSteeringBehavior.Clone();

            return clone;
        }
    }
}