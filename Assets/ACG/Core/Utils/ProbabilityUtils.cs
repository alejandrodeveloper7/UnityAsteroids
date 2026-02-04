using System;
using UnityEngine;

namespace ACG.Core.Utils
{
    public class ProbabilityUtils : MonoBehaviour
    {
        #region Percentage

        public static bool TryByPercentage(int percentage)
        {
            int value = UnityEngine.Random.Range(0, 100);
            return value < percentage;
        }

        public static bool TryByPercentage(float percentage)
        {
            float value = UnityEngine.Random.Range(0, 100);
            return value < percentage;
        }

        public static void DoWithPercentage(int percentage, Action action)
        {
            if (TryByPercentage(percentage))
                action?.Invoke();
        }

        public static void DoWithPercentage(float percentage, Action action)
        {
            if (TryByPercentage(percentage))
                action?.Invoke();
        }

        #endregion

        #region Odds

        public static bool TryByOdds(int odds)
        {
            int value = UnityEngine.Random.Range(0, odds);
            return value is 0;
        }

        public static void DoWithOdds(int odds, Action action)
        {
            if (TryByOdds(odds))
                action?.Invoke();
        }
        
        #endregion

    }
}