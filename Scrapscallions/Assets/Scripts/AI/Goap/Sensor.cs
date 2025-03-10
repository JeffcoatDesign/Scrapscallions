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

    SphereCollider detectionRange;

    public event Action OnTargetChanged = delegate { };

    public Vector3 TargetPosition => Target ? Target.transform.position : Vector3.zero;
    public bool IsTargetInRange => Target != null;

    GameObject Target { get
        {
            if (m_collidingParts != null && m_collidingParts.Count > 0)
            {
                PartController partController = m_collidingParts.First();
                if(partController != null)
                    return partController.gameObject;
            }
            return null;
        } 
    }
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
            UpdateTargetPosition(); 
            timer.Start();
        };
        timer.Start();
    }

    private void Update()
    {
        timer.Tick(Time.deltaTime);
        List<PartController> partsToRemove = new();
        foreach (PartController otherPart in m_collidingParts)
        {
            if (otherPart != null && (otherPart.isBroken || Vector3.Distance(transform.position, otherPart.transform.position) > detectionRadius))
            {
                partsToRemove.Add(otherPart);
                UpdateTargetPosition();
            }
        }
        foreach(PartController otherPart in partsToRemove)
        {
            m_collidingParts.Remove(otherPart);
        }
    }

    void UpdateTargetPosition()
    {
        if(IsTargetInRange && (lastKnownPosition != TargetPosition || lastKnownPosition != Vector3.zero))
        {
            if (Target == null && m_collidingParts.Count > 0)
            {
                var firstPart = m_collidingParts.First();
                while (firstPart == null && m_collidingParts.Count > 0)
                {
                    m_collidingParts.Remove(firstPart);
                    if (m_collidingParts.Count > 0)
                        firstPart = m_collidingParts.First();
                }
            }

            lastKnownPosition = TargetPosition;
            OnTargetChanged.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Robot")) return;
        if (other.TryGetComponent(out PartController otherPart)) {
            if (otherPart.GetRobot() == null) return;
            bool partIsNull = m_part == null;
            bool partRobotIsNull = partIsNull ? true : m_part.GetRobot() == null;
            if (!partIsNull && m_part.GetRobot() == otherPart.GetRobot())
            {
                //Debug.Log("From the same robot");
                return;
            }
            else if (otherPart.isBroken || partIsNull || partRobotIsNull)
            {
                //Debug.Log("Sensor's robot not found");
                return;
            }
        }
        m_collidingParts.Add(otherPart);
        UpdateTargetPosition();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsTargetInRange ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
