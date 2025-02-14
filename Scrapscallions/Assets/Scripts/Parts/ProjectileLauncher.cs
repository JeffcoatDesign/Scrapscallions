using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    public class ProjectileLauncher : MonoBehaviour
    {
        [SerializeField] private RangedArmController m_armController;
        [SerializeField] private Projectile m_projectilePrefab;

        private void OnEnable()
        {
            m_armController.Fire += OnFire;
        }

        private void OnDisable()
        {
            m_armController.Fire -= OnFire;
        }

        private void OnFire()
        {
            Instantiate(m_projectilePrefab, transform.position, transform.rotation).robot = m_armController.GetRobot();
        }
    }
}