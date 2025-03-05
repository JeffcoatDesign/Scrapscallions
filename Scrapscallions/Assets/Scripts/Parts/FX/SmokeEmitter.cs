using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Scraps.Parts
{
    public class SmokeEmitter : MonoBehaviour
    {
        [SerializeField] PartController m_part;
        private VisualEffect m_visualEffect;

        private void OnEnable()
        {
            m_visualEffect = GetComponent<VisualEffect>();
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
            m_visualEffect.enabled = true;
        }
    }
}