using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace narozip2epub
{
    public class EpubVolume(
        int volNum,
        string dirName,
        string title,
        string kanaTitle,
        string author,
        string kanaAuthor,
        string uniqueId,
        ReadOnlyCollection<EpubSectionGroup> sectGrp)
    {
        public readonly int VolumeNumber = volNum;
        public readonly string DirectoryName = dirName;
        public readonly string Title = title;
        public readonly string KanaTitle = kanaTitle;
        public readonly string Author = author;
        public readonly string KanaAuthor = kanaAuthor;
        public readonly string UniqueId = uniqueId;
        public readonly ReadOnlyCollection<EpubSectionGroup> SectionGroups = sectGrp;
    }
}
