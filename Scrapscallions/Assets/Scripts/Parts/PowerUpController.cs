using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    public class PowerUpController : MonoBehaviour
    {
        public bool isCharged = false;
        public bool isUsingPowerUp = false;
        [SerializeField] private KeyCode powerUpKey;
        [SerializeField] private float m_rechargeTime = 10f;
        [SerializeField] private float m_powerTime = 3f;
        [SerializeField] private GameObject m_readyUI;
        public bool IsPowerUpReady { get => isCharged && !isUsingPowerUp; }
        public float PowerTime { get => m_powerTime; }
        private PartController m_controller;
        //TODO Make a progress bar

        public float RechargeProgress { get => m_rechargeTimer.Progress; }
        public float PowerUpProgress { get => m_powerUpTimer.Progress; }
        public event Action Activate, Stop;

        private CountdownTimer m_rechargeTimer, m_powerUpTimer;

        private void Awake()
        {
            m_controller = GetComponent<PartController>();

            m_rechargeTimer = new(m_rechargeTime);
            m_powerUpTimer = new(m_powerTime);

            m_rechargeTimer.OnTimerStop += OnRecharged;
            m_powerUpTimer.OnTimerStop += OnFinished;
        }

        private void OnFinished()
        {
            isUsingPowerUp = false;
            m_rechargeTimer.Start();
            Stop?.Invoke();
        }

        private void OnRecharged()
        {
            isCharged = true;
        }

        private void Update()
        {
            m_readyUI.SetActive(isCharged);

            if (!isCharged && !isUsingPowerUp)
            {
                m_rechargeTimer.Tick(Time.deltaTime);
            } else if (isCharged && !isUsingPowerUp)
            {
                if (m_controller != null && m_controller.GetRobot().State.isPlayer && Input.GetKeyDown(powerUpKey))
                {
                    ActivatePowerUp();
                }
            }
            if (isUsingPowerUp)
            {
                m_powerUpTimer.Tick(Time.deltaTime);
            }
        }

        public void ActivatePowerUp()
        {
            isUsingPowerUp = true;
            isCharged = false;
            m_powerUpTimer.Start();
            Activate?.Invoke();
        }
    }
}