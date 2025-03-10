using Scraps.Parts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class HitSFX : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> m_hitSounds = new();
        private PartController m_partController;
        private AudioSource m_audioSource;
        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
            m_partController = GetComponent<PartController>();

            if (m_partController != null)
            {
                m_partController.PartHit += OnHit;
            }
        }

        private void OnHit(int amount)
        {
            int index = UnityEngine.Random.Range(0, m_hitSounds.Count);
            m_audioSource.PlayOneShot(m_hitSounds[index]);
        }
    }
}