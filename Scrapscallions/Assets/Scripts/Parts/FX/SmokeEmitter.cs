using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Scraps.Parts
{
    public class SmokeEmitter : MonoBehaviour
    {
        [SerializeField] PartController m_part;
        private AudioSource m_audioSource;
        private VisualEffect m_visualEffect;

        private void OnEnable()
        {
            m_audioSource = GetComponent<AudioSource>();
            m_visualEffect = GetComponent<VisualEffect>();

            if (m_visualEffect != null)
                m_visualEffect.Stop();
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
            m_visualEffect.Play();
            if (m_audioSource != null)
                m_audioSource.Play();
        }
    }
}