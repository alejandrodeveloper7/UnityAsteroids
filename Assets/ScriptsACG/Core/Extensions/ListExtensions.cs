using System;
using System.Collections.Generic;
using System.Linq;

namespace ToolsACG.Core.Extensions
{
    public static class ListExtensions
    {
        #region Fields

        private static readonly Random random = new();

        #endregion

        #region Public Methods

        public static void Randomize<T>(this List<T> list)
        {
            Random rng = new Random();
            for (int i = 0; i < list.Count; i++)
            {
                int randomIndex = rng.Next(list.Count);
                T temp = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }

        public static void RemoveNulls<T>(this List<T> list) where T : class
        {
            list.RemoveAll(item => item == null);
        }

        public static List<T> RemoveDuplicates<T>(this List<T> list)
        {
            return new HashSet<T>(list).ToList();
        }

        public static T GetRandomElement<T>(this List<T> list)
        {
            if (list is null || list.Count is 0)
                throw new ArgumentException("The list is null or empty");

            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        #endregion
    }
}
