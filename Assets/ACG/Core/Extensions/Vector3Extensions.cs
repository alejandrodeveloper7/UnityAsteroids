using UnityEngine;

namespace ACG.Core.Extensions
{
    public static class Vector3Extensions
    {
        #region RandomRange

        public static Vector3 RandomRange(this Vector3 min, Vector3 max)
        {
            return new Vector3(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y),
                Random.Range(min.z, max.z)
            );
        }

        public static Vector3 RandomRange(this Vector3 limits)
        {
            return new Vector3(
                Random.Range(-limits.x, limits.x),
                Random.Range(-limits.y, limits.y),
                Random.Range(-limits.z, limits.z)
            );
        }

        #endregion

        #region With

        public static Vector3 WithX(this Vector3 v, float x)
        {
            return new(x, v.y, v.z);
        }

        public static Vector3 WithY(this Vector3 v, float y)
        {
            return new(v.x, y, v.z);
        }

        public static Vector3 WithZ(this Vector3 v, float z)
        {
            return new(v.x, v.y, z);
        }

        #endregion

        #region Add

        public static Vector3 AddX(this Vector3 v, float x)
        {
            return v + new Vector3(x, 0, 0);
        }

        public static Vector3 AddY(this Vector3 v, float y)
        {
            return v + new Vector3(0, y, 0);
        }

        public static Vector3 AddZ(this Vector3 v, float z)
        {
            return v + new Vector3(0, 0, z);
        }

        #endregion

        #region Flatten

        public static Vector3 FlattenX(this Vector3 v)
        {
            return new(0f, v.y, v.z);
        }

        public static Vector3 FlattenY(this Vector3 v)
        {
            return new(v.x, 0f, v.z);
        }

        public static Vector3 FlattenZ(this Vector3 v)
        {
            return new(v.x, v.y, 0f);
        }

        #endregion

        #region Only

        public static Vector3 OnlyX(this Vector3 v)
        {
            return new(v.x, 0, 0);
        }

        public static Vector3 OnlyY(this Vector3 v)
        {
            return new(0, v.y, 0);
        }

        public static Vector3 OnlyZ(this Vector3 v)
        {
            return new(0, 0, v.z);
        }

        #endregion
    }
}