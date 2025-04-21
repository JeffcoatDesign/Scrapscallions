using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scraps.Utilities
{
    public static class MathUtility
    {
        // From Oneiros on the Unity Forum
        // https://discussions.unity.com/t/normal-distribution-random/66530/4
        public static float NormalizedRandom(float minValue = 0.0f, float maxValue = 1.0f)
        {
            float u, v, S;

            do
            {
                u = 2.0f * UnityEngine.Random.value - 1.0f;
                v = 2.0f * UnityEngine.Random.value - 1.0f;
                S = u * u + v * v;
            }
            while (S >= 1.0f);

            float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / 2);

            float mean = (minValue + maxValue) / 2.0f;
            float sigma = (maxValue - mean) / 3.0f;
            return Mathf.Clamp(std * sigma + mean, minValue, maxValue);
        }
    }
}