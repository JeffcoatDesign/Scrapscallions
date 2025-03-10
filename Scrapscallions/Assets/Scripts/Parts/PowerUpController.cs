using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    public class PowerUpController : ActionController
    {
        public bool isUsingPowerUp = false;
        [SerializeField] private KeyCode powerUpKey;
        [SerializeField] private float m_rechargeTime = 10f;
        [SerializeField] private float m_powerTime = 3f;
        [SerializeField] private GameObject m_readyUI;
        public float PowerTime { get => m_powerTime; }
        private PartController m_controller;
        //TODO Make a progress bar

        public float CooldownProgress { get => m_cooldownTimer.Progress; }
        public float PowerUpProgress { get => m_powerUpTimer.Progress; }
        public override bool IsTakingAction { get; set; } = false;
        public override bool IsReady { get { return IsTakingAction && IsCooledDown; } set { } }
        public override bool IsCooledDown { get; set; } = false;
        [field: SerializeField] public override float ActionLength { get; set; } = 2f;
        [field: SerializeField] public override float CooldownTime { get; set; } = 4f;
        public override Action ActionCompleted { get; set; }
        [field: SerializeField] public override string ActionName { get; set; } = "Power Up";

        public event Action Activated;

        private CountdownTimer m_powerUpTimer;

        protected override void Awake()
        {
            base.Awake();

            m_controller = GetComponent<PartController>();

            m_powerUpTimer = new(m_powerTime);

            m_powerUpTimer.OnTimerStop += OnFinished;
        }

        private void OnFinished()
        {
            isUsingPowerUp = false;
            IsTakingAction = false;
            ActionCompleted?.Invoke();
        }

        private void OnRecharged()
        {
            IsReady = true;
        }

        protected override void Update()
        {
            base.Update();

            m_readyUI.SetActive(IsReady);

            if (IsReady && !isUsingPowerUp)
            {
                if (m_controller != null && m_controller.GetRobot().State.isPlayer && Input.GetKeyDown(powerUpKey))
                {
                    Activate();
                }
            }
            if (isUsingPowerUp)
            {
                m_powerUpTimer.Tick(Time.deltaTime);
            }
        }

        public override void Activate()
        {
            isUsingPowerUp = true;
            IsReady = false;
            m_powerUpTimer.Start();
            IsTakingAction = true;
            Activated?.Invoke();
        }
    }
}