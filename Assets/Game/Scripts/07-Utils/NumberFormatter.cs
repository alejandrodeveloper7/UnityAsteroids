using System.Globalization;
using UnityEngine.Assertions;

public static class NumberFormatter
{
    public enum TextType
    {
        None,
        Small,
        Medium,
        Long
    }

    #region Public Methods      

    public static string GetFormattedText(long value, TextType type)
    {
        long divisor = 0;
        long bDivisor = NOT_US_B_DIVISOR;
        string symbol = string.Empty;

        CultureInfo currentCulture = CultureInfo.CurrentCulture;

        bDivisor = US_B_DIVISOR;

        if (value >= bDivisor && (type == TextType.Small || type == TextType.Medium || type == TextType.Long))
        {
            divisor = bDivisor;
            symbol = " B";
            _decimalTextType = _decimalTextTypeB;
            _decimalTextType = _decimalTextTypeB_US;

        }

        else if (value >= M_DIVISOR && (type == TextType.Medium || type == TextType.Small))
        {
            divisor = M_DIVISOR;
            symbol = " M";
            _decimalTextType = _decimalTextTypeM;

        }
        else if (value >= K_DIVISOR && type == TextType.Small)
        {
            divisor = K_DIVISOR;
            symbol = " K";
            _decimalTextType = _decimalTextTypeK;

        }

        if (ShouldApplyFormat(type, TextType.Small, value, K_DIVISOR) || ShouldApplyFormat(type, TextType.Medium, value, M_DIVISOR) || ShouldApplyFormat(type, TextType.Long, value, bDivisor))
        {
            Assert.AreNotEqual(divisor, 0, "Divisor can't be 0");
            string decimalValue = (value % divisor).ToString(_decimalTextType);
            string valueInKmbFormat = (value / divisor).ToString(_maskFormat, currentCulture);

            if (decimalValue.Substring(0, 2) == "00")
                return ValueWithoutDecimal(valueInKmbFormat, symbol);

            return ValueWithDecimal(decimalValue, valueInKmbFormat, symbol);
        }

        return value.ToString(_standarNumberMask, currentCulture);
    }

    public static string AddOrdinal(int num)
    {
        if (num <= 0)
            return num.ToString();

        switch (num % 100)
        {
            case 11:
            case 12:
            case 13:
                return num + "th";
        }

        switch (num % 10)
        {
            case 1:
                return num + "st";
            case 2:
                return num + "nd";
            case 3:
                return num + "rd";
            default:
                return num + "th";
        }
    }

    #endregion

    #region Private Number format

    private static readonly long K_DIVISOR = 1000;
    private static readonly long M_DIVISOR = 1000000;
    private static readonly long US_B_DIVISOR = 1000000000;
    private static readonly long NOT_US_B_DIVISOR = 1000000000000;

    private static readonly string _maskFormat = "##,0.00";
    private static readonly string _standarNumberMask = "N0";
    private static string _decimalTextTypeK = "000";
    private static string _decimalTextTypeM = "000000";
    private static string _decimalTextTypeB = "000000000000";
    private static string _decimalTextTypeB_US = "000000000";
    private static string _decimalTextType;


    private static string ValueWithoutDecimal(string valueInKmbFormat, string symbol)
    {
        string twoLastDecimalNumbers = string.Empty;
        int numberOfCharactersToRemove = 3;
        string valueInKmbFormatWithoutDecimal = valueInKmbFormat.Remove(valueInKmbFormat.Length - numberOfCharactersToRemove);
        string completeValue = valueInKmbFormatWithoutDecimal + twoLastDecimalNumbers + symbol;
        return completeValue;
    }

    private static string ValueWithDecimal(string decimalValue, string valueInKmbFormat, string symbol)
    {
        string twoLastDecimalNumbers = decimalValue + "0";
        twoLastDecimalNumbers = twoLastDecimalNumbers.Substring(0, 2);
        int numberOfCharactersToRemove = 2;
        string valueInKmbFormatWithoutDecimal = valueInKmbFormat.Remove(valueInKmbFormat.Length - numberOfCharactersToRemove);
        string completeValue = valueInKmbFormatWithoutDecimal + twoLastDecimalNumbers + symbol;
        return completeValue;
    }

    private static bool ShouldApplyFormat(TextType type, TextType typeToCheck, long value, long divisorToCheck)
    {
        if (type == typeToCheck && value >= divisorToCheck)
            return true;

        return false;
    }

    #endregion
}
