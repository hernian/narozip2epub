using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace narozip2mobi
{
    public partial class TocTemplate
    {
        private readonly List<(string LinkTo, string Name)> _items = [];

        public void AddItem(string linkTo, string name)
        {
            _items.Add((linkTo, name));
        }

        public void Generate(string dirOut)
        {
            var pathToc = Path.Combine(dirOut, "toc.html");
            var strToc = this.TransformText();
            TextFileUtil.WriteText(pathToc, strToc);
        }

        private static string HttpEncode(string text)
        {
            return HttpUtility.HtmlEncode(text);
        }
    }
}
