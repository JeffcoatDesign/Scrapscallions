using System.Collections;
using System.Collections.Generic;
using Scraps.Parts;
using UnityEngine;

namespace Scraps.UI
{
    public class PowerUpsDisplay : MonoBehaviour
    {
        [SerializeField] private PowerUpIcon m_powerUpIconPrefab;
        private Dictionary<PowerUpController, PowerUpIcon> m_powerUpIcons = new();

        public void AddPowerUp(PowerUpController powerUpController)
        {
            if (m_powerUpIcons.ContainsKey(powerUpController))
            {
                Debug.LogWarning("Power Up Display already contains Icon for power up controller: " + powerUpController.gameObject.name);
                return;
            }
            PowerUpIcon icon = Instantiate(m_powerUpIconPrefab, transform);

            icon.Initialize(powerUpController);

            m_powerUpIcons.Add(powerUpController, icon);

            powerUpController.partController.Broke +=
                () =>
                {
                    RemovePowerUp(powerUpController, icon);
                };
        }

        public void RemovePowerUp(PowerUpController powerUpController, PowerUpIcon icon = null)
        {
            if (m_powerUpIcons.ContainsKey(powerUpController))
            {
                if (icon == null)
                    icon = m_powerUpIcons[powerUpController];
                
                //Return if its still null.
                if (icon == null) return;
                
                m_powerUpIcons.Remove(powerUpController);
                Destroy(icon.gameObject);
            }
        }
    }
}