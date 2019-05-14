using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DrawShip.Viewer
{
    public static class StringExtensions
    {
        private static readonly Regex _supplantRegex = new Regex(@"\{(\w*?)\}", RegexOptions.Compiled);

        public static string Supplant(this string format, IReadOnlyDictionary<string, object> values)
        {
            return _supplantRegex.Replace(format, match =>
            {
                string key = match.Groups[1].Value;
                return values.ContainsKey(key)
                    ? values[key].ToString()
                    : "";
            });
        }

        public static string RemoveAll(this string original, string find)
        {
            var newString = original;
            do
            {
                original = newString;
                newString = newString.Replace(find, "");
            }
            while (newString != original);

            return newString;
        }
    }
}
