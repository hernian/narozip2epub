using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace narozip2epub
{
    public class DocSection(string pathXhtml, string id, int sectionNumber, string title, ReadOnlyCollection<string> paragraphs)
    {
        public readonly string PathXhtml = pathXhtml;
        public readonly string ID = id;
        public readonly int SectionNumber = sectionNumber;
        public readonly string Title = title;
        public readonly ReadOnlyCollection<string> Paragraphs = paragraphs;
    }
}
