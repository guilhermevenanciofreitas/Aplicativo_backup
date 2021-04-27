using System;

namespace Aplicativo.Utils.Helpers
{
    public static class HelpExtensions
    {


        public static string Juntar(this string value, string texto, char separador = '-')
        {
            if (string.IsNullOrEmpty(texto))
                return value;
            return string.Format("{0} {1} {2}", value.Trim(), separador, texto.Trim());
        }

        public static string ToStringOrNull(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            else
                return value.ToString();
        }

        public static string ToStringOrNull(this object value)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
                return null;
            else
                return value.ToString();
        }

        public static bool ToBoolean(this bool? value)
        {
            return value ?? false;
        }

        public static bool ToBoolean(this object value)
        {
            return (bool)TryParse<bool>(value.ToString(), bool.TryParse);
        }



        public static long ToLong(this long? value)
        {
            return (long)value;
        }

        public static long ToLong(this string value)
        {
            return long.Parse(value);
        }

        public static long? ToLongOrNull(this string value)
        {
            return TryParse<long>(value, long.TryParse);
        }

        public static long ToLong(this string value, long returnIfNull)
        {
            if (long.TryParse(value, out long output))
                return output;
            else
                return returnIfNull;
        }

        public static int ToInt(this string value)
        {
            return int.Parse(value);
        }

        public static int? ToIntOrNull(this string value)
        {
            return TryParse<int>(value, int.TryParse);
        }

        public static int? ToIntOrNull(this object value)
        {
            return ToIntOrNull(value.ToStringOrNull());
        }

        public static int ToInt(this string value, int returnIfNull)
        {
            if (int.TryParse(value, out int output))
                return output;
            else
                return returnIfNull;
        }

        public static decimal Round(this decimal value, int decimals = 2)
        {
            return Math.Round(value, decimals);
        }

        public static decimal RoundNull(this decimal? value, int decimals = 2)
        {
            return Math.Round(value ?? 0, decimals);
        }

        public static decimal ToDecimal(this string value)
        {
            return decimal.Parse(value);
        }

        public static decimal? ToDecimalOrNull(this string value)
        {
            return TryParse<decimal>(value, decimal.TryParse);
        }

        public static decimal ToDecimal(this string value, int returnIfNull)
        {
            if (decimal.TryParse(value, out decimal output))
                return output;
            else
                return returnIfNull;
        }

        public static short ToShort(this string value)
        {
            return short.Parse(value);
        }

        public static short? ToShortOrNull(this string value)
        {
            return TryParse<short>(value, short.TryParse);
        }

        public static int ToShort(this string value, short returnIfNull)
        {
            if (short.TryParse(value, out short output))
                return output;
            else
                return returnIfNull;
        }

        public static byte ToByte(this string value)
        {
            return byte.Parse(value);
        }

        public static byte? ToByteOrNull(this string value)
        {
            return TryParse<byte>(value, byte.TryParse);
        }

        public static byte ToByte(this string value, byte returnIfNull)
        {
            if (byte.TryParse(value, out byte output))
                return output;
            else
                return returnIfNull;
        }

        public static DateTime ToDate(this string value)
        {
            return DateTime.Parse(value);
        }

        public static DateTime? ToDateOrNull(this string value)
        {
            return TryParse<DateTime>(value, DateTime.TryParse);
        }

        public static DateTime ToDate(this string value, DateTime returnIfNull)
        {
            if (DateTime.TryParse(value, out DateTime output))
                return output;
            else
                return returnIfNull;
        }


        public delegate bool TryParseHandler<T>(string value, out T result);

        public static T? TryParse<T>(string value, TryParseHandler<T> handler) where T : struct
        {
            if (string.IsNullOrEmpty(value))
                return null; 
            if (handler(value, out T result))
                return result;

            return null;

        }


    }
}
