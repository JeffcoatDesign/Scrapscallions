using Scraps.AI.GOAP;
using Scraps.Gameplay;
using Scraps.UI;
using Scraps.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;

namespace Scraps.Parts
{
    [RequireComponent(typeof(BashActionController))]
    public class BodyController : PartController
    {
        public RobotPartBody body;
        [SerializeField] private Transform m_headAttachPoint;
        [SerializeField] private Transform m_leftArmAttachPoint;
        [SerializeField] private Transform m_rightArmAttachPoint;
        [SerializeField] private ActionController m_bashController;
        [SerializeField] private Sensor m_bashSensor;
        public Transform HeadAttachPoint { get => m_headAttachPoint; }
        public Transform LeftArmAttachPoint { get => m_leftArmAttachPoint; }
        public Transform RightArmAttachPoint { get => m_rightArmAttachPoint; }
        override public void Break()
        {
            isBroken = true;
            body.IsBroken = true;
            body.Break?.Invoke();
            Broke?.Invoke();
            body.CurrentHP = 0;
        }

        override public void GetActions(GoapAgent agent, SerializableHashSet<AgentAction> actions, Dictionary<string, AgentBelief> agentBeliefs)
        {
            actions.Add(new AgentAction.Builder("BodyBash")
                .WithCost(3)
                .WithPrecondition(agentBeliefs["IsDisarmed"])
                .WithPrecondition(agentBeliefs["CanMove"])
                .WithPrecondition(agentBeliefs["InBashRange"])
                .AddEffect(agentBeliefs["AttackingOpponent"])
                .WithStrategy(ScriptableObject.CreateInstance<TakeActionStrategy>().Initialize(m_bashController))
                .Build()
            );
            actions.Add(new AgentAction.Builder("MoveToBashRange")
                .WithCost(3)
                .WithPrecondition(agentBeliefs["IsDisarmed"])
                .WithPrecondition(agentBeliefs["CanMove"])
                .AddEffect(agentBeliefs["InBashRange"])
                .WithStrategy(ScriptableObject.CreateInstance<MoveToStrategy>().Initialize(m_robot.State, () => m_robot.State.target().transform.position, m_bashSensor.detectionRadius / 2))
                .Build()
            );
        }

        override public void GetBeliefs(GoapAgent agent, Dictionary<string, AgentBelief> agentBeliefs)
        {
            BeliefFactory beliefFactory = new(agent, agentBeliefs);
            beliefFactory.AddSensorBelief("InBashRange", m_bashSensor);
        }

        override public void GetGoals(GoapAgent agent, SerializableHashSet<AgentGoal> goals, Dictionary<string, AgentBelief> agentBeliefs)
        {
            //NOOP
        }

        override public Robot GetRobot()
        {
            return m_robot;
        }

        override public void Hit(int damage)
        {
            base.Hit(damage);
            PartHit?.Invoke(damage);

            if (body.IsBroken) return;

            int currentHP = body.CurrentHP - damage;
            if (currentHP <= 0)
            {
                currentHP = 0;
                Break();
            }

            body.CurrentHP = currentHP;
        }

        override public void Repair(int amount)
        {
            throw new System.NotImplementedException();
        }
    }
}