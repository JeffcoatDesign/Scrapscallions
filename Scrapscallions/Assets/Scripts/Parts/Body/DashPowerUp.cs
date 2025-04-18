using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Scraps.Parts.PowerUps
{
    public class DashPowerUp : MonoBehaviour
    {
        [SerializeField] private PartController m_partController;
        [SerializeField] private AttackCollider m_attackCollider;
        [SerializeField] private float m_leapForce = 4f;
        [SerializeField] private float m_leapTime = 0.5f;
        [SerializeField] private float m_anticipationForce = 1f;
        [SerializeField] private float m_anticipationTime = 0.3f;
        public void Activate()
        {
            StartCoroutine(JumpForward());
        }

        private IEnumerator JumpForward()
        {
            Robot robot = null;
            if (m_partController != null)
                robot = m_partController.GetRobot();
            if (robot != null)
            {
                float time = 0;
                Rigidbody rb = robot.agent.GetComponent<Rigidbody>();
                Vector3 direction = robot.State.target().transform.position - robot.State.Position;
                direction.Normalize();

                while (time < m_leapTime + m_anticipationTime)
                {
                    if (time < m_anticipationTime)
                    {
                        robot.State.SetDestination(() => robot.State.target().transform.position);
                        rb.AddForce(-direction * m_leapForce / 4, ForceMode.VelocityChange);
                    }
                    else
                    {
                        robot.State.SetDestination(() => robot.State.target().transform.position);
                        rb.AddForce(direction * m_leapForce, ForceMode.VelocityChange);
                        m_attackCollider.CanHit = true;
                    }

                    yield return new WaitForEndOfFrame();
                    time += Time.deltaTime;
                }
            }
            else
                Debug.Log("Robot is null");

            m_attackCollider.CanHit = false;
        }
    }
}