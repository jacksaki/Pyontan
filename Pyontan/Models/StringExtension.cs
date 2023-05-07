using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Models
{
    public static class StringExtension
    {
        public static string ToPascalCase(this string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                return "";
            }
            var _toUpper = false;
            var sb = new System.Text.StringBuilder();
            for(var i=0;i<value.Length;i++)
            {
                if (i == 0)
                {
                    sb.Append(value[i].ToString().ToUpper());
                    continue;
                }else if(value[i] == '_')
                {
                    _toUpper = true;
                    continue;
                }
                sb.Append(_toUpper ? value[i].ToString().ToUpper() : value[i].ToString().ToLower());
                _toUpper = false;
            }
            return sb.ToString();
        }
        public static string ToCSharpTypeName(this Type t)
        {
            if (t == null)
            {
                return null;
            }
            if (t == typeof(string))
            {
                return "string";
            }
            else if (t == typeof(int))
            {
                return "int" + (t.IsNullable() ? "?" : "");
            }
            else if (t == typeof(int?))
            {
                return "int?";
            }
            else if (t == typeof(float))
            {
                return "float" + (t.IsNullable() ? "?" : "");
            }
            else if (t == typeof(float?))
            {
                return "float?";
            }
            else if (t == typeof(double))
            {
                return "double" + (t.IsNullable() ? "?" : "");
            }
            else if (t == typeof(double?))
            {
                return "double?";
            }
            else if (t == typeof(decimal))
            {
                return "decimal" + (t.IsNullable() ? "?" : "");
            }
            else if (t == typeof(decimal?))
            {
                return "decimal?";
            }
            else if (t == typeof(DateTime))
            {
                return "DateTime" + (t.IsNullable() ? "?" : "");
            }
            else if (t == typeof(DateTime?))
            {
                return "DateTime?";
            }
            else if (t == typeof(byte))
            {
                return "byte" + (t.IsNullable() ? "?" : "");
            }
            else if (t == typeof(byte?))
            {
                return "byte?";
            }
            else if (t == typeof(byte[]))
            {
                return "byte[]" + (t.IsNullable() ? "?" : "");
            }
            else if (t == typeof(byte?))
            {
                return "byte[]?";
            }
            throw new NotImplementedException($"{t.Name} is not implemented.");
        }
        public static bool IsNullable(this Type t)
        {
            // t.IsValueType 
            return t == null || Nullable.GetUnderlyingType(t) != null;
        }
    }
}
