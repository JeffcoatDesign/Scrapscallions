using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI
{
    [CreateAssetMenu(fileName = "New Arrive", menuName = "Steering/Arrive")]
    public class Arrive : SteeringBehavior
    {
        public float timeToDestination = 0.1f;

        // the radius for arriving at the target
        public float targetRadius = 1.5f;

        // the radius for beginning to slow down
        public float slowRadius = 3f;

        public override SteeringOutput GetSteering(RobotState robotState)
        {
            SteeringOutput result = new SteeringOutput();

            // get the direction to the destination
            Vector3 direction = Vector3.zero;
            if (robotState.hasPath)
                direction = robotState.destination() - robotState.character.transform.position;

            float distance = direction.magnitude;

            // if we are outside the slow radius, then move at max speed
            float targetSpeed;
            if (distance > slowRadius)
            {
                targetSpeed = robotState.MaxSpeed;
            }
            else // otherwise calculate a scaled speed
            {
                //targetSpeed = -(maxSpeed * distance / slowRadius); // should slowRadius here instead be targetRadius?
                targetSpeed = robotState.MaxSpeed * (distance - targetRadius) / targetRadius;
            }

            // the target velocity combines speed and direction
            Vector3 targetVelocity = direction;
            targetVelocity.Normalize();
            targetVelocity *= targetSpeed;

            // acceleration tries to get to the target velocity
            Vector3 resultingVelocity = targetVelocity - robotState.character.linearVelocity;
            result.linear = new(resultingVelocity.x, resultingVelocity.z);
            result.linear /= timeToDestination;

            // check if the acceleration is too fast
            if (result.linear.magnitude > robotState.MaxSpeed)
            {
                //sresult.linear.y = 0f;
                result.linear.Normalize();
                result.linear *= robotState.MaxSpeed;
            }

            return result;
        }
    }
}