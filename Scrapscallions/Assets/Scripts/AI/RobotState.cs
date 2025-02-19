using Scraps.AI.GOAP;
using Scraps.Parts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps
{
    [System.Serializable]
    public class RobotState
    {
        public Kinematic character;
        public Func<GameObject> target;
        public Func<Vector3> destination;
        public GameObject[] obstacles;
        public Kinematic[] targets;
        public bool hasPath = false;
        public bool isAlive = true;
        public bool isPlayer;

        private readonly HeadController m_headController;
        private readonly BodyController m_bodyController;
        private readonly LegsController m_legsController;
        private readonly ArmController m_leftArmController;
        private readonly ArmController m_rightArmController;
        private readonly GoapAgent m_agent;

        public HeadController HeadController { get => m_headController; }
        public BodyController BodyController { get => m_bodyController; }
        public LegsController LegsController { get => m_legsController; }
        public ArmController LeftArmController { get => m_leftArmController; }
        public ArmController RightArmController { get => m_rightArmController; }
        public GoapAgent Agent { get => m_agent; }

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
        public float RemainingDistance { get => Vector3.Distance(m_agent.transform.position, destination()); }

        public float maxSpeed = 1f;
        public float maxAngularAcceleration = 45f;
        public float collisionRadius = 0.5f;

        public RobotState(HeadController headController, BodyController bodyController, LegsController legsController, 
            ArmController leftArmController, ArmController rightArmController, GoapAgent agent, Robot targetRobot, bool isPlayer)
        {
            this.isPlayer = isPlayer;
            m_headController = headController;
            m_bodyController = bodyController;
            m_legsController = legsController;
            m_leftArmController = leftArmController;
            m_rightArmController = rightArmController;
            m_agent = agent;
            if (agent.TryGetComponent(out Kinematic kinematic))
            {
                character = kinematic;
            }
            else
                Debug.LogError("Character Not Found!");
            target = () => targetRobot.State.character.gameObject;
        }

        public RobotState(Kinematic character, GameObject target = null, GameObject[] obstacles = null, Kinematic[] targets = null, Path path = null, float maxSpeed = 1f, float maxAngularAcceleration = 45f, float collisionRadius = 0.5f)
        {
            this.character = character;
            this.target = () => target;
            this.obstacles = obstacles;
            this.targets = targets;
            m_path = path;
            this.maxSpeed = maxSpeed;
            this.maxAngularAcceleration = maxAngularAcceleration;
            this.collisionRadius = collisionRadius;
        }

        internal void SetDestination(Func<Vector3> destination)
        {
            this.destination = destination;
            hasPath = true;
        }

        internal void ResetPath()
        {
            destination = () => m_agent.transform.position;
            hasPath = false;
        }
    }
}