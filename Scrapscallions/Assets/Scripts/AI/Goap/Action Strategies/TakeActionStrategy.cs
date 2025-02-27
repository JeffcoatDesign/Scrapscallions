using Scraps.Parts;
using System;
using UnityEngine;

namespace Scraps.AI.GOAP
{
    [CreateAssetMenu(fileName = "Take Action Strategy", menuName = "GOAP/Action Strategies/Take Action Strategy")]
    public class TakeActionStrategy : ScriptableObject, IActionStrategy
    {
        IActionController m_controller;
        public bool CanPerform => true;
        public bool IsComplete { get; private set; } = false;

        public TakeActionStrategy Initialize(IActionController actionController)
        {
            m_controller = actionController;
            m_controller.ActionCompleted += () => { IsComplete = true; };
            return this;
        }

        public void Begin()
        {
            m_controller.Activate();
        }

        public void Stop()
        {
            //NOOP
        }

        public void Tick(float deltaTime)
        {
            //NOOP
        }
    }
}