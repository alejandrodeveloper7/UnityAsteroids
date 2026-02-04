using Asteroids.Core.Interfaces.Enums;
using System;

namespace Asteroids.Core.Interfaces.Models
{
    [Serializable]
    public sealed class StatData
    {
        public StatIdType Id;
        public string PropertyName;

        public StatData(StatIdType id, string propertyName, bool isReverseValue)
        {
            Id = id;
            PropertyName = propertyName;
        }
    }
}