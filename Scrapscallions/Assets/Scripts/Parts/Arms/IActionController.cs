using System;

namespace Scraps.Parts
{
    public interface IActionController
    {
        public string ActionName { get; }
        public float ActionLength { get; }
        public float CooldownTime { get; }
        public bool IsReady { get; }
        public bool IsCooledDown { get; }
        public bool IsTakingAction { get; }

        public Action ActionCompleted { get; set; }

        public void Activate();
    }
}