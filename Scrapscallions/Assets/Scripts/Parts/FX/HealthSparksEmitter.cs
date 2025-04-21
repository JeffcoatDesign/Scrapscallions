using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    [RequireComponent(typeof(ParticleSystem))]
    public class HealthSparksEmitter : MonoBehaviour
    {
        [SerializeField] PartController m_part;
        [SerializeField] float m_sparkRate = 1f;
        private ParticleSystem m_particleSystem;

        private void OnEnable()
        {
            m_particleSystem = GetComponent<ParticleSystem>();
            if (m_part != null)
            {
                m_part.PartHit += OnPartHit;
                m_part.Broke += OnBroke;
            }
        }

        private void OnBroke()
        {
            m_particleSystem.Play();
        }

        private void OnDisable()
        {
            if (m_part != null)
            {
                m_part.PartHit -= OnPartHit;
                m_part.Broke -= OnBroke;
            }
        }

        private void OnPartHit(int amount)
        {
            m_particleSystem.Play();
        }
    }
}