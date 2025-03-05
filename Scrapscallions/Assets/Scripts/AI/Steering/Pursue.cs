using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI
{
    [CreateAssetMenu(fileName = "New Pursue", menuName = "Steering/Pursue")]
    public class Pursue : Seek
    {
        // the maximum prediction time
        public float maxPredictionTime;

        // overrides the position seek will aim for
        // assume the target will continue travelling in the same direction and speed
        // pick a point farther along that vector
        protected override Vector3 GetTargetPosition(GameObject target)
        {
            // 1. figure out how far ahead in time we should predict
            Vector3 directionToTarget = target.transform.position - robotState.character.transform.position;
            float distanceToTarget = directionToTarget.magnitude;
            float mySpeed = robotState.character.linearVelocity.magnitude;
            float predictionTime;
            if (mySpeed <= distanceToTarget / maxPredictionTime)
            {
                // if I'm far enough away, I can use the max prediction time
                predictionTime = maxPredictionTime;
            }
            else
            {
                // if I'm close enough that my current speed will get me to 
                // the target before the max prediction time elapses
                // use a smaller prediction time
                predictionTime = distanceToTarget / mySpeed;
            }

            // 2. get the current velocity of our target and add an offset based on our prediction time
            //Kinematic myMovingTarget = target.GetComponent(typeof(Kinematic)) as Kinematic;
            Kinematic myMovingTarget = target.GetComponent<Kinematic>();
            if (myMovingTarget == null)
            {
                // default to seek behavior for non-kinematic targets
                return base.GetTargetPosition(target);
            }

            Vector3 predictedPos = target.transform.position + myMovingTarget.linearVelocity * predictionTime;
            return new(predictedPos.x, 0, predictedPos.z);
        }
    }
}