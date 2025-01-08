using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.AI
{
    [System.Serializable]
    public class Goal
    {
        public string name;
        public float value;
        public float rateOfChange;

        public Goal (string name, float value, float rateOfChange)
        {
            this.name = name;
            this.value = value;
            this.rateOfChange = rateOfChange;
        }

        internal float GetDiscontentment(float newValue)
        {
            // Testing if newValue * value is better than newValue squared
            return newValue * newValue;
        }
    }
}