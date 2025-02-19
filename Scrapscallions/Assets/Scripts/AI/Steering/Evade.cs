using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI
{
    [CreateAssetMenu(fileName = "New Evade", menuName = "Steering/Evade")]
    public class Evade : Pursue
    {
        protected override Vector3 GetTargetPosition(GameObject target)
        {
            // 1. figure out how far ahead in time we should predict
            Vector3 directionToTarget = robotState.character.transform.position - target.transform.position;
            float distanceToTarget = directionToTarget.magnitude;
            float mySpeed = robotState.character.linearVelocity.magnitude;
            float predictionTime;
            Kinematic myMovingTarget = target.GetComponent<Kinematic>();
            if (myMovingTarget == null)
            {
                // default to seek behavior for non-kinematic targets
                Debug.Log("Seeking");
                return base.GetTargetPosition(target);
            }
            if (mySpeed <= distanceToTarget / maxPredictionTime)
            {
                // if I'm far enough away, I can use the max prediction time
                predictionTime = maxPredictionTime;
            }
            else
            {
                predictionTime = distanceToTarget / myMovingTarget.linearVelocity.magnitude;
            }

            Vector3 predictedEvasion = robotState.character.transform.position + myMovingTarget.linearVelocity * predictionTime;
            return new(predictedEvasion.x, 0, predictedEvasion.z);
        }
    }
}