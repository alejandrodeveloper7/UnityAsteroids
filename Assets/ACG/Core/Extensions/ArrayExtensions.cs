using System;
using System.Collections.Generic;
using System.Linq;

namespace ACG.Core.Extensions
{
    public static class ArrayExtensions
    {
        public static void Randomize<T>(this T[] array)
        {
            if (array == null || array.Length <= 1)
                return;

            System.Random rng = new();
            for (int i = 0; i < array.Length; i++)
            {
                int randomIndex = rng.Next(array.Length);
                (array[i], array[randomIndex]) = (array[randomIndex], array[i]);
            }
        }

        public static T[] RemoveNulls<T>(this T[] array) where T : class
        {
            if (array == null)
                return Array.Empty<T>();

            return array.Where(item => item != null).ToArray();
        }

        public static T[] RemoveDuplicates<T>(this T[] array)
        {
            if (array == null)
                return Array.Empty<T>();

            return new HashSet<T>(array).ToArray();
        }

        public static T GetRandomElement<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
                throw new ArgumentException("The array is null or empty");

            return array[UnityEngine.Random.Range(0, array.Length)];
        }
    }
}