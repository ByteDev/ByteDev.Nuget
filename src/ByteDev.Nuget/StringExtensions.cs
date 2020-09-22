using System.Collections.Generic;
using System.Linq;

namespace ByteDev.Nuget
{
    internal static class StringExtensions
    {
        public static bool ContainsAny(this string source, IEnumerable<string> values)
        {
            if (string.IsNullOrEmpty(source))
                return false;

            return values.Any(value => source.Contains(value));
        }
    }
}