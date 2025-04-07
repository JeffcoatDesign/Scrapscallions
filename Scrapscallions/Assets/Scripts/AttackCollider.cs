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
    private List<int> m_hitParts = new();
    private bool m_canHit = false;
    public bool CanHit {  
        get { return m_canHit; } 
        set 
        {
            if(!value)
            {
                m_hitParts.Clear();
            }
            if (m_canHit != value) 
            { 
                m_canHit = value; 
                if(value)
                {
                    Debug.Log("New Attack");
                }
            }
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CanHit && !other.CompareTag("Robot")) return;
        
        if (other.TryGetComponent(out PartController otherPart))
        {
            if (m_partController == null)
            {
                Debug.Log("No part controller found!");
                return;
            }
            if (otherPart.GetRobot() == null) return;
            if (otherPart.GetRobot() == m_partController.GetRobot()) return;
            if (otherPart.isBroken) return;
            int instanceID = otherPart.GetInstanceID();
            if (!m_hitParts.Contains(instanceID))
            {
                m_hitParts.Add(instanceID);

                Debug.Log("Hitting part: " + otherPart.name + " with instanceID: " + instanceID);

                if (m_partController is ArmController arm)
                    otherPart.Hit(arm.arm.AttackDamage);
                else
                {
                    otherPart.Hit(1);
                }
            }
        }
    }
}
