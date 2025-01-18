using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace narozip2mobi
{
    internal class ZipEpisode
    {
        private const string START_OF_EPISODE = "********************************************";
        private const string END_OF_EPISODE = "************************************************";

        public readonly string Id;
        public readonly string Title;
        private readonly List<string> _contents = [];
        private readonly int _skipBlank;

        public ZipEpisode(int epNumber, ZipArchiveEntry zipEntry, int skipBlank)
        {
            this.Id = $"EP{epNumber}";
            this._skipBlank = skipBlank;

            var enc = new UTF8Encoding(false);
            var contents = new List<string>();
            using (var stream = zipEntry.Open())
            using (var reader = new StreamReader(stream, enc))
            {
                while (true)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    contents.Add(line);
                }
            }
            var idxTitle = FindTitleIndex(contents);
            this.Title = $"{epNumber}: {contents[idxTitle]}";

            // 章末から空行を削除する
            var tempContents = contents.Slice(idxTitle + 1, contents.Count - idxTitle - 1);
            for (int i = tempContents.Count - 1; i >= 0; i--)
            {
                var line = tempContents[i];
                if (line != string.Empty)
                {
                    break;
                }
                tempContents.RemoveAt(i);
            }
            _contents = tempContents;
        }

        private static int FindTitleIndex(List<string> contents)
        {
            int idx = contents.FindIndex(0, cnt => cnt == START_OF_EPISODE);
            if (idx == -1)
            {
                return 0;
            }
            var idxTitle = idx + 1;
            if (idxTitle >= contents.Count)
            {
                return 0;
            }
            return idxTitle;
        }

        public IEnumerable<string> GetContens()
        {
            var prevLine = "";
            foreach (var line in _contents)
            {
                if (line == END_OF_EPISODE)
                {
                    break;
                }
                if (_skipBlank != 1 || prevLine == string.Empty || line != string.Empty)
                {
                    yield return line;
                }
                prevLine = line;
            }
        }
    }
}
