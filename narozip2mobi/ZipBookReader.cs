using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace narozip2mobi
{
    internal class ZipBookReader(string filename, string title, string author)
    {
        private const string START_OF_SECTION = "********************************************";
        private const string END_OF_SECTION = "************************************************";

        private readonly string _filename = filename;
        private readonly string _title = title;
        private readonly string _author = author;
        private readonly Encoding _enc = new UTF8Encoding(false);

        public Book ReadBook()
        {
            var zipArch = ZipFile.OpenRead(_filename);
            var sectNum = 1;
            List<Book.Section> listSect = [];
            foreach (var entry in zipArch.Entries)
            {
                var sect = ReadSection(entry, sectNum);
                listSect.Add(sect);
                sectNum++;
            }
            var book = new Book(_title, _author, listSect.AsReadOnly());
            return book;
        }

        private Book.Section ReadSection(ZipArchiveEntry entry, int sectNum)
        {
            List<string> listLine = [];
            using var stream = entry.Open();
            using var reader = new StreamReader(stream, _enc);
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }
                listLine.Add(line);
            }

            // 章の先頭にコメントがあってSTART_OF_SECTION文字列の後に本文がくることがある。
            // START_OF_SECTION文字列はなろうの仕様か作者の個人的な決めなのか不明
            var idxStart = listLine.FindIndex(0, aLine => aLine == START_OF_SECTION);
            if (idxStart >= 0)
            {
                listLine.RemoveRange(0, idxStart + 1);
            }
            // 章の最後にEND_OF_SECTION文字列の後にコメントがくることがある。
            // END_OF_SECTION文字列はなろうの仕様か作者の個人的な決めなのか不明
            var idxEnd = listLine.FindIndex(0, aLine => aLine == END_OF_SECTION);
            if (idxEnd >= 0)
            {
                listLine.RemoveRange(idxEnd, listLine.Count - idxEnd);
            }
            // 章の末尾にある空白行を削除する
            var idxEndNotBlank = listLine.Count - 1;
            while (idxEndNotBlank >= 0)
            {
                var line = listLine[idxEndNotBlank];
                if (line != string.Empty)
                {
                    break;
                }
                idxEndNotBlank--;
            }
            if (idxEndNotBlank < listLine.Count - 1)
            {
                listLine.RemoveRange(idxEndNotBlank, listLine.Count - idxEndNotBlank);
            }
            // タイトルと少なくとも１行の本文のため最低でも2行は必要
            while (listLine.Count < 2)
            {
                listLine.Add(string.Empty);
            }

            var title = listLine[0];
            var body = listLine[1..(listLine.Count - 1)];
            var sect = new Book.Section(sectNum, title, body.AsReadOnly());
            return sect;
        }
    }
}
