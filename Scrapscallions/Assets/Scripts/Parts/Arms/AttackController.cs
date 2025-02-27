using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    abstract public class AttackController : MonoBehaviour, IActionController
    {
        public abstract bool IsTakingAction { get; set; }
        public abstract bool IsReady { get; set; }
        public abstract bool IsCooledDown { get; set; }
        public abstract float ActionLength { get; set; }
        public abstract float CooldownTime { get; set; }
        public abstract Action ActionCompleted { get; set; }
        public abstract string ActionName { get; set; }

        private CountdownTimer m_cooldownTimer;

        private void Awake()
        {
            m_cooldownTimer = new(CooldownTime);

            m_cooldownTimer.OnTimerStop += () =>
            {
                IsReady = true;
            };

            m_cooldownTimer.Start();
        }

        abstract public void Activate();

        private void Update()
        {
            m_cooldownTimer.Tick(Time.deltaTime);
        }

        protected void StartCooldown()
        {
            m_cooldownTimer.Start();
        }
    }
}