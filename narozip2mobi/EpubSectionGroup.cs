using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace narozip2mobi
{
    public class EpubSectionGroup(int sectGrpNum, string sectGrpId, ReadOnlyCollection<EpubSection> sects, bool isEndOfVolume)
    {
        public readonly int SectionGroupNumber = sectGrpNum;
        public readonly string SectionGroupId = sectGrpId;
        public readonly ReadOnlyCollection<EpubSection> Sections = sects;
        public readonly bool IsEndOfVolume = isEndOfVolume;
    }
}
