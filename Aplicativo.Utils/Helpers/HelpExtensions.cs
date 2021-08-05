using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Aplicativo.Utils.Helpers
{
    public static class HelpExtensions
    {


        public static string Juntar(this string value, string texto, string separador = "-")
        {
            if (string.IsNullOrEmpty(texto))
                return value;
            return string.Format("{0}{1} {2}", value.Trim(), separador, texto.Trim());
        }

        public static string ToStringOrNull(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            else
                return value.ToString().Trim();
        }

        public static string ToStringOrNull(this object value)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
                return null;
            else
                return value.ToString().Trim();
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

        public static string StringFormat(this object valor, string mascara)
        {
            return StringFormat(valor.ToString(), mascara);
        }

        public static string StringFormat(this string valor, string mascara)
        {

            if (string.IsNullOrEmpty(valor))
            {
                return null;
            }

            string novoValor = string.Empty;

            int posicao = 0;

            for (int i = 0; mascara.Length > i; i++)
            {

                if (mascara[i] == '#')
                {
                    if (valor.Length > posicao)
                    {
                        novoValor = novoValor + valor[posicao];
                        posicao++;
                    }
                    else
                        break;
                }
                else
                {
                    if (valor.Length > posicao)
                        novoValor = novoValor + mascara[i];
                    else
                        break;
                }
            }

            return novoValor;

        }

        public static T Clone<T>(this T obj) where T : class
        {
            if (obj == null) return null;
            System.Reflection.MethodInfo inst = obj.GetType().GetMethod("MemberwiseClone",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (inst != null)
                return (T)inst.Invoke(obj, null);
            else
                return null;
        }

        public static T GetCode<T>(string value)
        {
            foreach (object o in System.Enum.GetValues(typeof(T)))
            {
                T enumValue = (T)o;
                if (GetXmlAttrNameFromEnumValue<T>(enumValue).Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    return (T)o;
                }
            }

            throw new ArgumentException("No XmlEnumAttribute code exists for type " + typeof(T).ToString() + " corresponding to value of " + value);
        }

        public static string GetXmlAttrNameFromEnumValue<T>(T pEnumVal)
        {
            Type type = pEnumVal.GetType();
            FieldInfo info = type.GetField(Enum.GetName(typeof(T), pEnumVal));
            XmlEnumAttribute att = (XmlEnumAttribute)info.GetCustomAttributes(typeof(XmlEnumAttribute), false)[0];
            //If there is an xmlattribute defined, return the name

            return att.Name;
        }

    }
}
