using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    [RequireComponent(typeof(ParticleSystem))]
    public class SparksEmitter : MonoBehaviour
    {
        [SerializeField] PartController m_part;
        private ParticleSystem m_particleSystem;

        private void OnEnable()
        {
            m_particleSystem = GetComponent<ParticleSystem>();
            if (m_part != null)
            {
                m_part.PartHit += OnPartHit;
            }
        }
        private void OnDisable()
        {
            if (m_part != null)
            {
                m_part.PartHit -= OnPartHit;
            }
        }

        private void OnPartHit(int amount)
        {
            m_particleSystem.Play();
        }
    }
}