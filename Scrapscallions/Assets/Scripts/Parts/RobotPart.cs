using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    public abstract class RobotPart : ScriptableObject
    {
        public abstract string PartName { get; set; }
        public abstract float MaxHP { get; set; }
        public abstract float CurrentHP { get; set; }
        public abstract int Price { get; set; }
        protected abstract GameObject Prefab { get; set; }
        public abstract Sprite Sprite { get; set; }

    }
}