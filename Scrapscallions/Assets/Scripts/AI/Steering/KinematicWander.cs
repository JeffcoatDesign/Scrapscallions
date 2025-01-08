using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI
{
    [CreateAssetMenu(fileName = "New Kinematic Wander", menuName = "Steering/Kinematic Wander")]
    public class KinematicWander : SteeringBehavior
    {
        public override SteeringOutput GetSteering(RobotState robotState)
        {
            SteeringOutput result = new()
            {
                linear = robotState.maxSpeed * robotState.character.transform.forward,
                angular = RandomBinomial() * robotState.maxAngularAcceleration
            };

            return result;
        }

        private float RandomBinomial() => Random.value - Random.value;
    }
}