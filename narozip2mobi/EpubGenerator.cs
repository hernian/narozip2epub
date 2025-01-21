using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing.Text;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace narozip2mobi
{
    internal class EpubGenerator(Book book, Options opt)
    {
        private const int SIZE_VOLUME_MAX = 1000 * 1024;
        private const int SIZE_SECTION_GROUP_MAX = 200 * 1024;

        private readonly Encoding _enc = new UTF8Encoding(false);

        private readonly Book _book = book;
        private readonly Options _opt = opt;
        private readonly string _dirOut = opt.OutputDir;

        public void Generate()
        {
            var vols = SplitBookToVolumes(_book);
            foreach (var vol in vols)
            {
                GenerateVolume(vol);
            }
            foreach (var vol in vols)
            {
                CompressVolume(vol);
            }
        }

        private ReadOnlyCollection<EpubVolume> SplitBookToVolumes(Book book)
        {
            // ボリュームとセクショングループは0埋めした値をディレクトリ名やファイル名に使用する。
            // ファイル名によるソートを容易にするため（Windowsエクスプローラーは0埋めしなくてもそれなりにソートするが…）
            // そのため、一度Book全体をボリュームやセクショングループに分離し、それぞれの総数が分かる状態にしてから
            // 最終的なボリュームとセクショングループとして組み立てる。
            // O埋めの桁数が分かるので。
            List<EpubVolume> listTempVol = [];
            List<EpubSectionGroup> listTempSectGrp = [];
            List<EpubSection> listSect = [];
            int sizeTempVol = 0;
            int sizeTempSectGrp = 0;
            // int tempVolNum = 1;
            // int tempSectGrpNum = 1;
            foreach (var sect in book.Sections)
            {
                var sectId = $"s{sect.SectionNumber}";
                var epubSect = new EpubSection(sect.SectionNumber, sectId, sect.Title, sect.Paragraphs);
                var htmlSectTempl = new XhtmlSectionTemplate(epubSect);
                var strHtmlSect = htmlSectTempl.TransformText();
                var sizeHtmlSect = _enc.GetBytes(strHtmlSect).Length;

                var sizeTempVolNext = sizeTempVol + sizeHtmlSect;
                var sizeTempSectGrpNext = sizeTempSectGrp + sizeHtmlSect;
                if (listSect.Count > 0 && (sizeTempSectGrpNext >= SIZE_SECTION_GROUP_MAX || sizeTempVolNext >= SIZE_VOLUME_MAX))
                {
                    // Debug.WriteLine($"sectGrp#{tempSectGrpNum} sizeVolNext: {sizeTempVolNext}, sizeSectGrpNext: {sizeTempSectGrpNext}, countSection:{listSect.Count}");
                    var tempSectGrp = new EpubSectionGroup(-1, string.Empty, listSect.AsReadOnly(), false);
                    listTempSectGrp.Add(tempSectGrp);
                    listSect = [];
                    sizeTempSectGrp = 0;
                    // tempSectGrpNum++;
                }
                if (listTempSectGrp.Count > 0 && sizeTempVolNext >= SIZE_VOLUME_MAX)
                {
                    // Debug.WriteLine($"vol#{tempVolNum} sizeVol: {sizeTempVol} countSectGrp: {listTempSectGrp.Count}");
                    var tempVol = new EpubVolume(-1, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, listTempSectGrp.AsReadOnly());
                    listTempVol.Add(tempVol);
                    listTempSectGrp = [];
                    sizeTempVol = 0;
                    // tempVolNum++;
                }
                listSect.Add(epubSect);
                sizeTempSectGrp += sizeHtmlSect;
                sizeTempVol += sizeHtmlSect;
            }
            if (listSect.Count > 0)
            {
                var tempSectGrp = new EpubSectionGroup(-1, string.Empty, listSect.AsReadOnly(), false);
                listTempSectGrp.Add(tempSectGrp);
            }
            if (listTempSectGrp.Count > 0)
            {
                var tempVol = new EpubVolume(-1, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, listTempSectGrp.AsReadOnly());
                listTempVol.Add(tempVol);
            }

            var volIdFormatter = new ZeroFillFormatter(listTempVol.Count);
            int volNum = 1;
            List<EpubVolume> listVol = [];
            foreach (var tempVol in listTempVol)
            {
                // Debug.WriteLine($"vol#{volNum} {tempVol.SectionGroups.Count}");
                var sectGrpIdFofmatter = new ZeroFillFormatter(tempVol.SectionGroups.Count);
                var sectGrpNum = 1;
                var sectGrps = tempVol.SectionGroups;
                List<EpubSectionGroup> listSectGrp = [];
                foreach (var tempSectGrp in sectGrps)
                {
                    // Debug.WriteLine($"vol#{volNum} sectGrp#{sectGrpNum} countSection: {tempSectGrp.Sections.Count}");
                    var sectGrpId = $"p-{sectGrpIdFofmatter.Format(sectGrpNum)}";
                    var isEndOfVolume = (tempSectGrp == sectGrps[^1]);
                    var sectGrp = new EpubSectionGroup(sectGrpNum, sectGrpId, tempSectGrp.Sections, isEndOfVolume);
                    listSectGrp.Add(sectGrp);
                    sectGrpNum++;
                }
                var dirName = $"{_opt.Title}({volIdFormatter.Format(volNum)})";
                var volTitle = $"{_opt.Title}({volNum})";
                var volKanaTitle = $"{_opt.KanaTitle}({volNum})";
                var uniqueId = Guid.NewGuid().ToString();
                var vol = new EpubVolume(volNum, dirName, volTitle, volKanaTitle, opt.Author, opt.KanaAuthor, uniqueId, listSectGrp.AsReadOnly());
                listVol.Add(vol);
                volNum++;
            }
            return listVol.AsReadOnly();
        }

        private void GenerateVolume(EpubVolume vol)
        {
            var dirVol = Path.Combine(_dirOut, vol.DirectoryName);
            var dirMetaInf = Path.Combine(dirVol, "META-INF");
            var dirItem = Path.Combine(dirVol, "item");
            var dirImage = Path.Combine(dirItem, "image");
            var dirStyle = Path.Combine(dirItem, "style");
            var dirXhtml = Path.Combine(dirItem, "xhtml");

            string[] dirs = [dirVol, dirMetaInf, dirItem, dirImage, dirStyle, dirXhtml];
            foreach (var dir in dirs)
            {
                Directory.CreateDirectory(dir);
            }

            var pathMimetype = Path.Combine(dirVol, "mimetype");
            TextFileUtil.WriteText(pathMimetype, Properties.Resources.manifest);

            var pathContainerXml = Path.Combine(dirMetaInf, "container.xml");
            TextFileUtil.WriteText(pathContainerXml, Properties.Resources.container_xml);

            var pathStyleCss = Path.Combine(dirStyle, "style.css");
            TextFileUtil.WriteText(pathStyleCss, Properties.Resources.style_css);

            var pathNaviDocXhtml = Path.Combine(dirItem, "navigation-documents.xhtml");
            var tocGen = new TocTemplate(vol);
            var strToc = tocGen.TransformText();
            TextFileUtil.WriteText(pathNaviDocXhtml, strToc);

            var pathCoverJpg = Path.Combine(dirImage, "cover.jpg");
            var coverImgGen = new CoverImageGenerator(vol.Title);
            coverImgGen.Generate(pathCoverJpg);

            var pathCoverXhtml = Path.Combine(dirXhtml, "p-cover.xhtml");
            var xhtmlCoverTempl = new XhtmlCoverTemplate(vol.Title);
            var strXhtmlCoverTempl = xhtmlCoverTempl.TransformText();
            TextFileUtil.WriteText(pathCoverXhtml, strXhtmlCoverTempl);

            foreach (var sectGrp in vol.SectionGroups)
            {
                var pathSectGrp = Path.Combine(dirXhtml, $"{sectGrp.SectionGroupId}.xhtml");
                var sectGrpTempl = new XhtmlSectionGroupTemplate(vol.Title, sectGrp);
                var strXhtml = sectGrpTempl.TransformText();
                TextFileUtil.WriteText(pathSectGrp, strXhtml);
            }

            var pathOpf = Path.Combine(dirItem, "standard.opf");
            var modDate = $"{DateTime.UtcNow:s}Z";
            var opfTempl = new OpfTemplate(vol, modDate);
            var strOpf = opfTempl.TransformText();
            TextFileUtil.WriteText(pathOpf, strOpf);
        }

        private void CompressVolume(EpubVolume vol)
        {
            var dirVol = Path.Combine(_dirOut, vol.DirectoryName);
            var pathZip = Path.Combine(_dirOut, $"{vol.DirectoryName}.epub");
            using var fs = new FileStream(pathZip, FileMode.Create, FileAccess.Write);
            using var zipArch = new ZipArchive(fs, ZipArchiveMode.Create);
            CompressFile(zipArch, "mimetype", CompressionLevel.NoCompression, Path.Combine(dirVol, "mimetype"));
            CompressFile(zipArch, "META-INF/container.xml", CompressionLevel.Optimal, Path.Combine(dirVol, "META-INF", "container.xml"));
            CompressFile(zipArch, "item/standard.opf", CompressionLevel.Optimal, Path.Combine(dirVol, "item", "standard.opf"));
            CompressFile(zipArch, "item/navigation-documents.xhtml", CompressionLevel.Optimal, Path.Combine(dirVol, "item", "navigation-documents.xhtml"));
            CompressFile(zipArch, "item/style/style.css", CompressionLevel.Optimal, Path.Combine(dirVol, "item", "style", "style.css"));
            CompressFile(zipArch, "item/image/cover.jpg", CompressionLevel.Optimal, Path.Combine(dirVol, "item", "image", "cover.jpg"));
            var pathXhtmls = Directory.GetFiles(Path.Combine(dirVol, "item", "xhtml"), "*.xhtml");
            Array.Sort(pathXhtmls);
            foreach (var pathXhtml in pathXhtmls)
            {
                var filename = Path.GetFileName(pathXhtml);
                CompressFile(zipArch, $"item/xhtml/{filename}", CompressionLevel.Optimal, pathXhtml);
            }
        }

        private static void CompressFile(ZipArchive zipArch, string entryName, CompressionLevel compLvl, string pathSrc)
        {
            var entry = zipArch.CreateEntry(entryName, compLvl);
            using var fsEntry = entry.Open();
            using var fsSrc = new FileStream(pathSrc, FileMode.Open, FileAccess.Read);
            fsSrc.CopyTo(fsEntry);
        }
    }
}
