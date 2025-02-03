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
        private const int LENGTH_UR_MAX = 5;
        public static string EncodeHead(string text)
        {
            return HttpUtility.HtmlEncode(text);
        }

        public static string EncodeBody(string text)
        {
            List<string> listTextFragment = [];
            var regexDigits = RegexDigit();
            int idx = 0;
            var match = regexDigits.Match(text, idx);
            if (match.Success)
            {
                do
                {
                    if (idx < match.Index)
                    {
                        listTextFragment.Add(text[idx..match.Index]);
                        idx = match.Index;
                    }
                    var idxMatchEnd = match.Index + match.Length;
                    listTextFragment.Add(text[idx..idxMatchEnd]);
                    idx = idxMatchEnd;
                    match = regexDigits.Match(text, idx);
                }while (match.Success);
                if (idx < text.Length)
                {
                    listTextFragment.Add(text[idx..text.Length]);
                }
            }
            else
            {
                listTextFragment.Add(text);
            }

            var sb = new StringBuilder();
            foreach (var textFragment in listTextFragment)
            {
                var isAllDigit = textFragment.All(ch => IsNumeric(ch));
                var isAllLetterOrDigit = textFragment.All(ch => IsAlphaNumeric(ch));
                var temp = HttpUtility.HtmlEncode(textFragment);
                if ((0 < textFragment.Length) && (textFragment.Length <= LENGTH_TCY_MAX) && isAllDigit)
                {
                    sb.Append("<span class=\"tcy\">");
                    sb.Append(temp);
                    sb.Append("</span>");
                }
                else if ((0 < textFragment.Length) && isAllLetterOrDigit)
                {
                    sb.Append("<span class=\"ur\">");
                    sb.Append(temp);
                    sb.Append("</span>");
                }
                else
                {
                    sb.Append(temp);
                }
            }
            return sb.ToString();
        }

        private static bool IsNumeric(char ch)
        {
            return ('0' <= ch && ch <= '9');
        }

        private static bool IsAlphaNumeric(char ch)
        {
            if ('0' <= ch && ch <= '9')
            {
                return true;
            }
            if ('A' <= ch && ch <= 'Z')
            {
                return true;
            }
            if ('a' <= ch && ch <= 'z')
            {
                return true;
            }
            return false;
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

        [GeneratedRegex(@"(\d+|[ -~]+)")]
        private static partial Regex RegexDigit();

    }
}
