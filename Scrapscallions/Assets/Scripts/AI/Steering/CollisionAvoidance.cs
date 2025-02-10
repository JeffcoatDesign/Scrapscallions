using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI
{
    [CreateAssetMenu(fileName = "New Collision Avoidance", menuName = "Steering/Collision Avoidance")]
    public class CollisionAvoidance : SteeringBehavior
    {
        public Kinematic[] targets;
        public float maxAcceleration;
        public float radius;



        public CollisionAvoidance(Kinematic[] targets, float radius = 0.5f, float maxAcceleration = 100f)
        {
            this.targets = targets;
            this.radius = radius;
            this.maxAcceleration = maxAcceleration;
        }


        public override SteeringOutput GetSteering(RobotState robotState)
        {
            SteeringOutput result = new SteeringOutput();

            float shortestTime = float.MaxValue;

            Kinematic firstTarget = null;
            float firstMinSeperation = float.MaxValue;
            float firstDistance = float.MaxValue;
            Vector3 firstRelativePos = Vector3.one;
            Vector3 firstRelativeVel = Vector3.one;

            foreach (Kinematic target in targets)
            {
                Vector3 relativePos = target.transform.position - robotState.character.transform.position;
                Vector3 relativeVel = robotState.character.linearVelocity - target.linearVelocity;
                float relativeSpeed = relativeVel.magnitude;
                float timeToCollision = Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);

                float distance = relativePos.magnitude;
                float minSeperation = distance - relativeSpeed * timeToCollision;
                if (minSeperation > 2 * radius) continue;

                if (timeToCollision > 0 && timeToCollision < shortestTime)
                {
                    shortestTime = timeToCollision;
                    firstTarget = target;
                    firstMinSeperation = minSeperation;
                    firstDistance = distance;
                    firstRelativePos = relativePos;
                    firstRelativeVel = relativeVel;
                }
            }

            if (firstTarget == null) { return result; }

            float dot = Vector3.Dot(robotState.character.linearVelocity.normalized, firstTarget.linearVelocity.normalized);
            if (dot < -0.9f)
                result.linear = -firstTarget.transform.right;
            else
                result.linear = -firstTarget.transform.forward;

            result.linear.y = 0;
            result.linear.Normalize();

            result.linear *= maxAcceleration;
            result.angular = 0;
            return result;
        }
    }
}