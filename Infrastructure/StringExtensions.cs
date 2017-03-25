using System.Text.RegularExpressions;

namespace AnimeciBackend
{
    public static class StringExtension
    {
        // public static string cfDecode(this IElement cfed)
        // {
        //     int s = cfed.TextContent.IndexOf("[email");
        //     int e = cfed.TextContent.LastIndexOf("*/");
        //     string str = cfed.TextContent.Remove(s, (e + 2) - s);

        //     string cfHash = cfed.InnerHtml.GetBetween("data-cfemail=", ">[email");
        //     cfHash = cfHash.Substring(1, cfHash.Length - 2);
        //     int k = int.Parse(cfHash.Substring(0, 2), NumberStyles.HexNumber);
        //     string hashed = "";
        //     for (int i = 2; i < cfHash.Length - 1; i += 2)
        //         hashed += (char)(int.Parse(cfHash.Substring(i, 2), NumberStyles.HexNumber) ^ k);

        //     return str.Insert(s, hashed);
        // }

        public static string GetBetween(this string strSource, string strStart, string strEnd, bool htmlClean = false)
        {
            int start, end;

            if (htmlClean)
                strSource = strSource.Replace("&quot;", "\"");

            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                start = strSource.IndexOf(strStart, 0) + strStart.Length;
                end = strSource.IndexOf(strEnd, start);
                return strSource.Substring(start, end - start);
            }
            else
            {
                return "";
            }
        }

        public static string GetFrom(this string strSource, string strStart, bool htmlClean = false)
        {
            int index = strSource.IndexOf(strStart) + strStart.Length;
            return strSource.Substring(index);
        }

        public static string Clean(this string source)
        {
            return source.Replace(",", "").Replace("\"", "");
        }

        public static string GenerateSlug(this string phrase)
        {
            var s = phrase.ToLower();
            s = Regex.Replace(s, @"[^a-z0-9\s-]", "");                      // remove invalid characters
            s = Regex.Replace(s, @"\s+", " ").Trim();                       // single space
            s = s.Substring(0, s.Length <= 45 ? s.Length : 45).Trim();      // cut and trim
            s = Regex.Replace(s, @"\s", "-");                               // insert hyphens
            return s.ToLower();
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static bool AitDegilse(this string cg, string istenen)
        {
            if (string.IsNullOrEmpty(istenen))
                return false;

            if (cg.Trim() == istenen)
                return false;

            return true;
        }
    }
}
