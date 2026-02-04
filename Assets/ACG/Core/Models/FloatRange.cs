using UnityEngine;

namespace ACG.Core.Models
{
    [System.Serializable]
    public struct FloatRange
    {
        public float Min;
        public float Max;

        public FloatRange(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public readonly float GetRandom()
        {
            return Random.Range(Min, Max);
        }
    }
}