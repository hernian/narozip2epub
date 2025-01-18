using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace narozip2mobi
{
    public partial class OpfTemplate(string title, string kanaTitle, string author, string kanaAuthor)
    {
        private readonly string _title = title;
        private readonly string _kanaTitle = kanaTitle;
        private readonly string _author = author;
        private readonly string _kanaAuthor = kanaAuthor;

        public void Generate(string pathOpf)
        {
            var strOpf = this.TransformText();
            TextFileUtil.WriteText(pathOpf, strOpf);
        }

        private static string XmlEncode(string text)
        {
            return SecurityElement.Escape(text);
        }
    }
}
