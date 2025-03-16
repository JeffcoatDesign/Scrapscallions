using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Scraps
{
    public class PostProcessingManager : MonoBehaviour
    {
        public static PostProcessingManager Instance;
        [SerializeField] private VolumeProfile m_postProcessingProfile;
        [SerializeField] private float m_fadeInSpeed = 2;
        [SerializeField] private float m_fadeOutSpeed = 2;
        [SerializeField] private float m_maxIntensity = 0.56f;
        private float m_vignetteIntensity = 0;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDisable()
        {
            if (m_postProcessingProfile.TryGet(out Vignette vig))
            {
                vig.intensity.Override(0);
            }
        }

        public void ShowVignette() => StartCoroutine(FadeInVignette());
        public void HideVignette() => StartCoroutine(FadeOutVignette());

        private IEnumerator FadeInVignette()
        {
            if (m_postProcessingProfile.TryGet(out Vignette vig))
            {
                while (m_vignetteIntensity < 1)
                {
                    m_vignetteIntensity = Mathf.Clamp01(m_vignetteIntensity + Time.deltaTime * m_fadeInSpeed);
                    vig.intensity.Override(m_vignetteIntensity * m_maxIntensity);
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return null;
        }
        private IEnumerator FadeOutVignette()
        {
            if (m_postProcessingProfile.TryGet(out Vignette vig))
            {
                while (m_vignetteIntensity > 0)
                {
                    m_vignetteIntensity = Mathf.Clamp01(m_vignetteIntensity - Time.deltaTime * m_fadeOutSpeed);
                    vig.intensity.Override(m_vignetteIntensity * m_maxIntensity);
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return null;
        }
    }
}