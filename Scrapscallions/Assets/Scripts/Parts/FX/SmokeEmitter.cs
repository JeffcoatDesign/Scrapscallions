using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    public class SmokeEmitter : MonoBehaviour
    {
        [SerializeField] PartController m_part;
        private ParticleSystem m_particleSystem;

        private void OnEnable()
        {
            m_particleSystem = GetComponent<ParticleSystem>();
            if (m_part != null)
            {
                m_part.Broke += OnBreak;
            }
        }
        private void OnDisable()
        {
            if (m_part != null)
            {
                m_part.Broke -= OnBreak;
            }
        }

        private void OnBreak()
        {
            m_particleSystem.Play();
        }
    }
}