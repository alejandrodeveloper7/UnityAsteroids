using UnityEngine;

namespace ACG.Core.Extensions
{
    public static class FloatExtensions
    {
        public static Vector3 ToVector3(this float value)
        {
            return new Vector3(value, value, value);
        }

        public static Vector2 ToVector2(this float value)
        {
            return new Vector2(value, value);
        }
    }
}