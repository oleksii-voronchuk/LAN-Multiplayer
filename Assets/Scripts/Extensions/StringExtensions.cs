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
                    return s.Replace("SSID", "").Replace(":", "").Trim(' ');
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
                    return s.Replace("Key Content", "").Replace(":", "").Trim(' ');
                }
            }
            return string.Empty;
        }
    }
}
