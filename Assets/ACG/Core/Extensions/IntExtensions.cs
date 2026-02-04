using UnityEngine;

namespace ACG.Core.Extensions
{
    public static class IntExtensions
    {
        public static Vector3 ToVector3(this int value)
        {
            return new Vector3(value, value, value);
        }

        public static Vector2 ToVector2(this int value)
        {
            return new Vector2(value, value);
        }
    }
}