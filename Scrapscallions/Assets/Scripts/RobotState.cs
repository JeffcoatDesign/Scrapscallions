using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RobotState
{
    public Kinematic character;
    public GameObject target;
    public GameObject[] obstacles;
    public Kinematic[] targets;
    [SerializeField] private Path m_path;
    public Path Path
    {
        get
        {
            if (m_path != null) return m_path;
            Path newPath = Object.FindObjectOfType<Path>();
            if (newPath != null) return newPath;
            return null; 
        }
    }
    public Vector2 Position { 
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


    public RobotState (Kinematic character, GameObject target = null, GameObject[] obstacles = null, Kinematic[] targets = null, Path path = null, float maxSpeed = 1f, float maxAngularAcceleration = 45f, float collisionRadius = 0.5f)
    {
        this.character = character;
        this.target = target;
        this.obstacles = obstacles;
        this.targets = targets;
        m_path = path;
        this.maxSpeed = maxSpeed;
        this.maxAngularAcceleration = maxAngularAcceleration;
        this.collisionRadius = collisionRadius;
    }
}
