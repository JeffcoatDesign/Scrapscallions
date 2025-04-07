using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI
{
    [CreateAssetMenu(fileName = "New Wander", menuName = "Steering/Wander")]
    public class Wander : SteeringBehavior
    {
        public float wanderRadius = 5f;
        public float wanderRate = 5f;
        public float slowRadius = 10f;
        public float timeToTarget = 0.1f;

        private RobotState m_robotState;
        private Vector3 targetPos;
        private float m_lastCheckedTime = 0f;
        private float m_timeUntilWander = 0f;
        public override SteeringOutput GetSteering(RobotState robotState)
        {
            m_robotState = robotState;
            SteeringOutput result = new SteeringOutput();

            // Get the time since the last steering update
            float steeringDeltaTime = Time.time - m_lastCheckedTime;
            m_timeUntilWander -= steeringDeltaTime;

            if (m_timeUntilWander <= 0f) 
            { 
                m_timeUntilWander = wanderRate;
                targetPos = robotState.Position + (GetRandomPointInUnitCircle() * wanderRadius);
            }
            Debug.DrawLine(targetPos, m_robotState.Position, Color.magenta);

            result.angular = Face(targetPos).angular;
            result.linear = robotState.MaxSpeed * robotState.character.transform.forward;

            m_lastCheckedTime = Time.time;

            return result;
        }

        protected Vector3 GetRandomPointInUnitCircle()
        {
            float x = (float)Random.value;
            float z = (float)Random.value;

            float radius = Mathf.Sqrt(x * x + z * z);
            
            x /= radius;
            z /= radius;

            return new(x, 0, z);
        }

        protected float GetTargetAngle(Vector3 target)
        {
            Vector3 direction = (target - m_robotState.Position).With(y:0);
            float targetAngle = Mathf.Atan2(-direction.x, direction.z) * Mathf.Rad2Deg;

            return targetAngle;
        }

        protected SteeringOutput Face(Vector3 target)
        {
            SteeringOutput result = new SteeringOutput();

            // get the naive direction to the target
            //float rotation = Mathf.DeltaAngle(character.transform.eulerAngles.y, target.transform.eulerAngles.y);
            float rotation = Mathf.DeltaAngle(m_robotState.character.transform.eulerAngles.y, GetTargetAngle(target));
            float rotationSize = Mathf.Abs(rotation);


            // if we are outside the slow radius, then use maximum rotation
            float targetRotation;
            if (rotationSize > slowRadius)
            {
                targetRotation = m_robotState.MaxAngularAcceleration;
            }
            else // otherwise use a scaled rotation
            {
                targetRotation = m_robotState.MaxAngularAcceleration * rotationSize / slowRadius;
            }

            // the final targetRotation combines speed (already in the variable) and direction
            targetRotation *= rotation / rotationSize;

            // acceleration tries to get to the target rotation
            // something is breaking my angularVelocty... check if NaN and use 0 if so
            float currentAngularVelocity = float.IsNaN(m_robotState.character.angularVelocity) ? 0f : m_robotState.character.angularVelocity;
            result.angular = targetRotation - currentAngularVelocity;
            result.angular /= timeToTarget;

            // check if the acceleration is too great
            float angularAcceleration = Mathf.Abs(result.angular);
            if (angularAcceleration > m_robotState.MaxAngularAcceleration)
            {
                result.angular /= angularAcceleration;
                result.angular *= m_robotState.MaxAngularAcceleration;
            }

            result.linear = Vector3.zero;
            return result;
        }

        public override SteeringBehavior Clone()
        {
            Wander wander = CreateInstance(nameof(Wander)) as Wander;
            wander.wanderRadius = wanderRadius;
            wander.timeToTarget = timeToTarget;
            wander.slowRadius = slowRadius;
            wander.wanderRate = wanderRate;
            return wander;
        }
    }
}