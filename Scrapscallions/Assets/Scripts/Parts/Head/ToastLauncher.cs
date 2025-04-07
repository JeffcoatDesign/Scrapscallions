using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Parts;
using Scraps.Gameplay;
using UnityEngine;

namespace Scraps.Parts
{
    public class ToastLauncher : MonoBehaviour
    {
        [SerializeField] private HeadController m_headController;
        [SerializeField] private PowerUpController m_powerUpController;
        [SerializeField] private Projectile m_toastPrefab;
        [SerializeField] private float m_toastVelocity = 10f;
        [SerializeField] private float m_respawnTime = 1f;
        [SerializeField] private Sensor m_toastSensor;
        private Projectile m_toast;
        private Robot m_robot;

        private void OnEnable()
        {
            m_headController.PartInitialized += OnInitialize;
            m_powerUpController.Activated += OnActivated;
        }

        private void OnDisable()
        {
            m_headController.PartInitialized -= OnInitialize;
            m_powerUpController.Activated -= OnActivated;
        }

        private void OnActivated()
        {
            if (m_toast != null)
                FireToast();
        }

        private void FireToast()
        {
            Debug.Log("Launching toast");

            Vector3 targetPos;

            //Use closest part if a target is in range or use opponent position
            if (m_toastSensor.IsTargetInRange)
                targetPos = m_toastSensor.TargetPosition;
            else
                targetPos = m_robot.State.target().transform.position;

            Vector3 delta = targetPos - transform.position;
            float a = Physics.gravity.sqrMagnitude;
            float b = -4 * (Vector3.Dot(Physics.gravity, delta) + m_toastVelocity * m_toastVelocity);
            float c = 4 * delta.sqrMagnitude;

            float b2minus4ac = (b * b) - (4 * a * c);
            if (b2minus4ac < 0) return;

            float time0 = Mathf.Sqrt((-b + Mathf.Sqrt(b2minus4ac)) / (2 * a));
            float time1 = Mathf.Sqrt((-b - Mathf.Sqrt(b2minus4ac)) / (2 * a));

            float timeToTarget;
            if (time0 < 0)
            {
                if (time1 < 0)
                {
                    return;
                }
                else
                    timeToTarget = time1;
            }
            else if (time1 < 0)
            {
                timeToTarget = time0;
            }
            else
            {
                timeToTarget = time0 < time1 ? time0 : time1;
            }
            Vector3 delta2 = delta * 2;
            Vector3 gravTtt2 = Physics.gravity * (timeToTarget * timeToTarget);
            float divisor = 2 * m_toastVelocity * timeToTarget;

            Vector3 solution = (delta2 - gravTtt2) / divisor;

            m_toast.Launch(solution, m_toastVelocity);
            m_toast = null;
            Invoke(nameof(SpawnToast), m_respawnTime);
        }

        private void OnInitialize()
        {
            m_robot = m_headController.GetRobot();
            SpawnToast();
        }

        private void Update()
        {
            if (m_powerUpController.IsReady && m_toast == null)
                SpawnToast();
        }

        private void SpawnToast()
        {
            m_toast = Instantiate(m_toastPrefab, transform);
            m_toast.robot = m_robot;
            m_toast.GetComponent<DetachOnBreak>().part = m_headController;
        }
    }
}