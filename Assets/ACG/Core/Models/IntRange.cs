using UnityEngine;

namespace ACG.Core.Models
{
    [System.Serializable]
    public struct IntRange
    {
        public int Min;
        public int Max;

        public IntRange(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public readonly int GetRandom()
        {
            return Random.Range(Min, Max);
        }
    }
}