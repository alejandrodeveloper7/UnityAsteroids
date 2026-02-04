using System;
using UnityEngine;

namespace ACG.Core.Utils
{
    public static class ColorUtils
    {
        #region Public methods

        public static Color GetColorWithAlpha(Color color, float alphaValue)
        {
            return new Color(color.r, color.g, color.b, alphaValue);
        }

        public static Color GetColorFromHexadecimal(string hexadecimal)
        {
            float red = HexToFloatNormalized(hexadecimal.Substring(0, 2));
            float green = HexToFloatNormalized(hexadecimal.Substring(2, 2));
            float blue = HexToFloatNormalized(hexadecimal.Substring(4, 2));
            return new Color(red, green, blue);
        }

        public static string GetHexadecimalFromColor(Color color)
        {
            string red = FloatNormalizedToHex(color.r);
            string green = FloatNormalizedToHex(color.g);
            string blue = FloatNormalizedToHex(color.b);
            return red + green + blue;
        }

        #endregion

        #region Private methods

        private static int HexToDec(string hex)
        {
            return Convert.ToInt32(hex, 16);
        }
        private static string DecToHex(int dec)
        {
            return dec.ToString("X2");
        }
        private static String FloatNormalizedToHex(float value)
        {
            return DecToHex(Mathf.RoundToInt(value * 255f));
        }

        private static float HexToFloatNormalized(string hex)
        {
            return HexToDec(hex) / 255f;
        }

        #endregion
    }
}