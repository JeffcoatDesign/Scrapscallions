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
        public GameObject[] obstacles;
        public Kinematic[] targets;

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
        public Vector2 Position
        {
            get
            {
                if (character != null)
                {
                    Vector2 position = new(character.transform.position.x, character.transform.position.z);
                    return position;
                }
                return Vector2.zero;
            }
        }

        public float maxSpeed = 1f;
        public float maxAngularAcceleration = 45f;
        public float collisionRadius = 0.5f;

        public RobotState(HeadController headController, BodyController bodyController, LegsController legsController, 
            ArmController leftArmController, ArmController rightArmController, Kinematic character, Robot targetRobot)
        {
            m_headController = headController;
            m_bodyController = bodyController;
            m_legsController = legsController;
            m_leftArmController = leftArmController;
            m_rightArmController = rightArmController;
            this.character = character;
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
    }
}