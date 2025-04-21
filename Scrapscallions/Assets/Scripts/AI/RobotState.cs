using Scraps.AI;
using Scraps.AI.GOAP;
using Scraps.Parts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps
{
    /// <summary>
    /// Tracks the state of the robot
    /// </summary>
    [System.Serializable]
    public class RobotState
    {
        public CustomKinematic character;
        public Func<GameObject> target;
        public Func<Vector3> destination;
        public GameObject[] obstacles;
        public Kinematic[] targets;

        public bool hasPath = false;
        public bool isAlive = true;
        internal bool isPursuing = true;
        public bool isPlayer;
        public float freezeTime = 0;

        private bool m_canMove = true;
        public bool CanMove
        {
            get
            {
                if(LegsController != null)
                {
                    return !LegsController.isBroken && !(freezeTime > 0) && m_canMove;
                }
                return false;
            }
            set => m_canMove = value;
        }
        public bool IsDisarmed { get => LeftArmController.isBroken && RightArmController.isBroken; }

        public float MaxSpeed { 
            get
            {
                if (!CanMove || Robot.legs == null)
                {
                    return 0f;
                } else
                {
                    return Robot.legs.MaxSpeed;
                }
            } 
        }
        public float MaxAngularAcceleration { 
            get
            {
                if (!CanMove || Robot.legs == null)
                {
                    return 0f;
                }
                else
                {
                    return Robot.legs.MaxAngularAcceleration;
                }
            }
                
        }
        public float collisionRadius = 0.5f;

        public Robot Robot { get; private set; }

        //Physical Parts
        private readonly HeadController m_headController;
        private readonly BodyController m_bodyController;
        private readonly LegsController m_legsController;
        private readonly ArmController m_leftArmController;
        private readonly ArmController m_rightArmController;

        public HeadController HeadController { get => m_headController; }
        public BodyController BodyController { get => m_bodyController; }
        public LegsController LegsController { get => m_legsController; }
        public ArmController LeftArmController { get => m_leftArmController; }
        public ArmController RightArmController { get => m_rightArmController; }
        public GoapAgent Agent { get; private set; }

        [SerializeField] private Path m_path;
        public Path Path
        {
            get
            {
                if (m_path != null) return m_path;
                Path newPath = UnityEngine.Object.FindObjectOfType<Path>();
                if (newPath != null) return newPath;
                return null;
            }
        }
        public Vector3 Position
        {
            get
            {
                if (Agent != null)
                {
                    Vector3 position = Agent.transform.position;
                    return position;
                }
                return Vector3.zero;
            }
        }
        public float RemainingDistance { get => Vector3.Distance(Agent.transform.position, destination()); }

        public RobotState(Robot robot, HeadController headController, BodyController bodyController, LegsController legsController, 
            ArmController leftArmController, ArmController rightArmController, GoapAgent agent, Robot targetRobot, bool isPlayer)
        {
            Robot = robot;

            this.isPlayer = isPlayer;

            m_headController = headController;
            m_bodyController = bodyController;
            m_legsController = legsController;
            m_leftArmController = leftArmController;
            m_rightArmController = rightArmController;
            Agent = agent;

            if (agent.TryGetComponent(out CustomKinematic kinematic))
            {
                character = kinematic;
            }
            else
                Debug.LogError("Character Not Found!");
            target = () => targetRobot.State.character.gameObject;
        }

        internal void SetDestination(Func<Vector3> destination)
        {
            this.destination = destination;
            hasPath = true;
        }

        internal void ResetPath()
        {
            destination = () => Agent.transform.position;
            hasPath = false;
        }

        internal SteeringBehavior GetSteeringBehavior(string key)
        {
            return m_headController.GetSteeringBehavior(key);
        }
    }
}