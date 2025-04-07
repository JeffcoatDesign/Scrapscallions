using System;
using System.Collections;
using System.Collections.Generic;
using Scraps.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Scraps.Parts
{
    public class PowerUpController : ActionController
    {
        [SerializeField] private string m_mainPowerUpKey;
        [SerializeField] private string m_altPowerUpKey;
        public PartController partController;
        public string powerUpKey = null;
        //TODO Make a progress bar

        public float CooldownProgress { get => m_cooldownTimer.Progress; }
        public float PowerUpProgress { get => m_powerUpTimer.Progress; }
        public override bool IsTakingAction { get; set; } = false;
        public override bool IsReady { get { return !IsTakingAction && IsCooledDown; } set { } }
        public override bool IsCooledDown { get; set; } = false;
        public override bool IsInitialized { get; set; } = false;
        [field: SerializeField] public override float ActionLength { get; set; } = 3f;
        [field: SerializeField] public override float CooldownTime { get; set; } = 4f;
        public override Action ActionCompleted { get; set; }
        [field: SerializeField] public override string ActionName { get; set; } = "Power Up";

        public event Action Activated;

        private CountdownTimer m_powerUpTimer;

        private void OnDestroy()
        {
            if (partController == null || partController.GetRobot() == null) return;

            if (partController.GetRobot().State.isPlayer)
            {
                BattleUI.Instance.playerPowerUps.RemovePowerUp(this);
            }
            else
            {
                BattleUI.Instance.opponentPowerUps.RemovePowerUp(this);
            }
        }

        protected override void Awake()
        {
            base.Awake();

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
            base.Update();

            if (!IsInitialized) return;

            bool isPlayer = partController != null && partController.GetRobot().State.isPlayer;

            if (IsReady && !IsTakingAction)
            {
                if (isPlayer && Input.GetKeyDown(powerUpKey))
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

        public override void Initialize(PartController part)
        {
            partController = part;

            if (partController is ArmController arm)
            {
                if (arm.side == ArmController.Side.Left)
                    powerUpKey = m_mainPowerUpKey;
                else
                    powerUpKey = m_altPowerUpKey;
            }
            else
                powerUpKey = m_mainPowerUpKey;

            if (BattleUI.Instance == null) return;
            if (partController.GetRobot().State.isPlayer)
            {
                BattleUI.Instance.playerPowerUps.AddPowerUp(this);
            }
            else
            {
                BattleUI.Instance.opponentPowerUps.AddPowerUp(this);
            }

            IsInitialized = true;
        }
    }
}