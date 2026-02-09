using System;
using UnityEngine;

namespace Asteroids.Core.Services
{
    public static class TimeScaleService
    {
        #region Events and Properties

        public static event Action<float> OnTimeScaleChanged;

        public static float Current => Time.timeScale;
        public static bool IsPaused => Time.timeScale is 0;

        #endregion

        #region Management

        public static void Pause()
        {
            Set(0f);
        }

        public static void Resume()
        {
            Set(1f);
        }

        public static void SetValue(float value)
        {
            Set(value);
        }

        #endregion

        #region private Methods

        private static void Set(float value)
        {
            if (value < 0f)
                return;

            if (value != Time.timeScale)
            {
                Time.timeScale = value;
                OnTimeScaleChanged?.Invoke(value);
            }
        }

        #endregion
    }
}