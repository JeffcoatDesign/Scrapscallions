using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI
{
    [CreateAssetMenu(fileName = "New Separation", menuName = "Steering/Separation")]
    public class Separation : SteeringBehavior
    {
        float maxAcceleration = 1f;

        // the threshold to take action
        public float threshold = 5f; // 5

        // the constant coefficient of decay for the inverse square law
        public float decayCoefficient = 100f;

        public override SteeringOutput GetSteering(RobotState robotState)
        {
            SteeringOutput result = new SteeringOutput();

            foreach (Kinematic target in robotState.targets)
            {
                Vector2 direction = robotState.Position - new Vector2(target.transform.position.x, target.transform.position.z);
                float distance = direction.magnitude;

                if (distance < threshold)
                {
                    // calculate the strength of repulsion
                    float strength = Mathf.Min(decayCoefficient / (distance * distance), maxAcceleration);
                    direction.Normalize();
                    result.linear += strength * direction;
                }
            }

            return result;
        }
    }
}