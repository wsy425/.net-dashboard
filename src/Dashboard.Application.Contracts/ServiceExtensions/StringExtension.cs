
using System.Collections.Generic;
using System.Linq;

namespace Dashboard.ServiceExtensions
{
    public static class StringExtension
    {
        private static readonly IEnumerable<string> BackGroundSuffixes = new List<string>{"jpg","png"};
        private static readonly char[] SeparateCode = {'_'};
        public static bool IsContainPngOrJpgName(this string name,string field)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (string.IsNullOrEmpty(field))
            {
                return BackGroundSuffixes.Any(name.Contains);
            }

            if (!name.Contains(field))
            {
                return false;
            }
            return BackGroundSuffixes.Any(name.Contains);
        }

        public static string RemoveSeparateCode(this string name)
        {
            return name.Contains(SeparateCode[0].ToString()) ? name.Split(SeparateCode,2)[1]: name;
        }
    }
}