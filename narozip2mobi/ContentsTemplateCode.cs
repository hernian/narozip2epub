using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace narozip2mobi
{
    public partial class ContentsTemplate(string title)
    {
        public readonly string FILENAME_CONTENTS_HTML = "contents.html";
        public class Section(string id, string title)
        {
            public readonly string Id = id;
            public readonly string Title = title;
            public readonly List<string> Paragraphs = [];
        }

        public readonly string Title = title;
        public List<Section> Sections = [];

        public string GetLinkTo(Section sect)
        {
            return $"{FILENAME_CONTENTS_HTML}#{sect.Id}";
        }

        public void Generate(string dirOut)
        {
            var pathContentHtml = Path.Combine(dirOut, FILENAME_CONTENTS_HTML);
            var strContentHtml = this.TransformText();
            TextFileUtil.WriteText(pathContentHtml, strContentHtml);

            var pathExe = Assembly.GetExecutingAssembly().Location;
            var dirExe = Path.GetDirectoryName(pathExe) ?? "";
            var pathStyleSrc = Path.Combine(dirExe, "style.css");
            var pathStyleDst = Path.Combine(dirOut, "style.css");
            File.Copy(pathStyleSrc, pathStyleDst, true);
        }

        private static string HtmlEncode(string text)
        {
            return HttpUtility.HtmlEncode(text);
        }
    }
}
