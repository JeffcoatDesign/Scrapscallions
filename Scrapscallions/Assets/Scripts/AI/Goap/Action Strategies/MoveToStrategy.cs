using System;
using UnityEngine;

namespace Scraps.AI.GOAP
{
    [CreateAssetMenu(fileName = "New Move To Strategy", menuName = "GOAP/Action Strategies/Move To Strategy")]
    public class MoveToStrategy : ScriptableObject, IActionStrategy
    {
        [SerializeField] Robot robot;
        [SerializeField] Func<Vector3> destination;
        public bool CanPerform => !IsComplete;
        public bool IsComplete => robot.RemainingDistance <= 2f;//&& !agent.pathPending;

        public MoveToStrategy Initialize(Robot robot, Func<Vector3> destination)
        {
            this.robot = robot;
            this.destination = destination;
            return this;
        }

        public void Begin() => robot.SetDestination(destination());
        public void Stop() => robot.ResetPath();
    }
}