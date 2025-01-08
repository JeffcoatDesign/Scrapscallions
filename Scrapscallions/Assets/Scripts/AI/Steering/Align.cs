using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI
{
    public class Align : SteeringBehavior
    {
        public float maxAngularAcceleration = 100f;
        public float maxAngularVelocity = 45f;
        public float timeToTarget = 0.1f;
        public float slowRadius = 10f;

        internal RobotState robotState;
        // the radius for beginning to slow down

        // the time over which to achieve target speed

        // returns the angle in degrees that we want to align with
        // Align will rotate to match the target's oriention
        // sub-classes can overwrite this function to set a different target angle e.g. to face a target
        public virtual float GetTargetAngle(GameObject target)
        {
            return target.transform.eulerAngles.y;
        }

        public override SteeringOutput GetSteering(RobotState robotState)
        {
            this.robotState = robotState;
            SteeringOutput result = new SteeringOutput();

            // get the naive direction to the target
            //float rotation = Mathf.DeltaAngle(character.transform.eulerAngles.y, target.transform.eulerAngles.y);
            float rotation = Mathf.DeltaAngle(robotState.character.transform.eulerAngles.y, GetTargetAngle(robotState.target));
            float rotationSize = Mathf.Abs(rotation);

            // check if we are there, return no steering
            //if (rotationSize < targetRadius)
            //{
            //    return null;
            //}

            // if we are outside the slow radius, then use maximum rotation
            float targetRotation = 0.0f;
            if (rotationSize > slowRadius)
            {
                targetRotation = maxAngularVelocity;
            }
            else // otherwise use a scaled rotation
            {
                targetRotation = maxAngularVelocity * rotationSize / slowRadius;
            }

            // the final targetRotation combines speed (already in the variable) and direction
            targetRotation *= rotation / rotationSize;

            // acceleration tries to get to the target rotation
            // something is breaking my angularVelocty... check if NaN and use 0 if so
            float currentAngularVelocity = float.IsNaN(robotState.character.angularVelocity) ? 0f : robotState.character.angularVelocity;
            result.angular = targetRotation - currentAngularVelocity;
            result.angular /= timeToTarget;

            // check if the acceleration is too great
            float angularAcceleration = Mathf.Abs(result.angular);
            if (angularAcceleration > maxAngularAcceleration)
            {
                result.angular /= angularAcceleration;
                result.angular *= maxAngularAcceleration;
            }

            result.linear = Vector3.zero;
            return result;
        }
    }
}
