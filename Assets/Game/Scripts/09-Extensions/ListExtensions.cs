using System;
using System.Collections.Generic;
using System.Linq;

public static class ListExtensions
{
    private static System.Random rng = new System.Random();

    public static void Randomize<T>(this List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            int k = rng.Next(n--);
            (list[n], list[k]) = (list[k], list[n]);
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
        if (list == null || list.Count == 0)
            throw new ArgumentException("The list is null or empty");

        return list[UnityEngine.Random.Range(0, list.Count)];
    }

}
