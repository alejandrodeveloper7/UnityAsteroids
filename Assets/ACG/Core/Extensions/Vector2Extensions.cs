using UnityEngine;

namespace ACG.Core.Extensions
{
    public static class Vector2Extensions
    {
        #region RandomRange

        public static Vector2 RandomRange(this Vector2 min, Vector2 max)
        {
            return new Vector2(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y)
            );
        }

        public static Vector2 RandomRange(this Vector2 limits)
        {
            return new Vector2(
                Random.Range(-limits.x, limits.x),
                Random.Range(-limits.y, limits.y)
            );
        }

        #endregion

        #region With

        public static Vector2 WithX(this Vector2 v, float x)
        {
            return new(x, v.y);
        }

        public static Vector2 WithY(this Vector2 v, float y)
        {
            return new(v.x, y);
        }

        #endregion

        #region Add

        public static Vector2 AddX(this Vector2 v, float x)
        {
            return v + new Vector2(x, 0);
        }

        public static Vector2 AddY(this Vector2 v, float y)
        {
            return v + new Vector2(0, y);
        }

        #endregion

        #region Only

        public static Vector2 OnlyX(this Vector2 v)
        {
            return new(v.x, 0);
        }

        public static Vector2 OnlyY(this Vector2 v)
        {
            return new(0, v.y);
        }

        #endregion
    }
}