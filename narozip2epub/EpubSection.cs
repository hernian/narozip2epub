using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace narozip2epub
{
    public class EpubSection(int sectNum, string sectId, string title, ReadOnlyCollection<string> paragraphs)
    {
        public readonly int SectionNumber = sectNum;
        public readonly string SectionId = sectId;
        public readonly string Title = title;
        public readonly ReadOnlyCollection<string> Paragraphs = paragraphs;
    }
}
