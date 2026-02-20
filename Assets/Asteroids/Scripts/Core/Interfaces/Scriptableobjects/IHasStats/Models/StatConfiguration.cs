using ACG.Core.Models;
using Asteroids.Core.Interfaces.Enums;
using System;

namespace Asteroids.Core.Interfaces.Models
{
    [Serializable]
    public sealed class StatConfiguration
    {
        public StatIdType Id;
        public string DisplayName;
        public FloatRange ValueRange;
        public bool IsReverseValue;

        public StatConfiguration(StatIdType id, string displayname, FloatRange valueRange, bool isReverseValue)
        {
            Id = id;
            DisplayName = displayname;
            ValueRange = valueRange;
            IsReverseValue = isReverseValue;
        }
    }
}