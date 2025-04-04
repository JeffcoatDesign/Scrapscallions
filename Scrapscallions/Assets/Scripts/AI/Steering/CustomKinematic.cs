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
    private bool m_canMove = true;

    internal void SetSteeringBehavior(SteeringBehavior steeringBehavior)
    {
        m_steeringBehavior = steeringBehavior;
    }

    internal void DisableMovement()
    {
        m_canMove = false;
    }

    internal void EnableMovement()
    {
        m_canMove = true;
    }

    private void Start()
    {
        robotState = agent.robot.State;
        maxSpeed = robotState.maxSpeed;
        robotState.maxAngularAcceleration = maxAngularVelocity;

        if (m_steeringBehavior != null)
        {
            m_steeringInstance = m_steeringBehavior.Clone();
        }
    }

    protected override void Update()
    {
        maxSpeed = robotState.maxSpeed;
        if (!m_canMove || !robotState.CanMove)
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
