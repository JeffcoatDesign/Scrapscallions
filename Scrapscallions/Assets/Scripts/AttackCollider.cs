using Scraps.Parts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Collider))]
public class AttackCollider : MonoBehaviour
{
    [SerializeField] private PartController m_partController;
    private List<PartController> m_hitParts = new();
    public bool canHit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!canHit && !other.CompareTag("Robot")) return;
        
        if (other.TryGetComponent(out PartController otherPart))
        {
            if (otherPart.GetRobot() == null) return;
            if (otherPart.GetRobot() == m_partController.GetRobot()) return;
            if (otherPart.isBroken) return;
            if (!m_hitParts.Contains(otherPart))
            {
                m_hitParts.Add(otherPart);
                if (m_partController is ArmController arm)
                    otherPart.Hit(arm.arm.AttackDamage);
                else
                {
                    otherPart.Hit(1);
                }
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
