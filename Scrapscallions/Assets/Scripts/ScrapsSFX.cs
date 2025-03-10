using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class ScrapsSFX : MonoBehaviour
    {
        [SerializeField] private AudioSource m_audioSource;
        [SerializeField] private Vector2 m_randomPitch = new(0.8f,1.2f);

        public void Play()
        {
            m_audioSource.pitch = Random.Range(m_randomPitch.x, m_randomPitch.y);
            m_audioSource.Play();
        }

        public void Stop()
        {
            m_audioSource.Stop();
        }

        private void Reset()
        {
            m_audioSource = GetComponent<AudioSource>();
        }
    }
}