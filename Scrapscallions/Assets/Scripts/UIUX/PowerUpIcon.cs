using System;
using System.Collections;
using System.Collections.Generic;
using Scraps.Parts;
using TMPro;
using UnityEngine;

namespace Scraps.UI
{
    public class PowerUpIcon : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_text;
        [SerializeField] private Color m_baseColor;
        [SerializeField] private Color m_inactiveColor;
        private PowerUpController m_powerUpController;
        internal void Initialize(PowerUpController powerUpController)
        {
            m_powerUpController = powerUpController;

            m_text.text = $"[{powerUpController.powerUpKey.ToUpper()}]";

            powerUpController.CooledDown += OnStatusChanged;
            powerUpController.OnActivated.AddListener(OnStatusChanged);

            OnStatusChanged();
        }

        private void OnDisable()
        {
            if (m_powerUpController != null)
            {
                m_powerUpController.CooledDown -= OnStatusChanged;
                m_powerUpController.OnActivated.RemoveListener(OnStatusChanged);
            }
        }

        private void OnStatusChanged()
        {
            m_text.color = m_powerUpController.IsReady ? m_baseColor : m_inactiveColor;
        }

        private void Reset()
        {
            m_text = GetComponent<TextMeshProUGUI>();
            m_baseColor = m_text.color;
        }
    }
}