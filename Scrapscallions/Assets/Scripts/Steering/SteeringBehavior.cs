using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI
{
    [Serializable]
    public abstract class SteeringBehavior : ScriptableObject, ISteeringBehavior
    {
        public string behaviorName;
        public abstract SteeringOutput GetSteering(RobotState robotState);

        public virtual SteeringBehavior Clone()
        {
            return Instantiate(this);
        }
    }

    public interface ISteeringBehavior
    {
        public SteeringOutput GetSteering(RobotState robotState);
        public SteeringBehavior Clone();
    }
}