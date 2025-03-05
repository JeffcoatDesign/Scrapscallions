using Scraps.AI.GOAP;
using Scraps.Parts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Sensor : MonoBehaviour
{
    [SerializeField] internal float detectionRadius = 5f;
    [SerializeField] float timerInterval = 1f;
    [SerializeField] private PartController m_part;
    [SerializeField] private GoapAgent m_goapAgent;

    SphereCollider detectionRange;

    public event Action OnTargetChanged = delegate { };

    public Vector3 TargetPosition => target ? target.transform.position : Vector3.zero;
    public bool IsTargetInRange => target != null;

    GameObject target;
    Vector3 lastKnownPosition;
    CountdownTimer timer;
    private List<PartController> m_collidingParts;

    private void Awake()
    {
        detectionRange = GetComponent<SphereCollider>();
        detectionRange.isTrigger = true;
        detectionRange.radius = detectionRadius;
        m_collidingParts = new();
    }

    private void Start()
    {
        timer = new CountdownTimer(timerInterval);
        timer.OnTimerStop += () => { 
            UpdateTargetPosition(target.OrNull()); 
            timer.Start();
        };
        timer.Start();
    }

    private void Update()
    {
        timer.Tick(Time.deltaTime);
    }

    void UpdateTargetPosition(GameObject target = null)
    {
        this.target = target;
        if(IsTargetInRange && (lastKnownPosition != TargetPosition || lastKnownPosition != Vector3.zero))
        {
            lastKnownPosition = TargetPosition;
            OnTargetChanged.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Robot")) return;
        if (other.TryGetComponent(out PartController otherPart)) {
            if (otherPart.GetRobot() == null) return;
            bool partIsNull = m_part == null;
            bool partRobotIsNull = partIsNull ? true : m_part.GetRobot() == null;
            bool agentIsNull = m_goapAgent == null;
            if ((!partIsNull && m_part.GetRobot() == otherPart.GetRobot()) || (!agentIsNull && m_goapAgent.robot == otherPart.GetRobot()))
            {
                //Debug.Log("From the same robot");
                return;
            }
            else if ((partIsNull || partRobotIsNull) && agentIsNull)
            {
                //Debug.Log("Sensor's robot not found");
                return;
            }
        }
        m_collidingParts.Add(otherPart);
        UpdateTargetPosition(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Robot")) return;
        if (other.TryGetComponent(out PartController otherPart))
        {
            if (m_collidingParts.Contains(otherPart))
                m_collidingParts.Remove(otherPart);
            if (m_collidingParts.Count > 0)
            {
                var firstPart = m_collidingParts.First();
                while (firstPart == null && m_collidingParts.Count > 0)
                {
                    m_collidingParts.Remove(firstPart);
                    if (m_collidingParts.Count > 0)
                        firstPart = m_collidingParts.First();
                }
                if (firstPart != null)
                    UpdateTargetPosition(firstPart.gameObject);
            }
            else
                UpdateTargetPosition();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsTargetInRange ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
