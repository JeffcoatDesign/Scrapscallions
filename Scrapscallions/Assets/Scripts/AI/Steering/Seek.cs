using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI
{
    [CreateAssetMenu(fileName = "New Seek", menuName = "Steering/Seek")]
    public class Seek : SteeringBehavior
    {
        internal RobotState robotState;
        public bool flee = false;

        protected virtual Vector3 GetTargetPosition(GameObject target)
        {
            return target.transform.position.With(y:0);
        }

        public override SteeringOutput GetSteering(RobotState robotState)
        {
            this.robotState = robotState;
            SteeringOutput result = new SteeringOutput();
            Vector3 targetPosition = GetTargetPosition(robotState.target());
            if (targetPosition == Vector3.positiveInfinity)
            {
                return null;
            }

            // Get the direction to the target
            if (flee)
            {
                //result.linear = character.transform.position - target.transform.position;
                result.linear = robotState.Position - targetPosition;
            }
            else
            {
                //result.linear = target.transform.position - character.transform.position;
                result.linear = targetPosition - robotState.Position;
            }

            // give full acceleration along this direction
            result.linear.Normalize();
            result.linear *= robotState.MaxSpeed;

            result.angular = 0;
            return result;
        }
    }
}