using CommandLine;
using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Windows.Markup;

namespace narozip2mobi
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args).WithParsed(opt =>
            {
                Console.WriteLine($"SourceZip   : {opt.PathSourceZip}");
                Console.WriteLine($"OutputDir   : {opt.OutputDir}");
                Console.WriteLine($"Title       : {opt.Title}");
                Console.WriteLine($"Title(Kana) : {opt.KanaTitle}");
                Console.WriteLine($"Author      : {opt.Author}");
                Console.WriteLine($"Author(Kana): {opt.KanaAuthor}");

                var dirOut = Path.Combine(opt.OutputDir, opt.Author, opt.Title);
                Directory.CreateDirectory(dirOut);

                var contentsGen = new ContentsTemplate(opt.Title);
                using var zipArchive = ZipFile.OpenRead(opt.PathSourceZip);
                if (zipArchive.Entries.Count > opt.EpisodesInVolume)
                {
                    // 分冊する
                    var volNumber = 1;
                    var epNumber = 1;
                    var zipVolumeSplitter = new ZipVolumeSplitter(zipArchive.Entries, opt.EpisodesInVolume);
                    foreach (var epInVol in zipVolumeSplitter.GetEpisodesInVolume())
                    {
                        var dirOutVol = Path.Combine(dirOut, $"{volNumber}");
                        var titleSuffix = $"({volNumber})";
                        GenerateVolume(opt, dirOutVol, titleSuffix, epNumber, epInVol);
                        epNumber += epInVol.Count;
                        volNumber++;
                    }
                }
                else
                {
                    // 分冊しない
                    GenerateVolume(opt, dirOut, "", 1, zipArchive.Entries);
                }
            });
        }
      
        static void GenerateVolume(Options opt, string dirOut, string titleSuffix, int epNumber, ReadOnlyCollection<ZipArchiveEntry> zipEntries)
        {
            Directory.CreateDirectory(dirOut);

            var title = opt.Title + titleSuffix;
            var kanaTitle = opt.KanaTitle + titleSuffix;

            var contentsGen = new ContentsTemplate(title);
            foreach (var entry in zipEntries)
            {
                var zipEpisode = new ZipEpisode(epNumber, entry, opt.SkipBlank);
                var sec = new ContentsTemplate.Section($"ep{epNumber}", zipEpisode.Title);
                foreach (var paragraph in zipEpisode.GetContens())
                {
                    sec.Paragraphs.Add(paragraph);
                }
                contentsGen.Sections.Add(sec);
                epNumber++;
            }
            contentsGen.Generate(dirOut);

            var tocGen = new TocTemplate();
            foreach (var sect in contentsGen.Sections)
            {
                var linkTo = contentsGen.GetLinkTo(sect);
                tocGen.AddItem(linkTo, sect.Title);
            }
            tocGen.Generate(dirOut);

            var opfGen = new OpfTemplate(title, kanaTitle, opt.Author, opt.KanaAuthor);
            var oftFilename = $"{opt.OpfFilename}{titleSuffix}.opf";
            var pathOpf = Path.Combine(dirOut, oftFilename);
            opfGen.Generate(pathOpf);

            var batGen = new GenMobiBatTemplate(oftFilename);
            batGen.Generate(dirOut);

            var coverHtmlGen = new CoverHtmlTemplate(title);
            coverHtmlGen.Generate(dirOut);

            var coverImgGen = new CoverImageGenerator(title);
            coverImgGen.Generate(dirOut);
        }
    }
}
