using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace narozip2mobi
{
    internal class ZipBookReader(string filename)
    {
        private const double RATIO_BLANK_LINE_IN_EVERY_OTHER_LINE = 0.4;
        // ここから、解析オプション
        public string StartSectionMarker = string.Empty;
        public string EndSectionMarker = string.Empty;
        public string Title = "名無し本";
        public string KanaTitle = "ナナシホン";
        public string Author = "名無し";
        public string KanaAuthor = "ナナシ";
        // ここまで、解析オプション

        private readonly string _filename = filename;
        private readonly Encoding _enc = new UTF8Encoding(false);

        public Book ReadBook()
        {
            using var zipArch = ZipFile.OpenRead(_filename);
            var sectNum = 1;
            List<Book.Section> listSect = [];
            foreach (var entry in zipArch.Entries)
            {
                var sect = ReadSection(entry, sectNum);
                listSect.Add(sect);
                sectNum++;
            }
            var book = new Book(this.Title, this.KanaTitle, this.Author, this.KanaAuthor, listSect.AsReadOnly());
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
            if (this.StartSectionMarker != string.Empty)
            {
                var idxStart = listLine.FindIndex(0, aLine => aLine == this.StartSectionMarker);
                if (idxStart >= 0)
                {
                    listLine.RemoveRange(0, idxStart + 1);
                }
            }
            // 章の最後にEND_OF_SECTION文字列の後にコメントがくることがある。
            // END_OF_SECTION文字列はなろうの仕様か作者の個人的な決めなのか不明
            if (this.EndSectionMarker != string.Empty)
            {
                var idxEnd = listLine.FindIndex(0, aLine => aLine == EndSectionMarker);
                if (idxEnd >= 0)
                {
                    listLine.RemoveRange(idxEnd, listLine.Count - idxEnd);
                }
            }
            // タイトルと少なくとも１行の本文のため最低でも2行は必要
            while (listLine.Count < 2)
            {
                listLine.Add(string.Empty);
            }
            var title = listLine[0];
            var body = listLine[1..listLine.Count];

            // 章の先頭にある空白行を削除する
            RemoveFrontBlankLines(body);
            // 章の末尾にある空白行を削除する
            RemoveLastBlankLines(body);

            // 章のなかの空行の数が全体の40%を超えていたら、隔行に空白があるものと見做して空白行を削除する
            var countBlankLine = body.Count(line => line == string.Empty);
            if ((double)countBlankLine / listLine.Count >= RATIO_BLANK_LINE_IN_EVERY_OTHER_LINE)
            {
                body = RemoveBlankLineInEveryOtherLine(body);
            }

            var sect = new Book.Section(sectNum, title, body.AsReadOnly());
            return sect;
        }

        private static void RemoveFrontBlankLines(List<string> listLine)
        {
            int idxNotBlank = listLine.FindIndex(a => a !=  string.Empty);
            if (idxNotBlank > 0)
            {
                listLine.RemoveRange(0, idxNotBlank);
            }
        }

        private static void RemoveLastBlankLines(List<string> listLine)
        {
            var idxEndNotBlank = listLine.Count;
            while (idxEndNotBlank > 0)
            {
                var line = listLine[idxEndNotBlank - 1];
                if (line != string.Empty)
                {
                    break;
                }
                idxEndNotBlank--;
            }
            if (idxEndNotBlank < listLine.Count)
            {
                listLine.RemoveRange(idxEndNotBlank, listLine.Count - idxEndNotBlank);
            }
        }
        private static List<string> RemoveBlankLineInEveryOtherLine(IEnumerable<string> lines)
        {
            var linePrev = string.Empty; // 仮の前行
            List<string> listNew = [];
            foreach (var line in lines)
            {
                if (line != string.Empty || linePrev == string.Empty)
                {
                  listNew.Add(line);
                }
                linePrev = line;
            }
            return listNew;
        }
    }
}
