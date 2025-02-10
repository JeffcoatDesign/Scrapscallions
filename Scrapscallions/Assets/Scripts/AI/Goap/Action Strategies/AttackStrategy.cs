using Scraps.Parts;
using UnityEngine;

namespace Scraps.AI.GOAP
{
    [CreateAssetMenu(fileName = "New Attack Strategy", menuName = "GOAP/Action Strategies/Attack Strategy")]
    public class AttackStrategy : ScriptableObject, IActionStrategy
    {
        [SerializeField] float attackTime = 2;
        private CountdownTimer timer;
        private ArmController arm;

        public bool CanPerform => true;
        public bool IsComplete { get; private set; }

        public AttackStrategy Initialize(ArmController arm, float attackTime = -1f)
        {
            this.arm = arm;

            if (attackTime >= 0)
                this.attackTime = attackTime;

            timer = new CountdownTimer(this.attackTime);
            timer.OnTimerStart += () => IsComplete = false;
            timer.OnTimerStop += () => IsComplete = true;
            return this;
        }

        public void Begin()
        {
            timer.Time = attackTime;
            timer.Start();
            arm.Attack();
        }

        public void Stop() {
            Debug.Log("Stopped Attack");
            arm.Idle();
        }

        public void Tick(float deltaTime) => timer.Tick(deltaTime);
    }
}