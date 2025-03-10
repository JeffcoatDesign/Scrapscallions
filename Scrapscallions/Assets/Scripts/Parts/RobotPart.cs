using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Parts
{
    public abstract class RobotPart : ScriptableObject
    {
        public abstract string PartName { get; set; }
        public abstract int MaxHP { get; set; }
        public abstract int CurrentHP { get; set; }
        public abstract int Price { get; set; }
        public abstract bool IsBroken { get; set; }
        protected abstract GameObject Prefab { get; set; }
        public abstract int ItemID { get; set; }
        public abstract Sprite Sprite { get; set; }
        public Action Break;

    }
}