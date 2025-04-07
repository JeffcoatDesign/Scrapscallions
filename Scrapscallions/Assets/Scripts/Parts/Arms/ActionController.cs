using System;
using System.Collections;
using System.Collections.Generic;
using Scraps.UI;
using UnityEngine;

namespace Scraps.Parts
{
    abstract public class ActionController : MonoBehaviour, IActionController
    {
        public abstract bool IsTakingAction { get; set; }
        public abstract bool IsReady { get; set; }
        public abstract bool IsInitialized { get; set; }
        public abstract bool IsCooledDown { get; set; }
        public abstract float ActionLength { get; set; }
        public abstract float CooldownTime { get; set; }
        public abstract Action ActionCompleted { get; set; }
        public abstract string ActionName { get; set; }
        protected CountdownTimer m_cooldownTimer;
        public event Action CooledDown;

        protected virtual void Awake()
        {
            m_cooldownTimer = new(CooldownTime);

            m_cooldownTimer.OnTimerStop += () =>
            {
                IsReady = true;
                IsCooledDown = true;
                CooledDown?.Invoke();
            };

            if(!IsReady)
                m_cooldownTimer.Start();
        }

        abstract public void Activate();

        protected virtual void Update()
        {
            if(!IsInitialized) return;

            m_cooldownTimer.Tick(Time.deltaTime);
        }

        protected void StartCooldown()
        {
            m_cooldownTimer.Start();
        }

        public virtual void Initialize(PartController part)
        {
            IsInitialized = true;
        }
    }
}