using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace narozip2mobi
{
    internal partial class HtmlUtils
    {
        private const int LENGTH_TCY_MAX = 3;
        public static string EncodeHead(string text)
        {
            return HttpUtility.HtmlEncode(text);
        }

        public static string EncodeBody(string text)
        {
            var temp = HttpUtility.HtmlEncode(text);
            var regexTcy = RegexDigits();
            var match = regexTcy.Match(temp);
            if (match.Success)
            {
                int idx = 0;
                var sb = new StringBuilder();
                do
                {
                    if (match.Length <= LENGTH_TCY_MAX)
                    {
                        var len = match.Index - idx;
                        sb.Append(temp.AsSpan(idx, len));
                        sb.Append($"<span class=\"tcy\">{match.Value}</span>");
                        idx = match.Index + match.Length;
                    }
                    else
                    {
                        var matchEnd = match.Index + match.Length;
                        var len = matchEnd - idx;
                        sb.Append(temp.AsSpan(idx, len));
                        idx = matchEnd;
                    }
                    match = regexTcy.Match(temp, idx);
                } while (match.Success);
                if (idx < temp.Length)
                {
                    sb.Append(temp.AsSpan(idx));
                }
                text = sb.ToString();
            }
            return text;
        }

        public static string EncodeBody(int num)
        {
            return EncodeBody(num.ToString());
        }

        [GeneratedRegex(@"\d+")]
        private static partial Regex RegexDigits();
    }
}
