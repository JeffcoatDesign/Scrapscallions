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

    public float PercentHP
    {
        get => TotalCurrentHP / TotalMaxHP;
    }

    public int TotalCurrentHP
    { 
        get
        {
            return head.CurrentHP + body.CurrentHP + legs.CurrentHP + leftArm.CurrentHP + rightArm.CurrentHP;
        }
    }

    public int TotalMaxHP
    { 
        get
        {
            return head.MaxHP + body.MaxHP + legs.MaxHP + leftArm.MaxHP + rightArm.MaxHP;
        }
    }

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
        legsController.legs = legs;

        State = new(this, headController, bodyController, legsController, leftArmController, rightArmController, agent, target, isPlayer);

        State.character.myTarget = target.AgentObject;

        legsController.Initialize(this);
        bodyController.Initialize(this);
        headController.Initialize(this);
        leftArmController.Initialize(this);
        rightArmController.Initialize(this);

        AgentObject = () => agent.gameObject;
        agent.Initialize(this);
    }

    public override string ToString()
    {
        return $"Head: {head.name} \nBody: {body.name} \nLeft arm: {leftArm.name} \nRight Arm: {rightArm.name} \nLegs: {legs.name}";
    }
}
