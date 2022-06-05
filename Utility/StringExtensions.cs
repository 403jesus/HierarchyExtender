using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ReSharper disable CheckNamespace

namespace HierarchyExtender.Utility
{
    public static class StringExtensions
    {
        public static string NumbersOnly(this string str)
        {
            return new string(str.Where(char.IsDigit).ToArray());
        }

        public static string CharactersOnly(this string str)
        {
            throw new NotImplementedException();
        }
        
        public static string LimitLength(this string str, int len)
        {
            if (str.Length != 0)
                str = str.Substring(0,Mathf.Min(str.Length, len));

            string result = "";
            result = str.Substring(0,Mathf.Min(str.Length, len));
            
            return result;
        }

        public static string ToCamel(this string str)
        {
            if (str.Length == 0)
                return str;
            return str[..1].ToLower() + str[1..];
        }
        
        public static string ToPascal(this string str)
        {
            if (str.Length == 0)
                return str;
            return str[..1].ToUpper() + str[1..];
        }
    }
}
