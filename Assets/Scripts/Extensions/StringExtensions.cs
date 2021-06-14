using System.Linq;

namespace Assets.Scripts.Extensions
{
    public static class StringExtensions
    {
        public static string FindSSID(this string str)
        {
            var strings = str.Split('\n');
            foreach (var s in strings)
            {
                if (s.Contains("SSID"))
                {
                    return s.Replace("SSID", " ").Replace(":", " ").Trim(' ').Normalized();
                }
            }

            return string.Empty;
        }

        public static string FindSSIDKey(this string str)
        {
            var strings = str.Split('\n');
            foreach (var s in strings)
            {
                if (s.Contains("Content"))
                {
                    var result = s.Replace("Key Content", " ").Replace(":", " ").Trim(' ').Normalized();
                    return result;
                }
            }
            return string.Empty;
        }

        private static string Normalized(this string str)
        {
            var chars = str.ToCharArray().ToList();
            chars.Remove(chars.Last());
            return new string(chars.ToArray());
        }
    }
}
