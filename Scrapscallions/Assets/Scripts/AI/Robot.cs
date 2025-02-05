using Scraps;
using Scraps.AI.GOAP;
using Scraps.Parts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Robot", menuName = "Robot")]
public class Robot : ScriptableObject
{
    public RobotPartHead head;
    public RobotPartBody body;
    public RobotPartLegs legs;
    public RobotPartArm leftArm;
    public RobotPartArm rightArm;
    public RobotState State { get; private set; }

    private Transform m_legsTransform; // Remove when moving remaining distance
    
    //Move to state
    public float RemainingDistance { get => Vector3.Distance(m_legsTransform.position, destination); }

    private Vector3 destination;
    public bool hasPath = false;

    internal void Spawn(GoapAgent agent, Robot target)
    {
        LegsController legsController = legs.Spawn(agent);
        BodyController bodyController = body.Spawn(legsController);
        HeadController headController = head.Spawn(bodyController);
        ArmController leftArmController = leftArm.Spawn(bodyController, false);
        ArmController rightArmController = leftArm.Spawn(bodyController, true);

        State = new(headController, bodyController, legsController, leftArmController, rightArmController, agent.GetComponent<Kinematic>(), target);

        //Remove this when moving remaining distance to state
        m_legsTransform = legsController.transform;

        State.character.myTarget = () => target.State.character.gameObject;

        agent.Initialize(this);
    }

    internal void ResetPath()
    {
        destination = m_legsTransform.position;
        hasPath = false;
    }

    internal void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        hasPath = true;
    }

    void Update()
    {
        /*if (Vector3.Distance(transform.position, destination) > 1f)
        {
            transform.position += (destination - transform.position).normalized * m_speed * Time.deltaTime;
        }
        else
            ResetPath();*/
    }
}
