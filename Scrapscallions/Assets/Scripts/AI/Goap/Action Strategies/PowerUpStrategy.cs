using Scraps.Parts;
using UnityEngine;

namespace Scraps.AI.GOAP
{
    //[CreateAssetMenu(fileName = "New Power Up Strategy", menuName = "GOAP/Action Strategies/Power Up Strategy")]
    public class PowerUpStrategy : ScriptableObject, IActionStrategy
    {
        [SerializeField] float powerUpTime = 2;
        private PowerUpController powerUpController;
        private CountdownTimer timer;

        public bool CanPerform => true;
        public bool IsComplete { get; private set; }

        public PowerUpStrategy Initialize(PowerUpController powerUpController)
        {
            this.powerUpController = powerUpController;

            powerUpTime = powerUpController.PowerTime;

            timer = new CountdownTimer(this.powerUpTime);
            timer.OnTimerStart += () => IsComplete = false;
            timer.OnTimerStop += () =>
            { 
                IsComplete = true;
            };
            return this;
        }

        public void Begin()
        {
            timer.Time = powerUpTime;
            timer.Start();
            powerUpController.ActivatePowerUp();
        }

        public void Stop() {

        }

        public void Tick(float deltaTime) => timer.Tick(deltaTime);
    }
}