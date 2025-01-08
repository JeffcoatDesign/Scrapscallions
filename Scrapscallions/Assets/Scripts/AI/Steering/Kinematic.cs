using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Kinematic : MonoBehaviour
{
    public Vector3 linearVelocity;
    public float angularVelocity;  // Millington calls this rotation
    // because I'm attached to a gameobject, we also have:
    // rotation <<< Millington calls this orientation
    // position
    public float maxSpeed = 10.0f;
    public float maxAngularVelocity = 45.0f; // degrees

    public GameObject myTarget;
    private Rigidbody m_rb;

    // child classes will get new steering data for use in our update function
    protected SteeringOutput steeringUpdate;

    // Start is called before the first frame update
    void Awake()
    {
        m_rb = GetComponent<Rigidbody>();

        steeringUpdate = new SteeringOutput(); // default to nothing. should be overriden by children
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // something is breaking my angular velocity
        // check here and reset it if it broke
        if (float.IsNaN(angularVelocity))
        {
            angularVelocity = 0.0f;
        }

        // update my position and rotation - Millington p. 58, lines 7-9
        m_rb.velocity = linearVelocity;
        if (Mathf.Abs(angularVelocity) > 0.01f)
        {
            Vector3 v = new Vector3(0, angularVelocity, 0);
            m_rb.MoveRotation(m_rb.rotation * Quaternion.Euler(v * Time.deltaTime));
        }

        // update linear and angular velocities - I might be accelerating or decelerating, etc.
        // Millington p. 58, lines 11-13
        if (steeringUpdate != null)
        {
            linearVelocity += new Vector3(steeringUpdate.linear.x, 0, steeringUpdate.linear.y) * Time.deltaTime;
            angularVelocity += steeringUpdate.angular * Time.deltaTime;
        }

        // check for speeding and clip
        // Millington p.58, lines 15-18
        // note that Millington's pseudocode on p.58 does not clip angular velocity, but we do here
        if (linearVelocity.magnitude > maxSpeed)
        {
            linearVelocity.Normalize();
            linearVelocity *= maxSpeed;
        }
        if (Mathf.Abs(angularVelocity) > maxAngularVelocity)
        {
            angularVelocity = maxAngularVelocity * (angularVelocity / Mathf.Abs(angularVelocity));
        }
    }

}
