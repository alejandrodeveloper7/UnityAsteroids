using Asteroids.Core.Interfaces.Enums;
using System;

namespace Asteroids.Core.Interfaces.Models
{
    [Serializable]
    public sealed class StatConfiguration
    {
        public StatIdType Id;
        public string Name;
        public float MinValue;
        public float MaxValue;
        public bool IsReverseValue;

        public StatConfiguration(StatIdType id, string name, float minValue, float maxValue, bool isReverseValue)
        {
            Id = id;
            Name = name;
            MinValue = minValue;
            MaxValue = maxValue;
            IsReverseValue = isReverseValue;
        }
    }
}