using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scraps.Parts
{
    public class PowerUpController : ActionController
    {
        [SerializeField] private string m_mainPowerUpKey;
        [SerializeField] private string m_altPowerUpKey;
        [SerializeField] private Canvas m_readyUI;
        [SerializeField] private TextMeshProUGUI m_keyText;
        private PartController m_controller;
        private string m_powerUpKey = null;
        //TODO Make a progress bar

        public float CooldownProgress { get => m_cooldownTimer.Progress; }
        public float PowerUpProgress { get => m_powerUpTimer.Progress; }
        public override bool IsTakingAction { get; set; } = false;
        public override bool IsReady { get { return !IsTakingAction && IsCooledDown; } set { } }
        public override bool IsCooledDown { get; set; } = false;
        [field: SerializeField] public override float ActionLength { get; set; } = 3f;
        [field: SerializeField] public override float CooldownTime { get; set; } = 4f;
        public override Action ActionCompleted { get; set; }
        [field: SerializeField] public override string ActionName { get; set; } = "Power Up";

        public event Action Activated;

        private CountdownTimer m_powerUpTimer;

        protected override void Awake()
        {
            base.Awake();

            m_controller = GetComponent<PartController>();

            m_powerUpTimer = new(ActionLength);

            m_powerUpTimer.OnTimerStop += OnFinished;
        }

        private void OnFinished()
        {
            IsTakingAction = false;
            ActionCompleted?.Invoke();
            StartCooldown();
        }

        protected override void Update()
        {
            if (m_powerUpKey == null)
            {
                if (m_controller is ArmController arm && arm.isInitialized)
                {
                    if (arm.side == ArmController.Side.Left)
                        m_powerUpKey = m_altPowerUpKey;
                    else
                        m_powerUpKey = m_mainPowerUpKey;
                }
                if (m_powerUpKey != null)
                    m_keyText.text = $"[{m_powerUpKey}]";
            }

            base.Update();

            bool isPlayer = m_controller != null && m_controller.GetRobot().State.isPlayer;
            m_readyUI.enabled = IsReady && isPlayer;

            if (IsReady && !IsTakingAction)
            {
                if (isPlayer && Input.GetKeyDown(m_powerUpKey))
                {
                    Activate();
                }
            }
            m_powerUpTimer.Tick(Time.deltaTime);
        }

        public override void Activate()
        {
            IsCooledDown = false;
            m_powerUpTimer.Start();
            IsTakingAction = true;
            Activated?.Invoke();
        }
    }
}