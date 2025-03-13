using Scraps.Parts;
using System;
using UnityEngine;

namespace Scraps.AI.GOAP
{
    //[CreateAssetMenu(fileName = "New Power Up Strategy", menuName = "GOAP/Action Strategies/Power Up Strategy")]
    public class PowerUpStrategy : ScriptableObject, IActionStrategy
    {
        private PowerUpController powerUpController;

        public bool CanPerform => powerUpController != null ? powerUpController.IsReady : false;
        public bool IsComplete { get; private set; } = false;

        public PowerUpStrategy Initialize(PowerUpController powerUpController)
        {
            this.powerUpController = powerUpController;
            IsComplete = false;

            powerUpController.ActionCompleted += OnPowerUpFinished;
            return this;
        }

        private void OnPowerUpFinished()
        {
            IsComplete = true;
        }

        public void Begin()
        {
            powerUpController.Activate();
        }

        public void Stop() {

        }

        public void Tick(float deltaTime) {
            //NOOP
        }
    }
}