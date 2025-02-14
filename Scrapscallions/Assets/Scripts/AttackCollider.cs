using Scraps.Parts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttackCollider : MonoBehaviour
{
    [SerializeField] private ArmController m_armController;
    private bool m_canHit = false;

    private void OnEnable()
    {
        if (m_armController == null) return;

        m_armController.Attacked += OnAttack;
        m_armController.AttackEnded += OnAttackEnded;
    }

    private void OnAttack() => m_canHit = true;
    private void OnAttackEnded() => m_canHit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!m_canHit || !other.CompareTag("Robot")) return;
        
        if (other.TryGetComponent(out PartController otherPart))
        {
            if (otherPart.GetRobot() == m_armController.GetRobot()) return;
            if (otherPart.isBroken) return;
            otherPart.Hit(m_armController.arm.AttackDamage);
        }
    }
}
