using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace narozip2mobi
{
    public partial class CoverHtmlTemplate(string title)
    {
        private readonly string _title = title;

        public void Generate(string dirOut)
        {
            var pathCoverHtml = Path.Combine(dirOut, "cover.html");
            var strCoverHtml = this.TransformText();
            TextFileUtil.WriteText(pathCoverHtml, strCoverHtml);
        }

        private static string HttpEncode(string text)
        {
            return HttpUtility.HtmlEncode(text);
        }
    }
}
