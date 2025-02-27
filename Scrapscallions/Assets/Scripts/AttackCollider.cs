using Scraps.Parts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttackCollider : MonoBehaviour
{
    [SerializeField] private ArmController m_armController;
    private List<PartController> m_hitParts = new();
    public bool canHit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!canHit && !other.CompareTag("Robot")) return;
        
        if (other.TryGetComponent(out PartController otherPart))
        {
            if (otherPart.GetRobot() == m_armController.GetRobot()) return;
            if (otherPart.isBroken) return;
            if (!m_hitParts.Contains(otherPart))
            {
                m_hitParts.Add(otherPart);
                otherPart.Hit(m_armController.arm.AttackDamage);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PartController otherPart))
        {
            if (m_hitParts.Contains(otherPart))
            {
                m_hitParts.Remove(otherPart);
            }
        }
    }
}
