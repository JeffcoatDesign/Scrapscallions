using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scraps.AI;

public class CustomKinematic : Kinematic
{
    public RobotState robotState;

    [SerializeField] private SteeringBehavior m_steeringBehavior;

    private SteeringBehavior m_steeringInstance;

    private void Start()
    {
        robotState = new(this)
        {
            target = myTarget,
            maxSpeed = maxSpeed
        };

        if (m_steeringBehavior != null)
        {
            m_steeringInstance = m_steeringBehavior.Clone();
        }
    }

    protected override void Update()
    {
        if (m_steeringInstance != null)
            steeringUpdate = m_steeringInstance.GetSteering(robotState);

        base.Update();
    }
}
