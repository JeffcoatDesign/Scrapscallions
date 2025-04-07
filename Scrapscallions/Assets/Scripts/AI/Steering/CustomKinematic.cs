using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scraps.AI;
using Scraps;
using Scraps.AI.GOAP;

[RequireComponent(typeof(Rigidbody))]
public class CustomKinematic : Kinematic
{
    public RobotState robotState;

    [SerializeField] private GoapAgent agent;
    [SerializeField] private SteeringBehavior m_steeringBehavior;

    private SteeringBehavior m_steeringInstance;

    internal void SetSteeringBehavior(SteeringBehavior steeringBehavior)
    {
        m_steeringBehavior = steeringBehavior;
    }

    internal void DisableMovement()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        robotState.CanMove = false;
    }

    internal void EnableMovement()
    {
        robotState.CanMove = true;
    }

    public void Initialize()
    {
        robotState = agent.robot.State;
        maxSpeed = robotState.MaxSpeed;
        maxAngularVelocity = robotState.MaxAngularAcceleration;

        if (m_steeringBehavior != null)
        {
            m_steeringInstance = m_steeringBehavior.Clone();
        }
    }

    protected override void Update()
    {
        maxSpeed = robotState.MaxSpeed;
        maxAngularVelocity = robotState.MaxAngularAcceleration;
        if (!robotState.CanMove || !robotState.CanMove)
        {
            SteeringOutput stationary = new()
            {
                linear = Vector3.zero,
                angular = 0
            };
            steeringUpdate = stationary;
            base.Update();
            return;
        }

        if (m_steeringInstance != null)
            steeringUpdate = m_steeringInstance.GetSteering(robotState);

        base.Update();
    }
}
