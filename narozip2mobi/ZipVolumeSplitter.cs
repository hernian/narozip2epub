using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace narozip2mobi
{
    internal class ZipVolumeSplitter(ReadOnlyCollection<ZipArchiveEntry> entries, int epsodesInVolume)
    {
        private readonly ReadOnlyCollection<ZipArchiveEntry> _entries = entries;
        private readonly int _epsodesInVolume = epsodesInVolume;

        public IEnumerable<ReadOnlyCollection<ZipArchiveEntry>> GetEpisodesInVolume()
        {
            var list = new List<ZipArchiveEntry>();
            var countEpisode = 0;
            foreach (var entry in _entries)
            {
                list.Add(entry);
                ++countEpisode;
                if (countEpisode >= _epsodesInVolume)
                {
                    yield return list.AsReadOnly();
                    list = new List<ZipArchiveEntry>();
                    countEpisode = 0;
                }
            }
            if (list.Count > 0)
            {
                yield return list.AsReadOnly();
            }
        }
    }
}
