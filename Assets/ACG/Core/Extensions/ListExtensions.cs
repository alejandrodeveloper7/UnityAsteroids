using System;
using System.Collections.Generic;
using System.Linq;

namespace ACG.Core.Extensions
{
    public static class ListExtensions
    {
        public static void Randomize<T>(this List<T> list)
        {
            Random rng = new();
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

        public static void RemoveDuplicates<T>(this List<T> list)
        {
            List<T> newList = new HashSet<T>(list).ToList();
            list.Clear();
            list.AddRange(newList);
        }

        public static T GetRandomElement<T>(this List<T> list)
        {
            if (list is null || list.Count is 0)
                throw new ArgumentException("The list is null or empty");

            return list[UnityEngine.Random.Range(0, list.Count)];
        }
    }
}
