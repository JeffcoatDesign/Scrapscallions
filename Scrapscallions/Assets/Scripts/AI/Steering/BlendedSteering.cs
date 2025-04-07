using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI
{
    /// <summary>
    /// Blends two steering behavoirs together
    /// </summary>
    [CreateAssetMenu(fileName = "New Blended Steering", menuName = "Steering/Blended Steering")]
    public class BlendedSteering : SteeringBehavior
    {
        public SteeringBehavior steeringBehavior1, steeringBehavior2;
        public float blendWeight = 0.5f;

        public BlendedSteering(SteeringBehavior steeringBehavior1, SteeringBehavior steeringBehavior2)
        {
            this.steeringBehavior1 = steeringBehavior1;
            this.steeringBehavior2 = steeringBehavior2;
        }

        public override SteeringOutput GetSteering(RobotState robotState)
        {
            SteeringOutput result = new();

            if (steeringBehavior1 != null)
            {
                if (steeringBehavior2 != null)
                {
                    var res1 = steeringBehavior1.GetSteering(robotState);
                    var res2 = steeringBehavior2.GetSteering(robotState);
                    result.linear = res1.linear * blendWeight + res2.linear * (1 - blendWeight);
                    result.angular = res1.angular * blendWeight + res2.angular * (1 - blendWeight);
                }
                else {
                    result = steeringBehavior1.GetSteering(robotState);
                    Debug.Log("steering behavior not found: " + steeringBehavior2.name);
                }
            } 
            else if (steeringBehavior2 != null)
            {
                result = steeringBehavior2.GetSteering(robotState);
                Debug.Log("steering behavior not found: " + steeringBehavior1.name);
            }

            return result;
        }

        public override SteeringBehavior Clone()
        {
            BlendedSteering clone = (BlendedSteering)base.Clone();

            if (steeringBehavior1 != null)
                clone.steeringBehavior1 = steeringBehavior1.Clone();
            else if (steeringBehavior2 != null)
                clone.steeringBehavior2 = steeringBehavior2.Clone();

            return clone;
        }
    }
}