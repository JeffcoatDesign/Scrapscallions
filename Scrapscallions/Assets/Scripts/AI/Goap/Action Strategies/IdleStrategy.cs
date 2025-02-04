using UnityEngine;

namespace Scraps.AI.GOAP
{
    [CreateAssetMenu(fileName = "New Idle Strategy", menuName = "GOAP/Action Strategies/Idle Strategy")]
    public class IdleStrategy : ScriptableObject, IActionStrategy
    {
        private CountdownTimer timer;
        public bool CanPerform => true;

        public bool IsComplete { get; private set; }


        public IdleStrategy Initialize (float duration)
        {
            timer = new CountdownTimer(duration);
            timer.OnTimerStart += () => IsComplete = false;
            timer.OnTimerStop += () => IsComplete = true;
            return this;
        }

        public void Begin() => timer.Start();
        public void Tick(float deltaTime) => timer.Tick(deltaTime);
    }
}