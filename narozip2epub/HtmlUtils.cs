using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace narozip2epub
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
            var regexTcy = RegexAlnum();
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

        private static void ToZenkaku(StringBuilder sb, ReadOnlySpan<char> chSpan)
        {
            foreach (var ch in chSpan)
            {
                if ('0' <= ch && ch <= '9')
                {
                    var zench = '\uFF00' + (ch - '0');
                    sb.Append(zench);
                }
                else if ('a' <= ch && ch <= 'z')
                {
                    var zench = '\uFF41' + (ch - '0');
                    sb.Append(zench);
                }
                else
                {
                    var zench = '\uFF21' + (ch - '0');
                    sb.Append(zench);
                }
            }
        }

        public static string EncodeBody(int num)
        {
            return EncodeBody(num.ToString());
        }

        [GeneratedRegex(@"([0-9a-zA-Z]+)")]
        private static partial Regex RegexAlnum();

    }
}
