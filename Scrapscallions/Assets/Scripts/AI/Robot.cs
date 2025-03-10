using Scraps;
using Scraps.AI.GOAP;
using Scraps.Parts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Robot", menuName = "Robot")]
public class Robot : ScriptableObject
{
    public RobotPartHead head;
    public RobotPartBody body;
    public RobotPartLegs legs;
    public RobotPartArm leftArm;
    public RobotPartArm rightArm;
    public GoapAgent agent;

    [HideInInspector] public HeadController headController;
    [HideInInspector] public BodyController bodyController;
    [HideInInspector] public LegsController legsController;
    [HideInInspector] public ArmController leftArmController;
    [HideInInspector] public ArmController rightArmController;

    public RobotState State { get; private set; }
    public Func<GameObject> AgentObject;

    public float TotalHealth { get; private set; }

    internal void Spawn(GoapAgent agent, Robot target, bool isPlayer)
    {
        this.agent = agent;

        legsController = legs.Spawn(agent);
        bodyController = body.Spawn(legsController);
        headController = head.Spawn(bodyController);
        leftArmController = leftArm.Spawn(bodyController, false);
        rightArmController = rightArm.Spawn(bodyController, true);

        headController.head = head;
        bodyController.body = body;
        leftArmController.arm = leftArm;
        rightArmController.arm = rightArm;

        legsController.Initialize(this);
        bodyController.Initialize(this);
        headController.Initialize(this);
        leftArmController.Initialize(this);
        rightArmController.Initialize(this);

        State = new(headController, bodyController, legsController, leftArmController, rightArmController, agent, target, isPlayer);

        State.character.myTarget = target.AgentObject;

        AgentObject = () => agent.gameObject;
        agent.Initialize(this);
    }
}
