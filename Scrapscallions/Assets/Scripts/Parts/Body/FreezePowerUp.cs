using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Scraps.Parts.PowerUps
{
    public class FreezePowerUp : MonoBehaviour
    {
        [SerializeField] private PartController m_partController;
        [SerializeField] private float m_startRadius = 1f;
        [SerializeField] private float m_endRadius = 10f;
        [SerializeField] private float m_blastTime = 0.5f;
        [SerializeField] private float m_freezeTime = 3f;
        //TODO REmove this when freezing
        [SerializeField] private int m_freezeDamage;

        private ParticleSystem m_particleSystem;
        private SphereCollider m_collider;

        private void OnEnable()
        {
            m_collider = GetComponent<SphereCollider>();
            m_particleSystem = GetComponent<ParticleSystem>();
            m_collider.enabled = false;
        }

        public void Activate()
        {
            StartCoroutine(Freeze());
        }

        private IEnumerator Freeze()
        {
            Robot robot = null;
            if (m_partController != null)
                robot = m_partController.GetRobot();
            if (robot != null)
            {
                float time = 0;
                m_collider.enabled = true;
                m_particleSystem.Play();
                while (time < m_blastTime)
                {
                    m_collider.radius = Mathf.Lerp(m_startRadius, m_endRadius, time / m_blastTime);
                    yield return new WaitForEndOfFrame();
                    time += Time.deltaTime;
                }
                m_particleSystem.Stop();
                m_collider.enabled = false;
            }
            else
                Debug.Log("Robot is null");
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out PartController otherPart))
            {
                Robot otherRobot = otherPart.GetRobot();
                if (otherRobot == null || otherRobot == m_partController.GetRobot())
                    return;

                //TODO Remove this when freezing
                otherPart.Hit(m_freezeDamage);
                Debug.Log("Freeze blast hit: " +  otherRobot.name);

                //TODO Add freezing
                otherRobot.State.freezeTime = m_freezeTime;
            }
        }
    }
}