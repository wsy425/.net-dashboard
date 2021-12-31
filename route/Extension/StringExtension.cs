using System;
using System.Collections.Generic;

namespace route.Extension
{
    public static class StringExtension
    {
        public static string Left(this string str, int len)
        {
            if (str.IsNullOrEmpty())
            {
                throw new Exception("字符段不能为空！");
            }
            if (str.Length < len)
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            return str.Substring(0, len);
        }
        
        public static string RemovePostFix(this string str, params string[] postFixes) => str.RemovePostFix(StringComparison.Ordinal, postFixes);

        private static string RemovePostFix(
            this string str,
            StringComparison comparisonType,
            params string[] postFixes)
        {
            if (str.IsNullOrEmpty() || ((ICollection<string>) postFixes).IsNullOrEmpty<string>())
                return str;
            foreach (string postFix in postFixes)
            {
                if (str.EndsWith(postFix, comparisonType))
                    return str.Left(str.Length - postFix.Length);
            }
            return str;
        }
        
        private static bool IsNullOrEmpty<T>(this ICollection<T> source) => source == null || source.Count <= 0;
        
        private static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);
    }
}