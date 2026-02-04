using UnityEngine;

namespace ACG.Core.Extensions
{
    public static class QuaternionExtensions
    {
        #region Random

        public static Quaternion Random()
        {
            return Quaternion.Euler(
                UnityEngine.Random.Range(0f, 360f),
                UnityEngine.Random.Range(0f, 360f),
                UnityEngine.Random.Range(0f, 360f)
            );
        }

        public static Quaternion RandomX()
        {
            float x = UnityEngine.Random.Range(0f, 360f);
            return Quaternion.Euler(x, 0f, 0f);
        }

        public static Quaternion RandomY()
        {
            float y = UnityEngine.Random.Range(0f, 360f);
            return Quaternion.Euler(0f, y, 0f);
        }

        public static Quaternion RandomZ()
        {
            float z = UnityEngine.Random.Range(0f, 360f);
            return Quaternion.Euler(0f, 0f, z);
        }

        #endregion

        #region Flatten

        public static Quaternion FlattenX(this Quaternion q)
        {
            Vector3 euler = q.eulerAngles;
            euler.x = 0f;
            return Quaternion.Euler(euler);
        }

        public static Quaternion FlattenY(this Quaternion q)
        {
            Vector3 euler = q.eulerAngles;
            euler.y = 0f;
            return Quaternion.Euler(euler);
        }

        public static Quaternion FlattenZ(this Quaternion q)
        {
            Vector3 euler = q.eulerAngles;
            euler.z = 0f;
            return Quaternion.Euler(euler);
        }

        #endregion

        #region Only

        public static Quaternion OnlyY(this Quaternion q)
        {
            Vector3 euler = q.eulerAngles;
            return Quaternion.Euler(0f, euler.y, 0f);
        }

        public static Quaternion OnlyX(this Quaternion q)
        {
            Vector3 euler = q.eulerAngles;
            return Quaternion.Euler(euler.x, 0f, 0f);
        }

        public static Quaternion OnlyZ(this Quaternion q)
        {
            Vector3 euler = q.eulerAngles;
            return Quaternion.Euler(0f, 0f, euler.z);
        }

        #endregion
    }
}