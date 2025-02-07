﻿using CommandLine;
using System.Collections.ObjectModel;
using System.IO.Compression;
using System.IO.Pipes;
using System.Windows.Markup;

namespace narozip2epub
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

                var bookReader = new ZipBookReader(opt.PathSourceZip)
                {
                    Title = opt.Title,
                    KanaTitle = opt.KanaTitle,
                    Author = opt.Author,
                    KanaAuthor = opt.KanaAuthor,
                    StartSectionMarker = "********************************************",
                    EndSectionMarker = "************************************************",
                };
                var book = bookReader.ReadBook();

                var epubGen = new EpubGenerator(book, opt.OutputDir);
                epubGen.Generate();
            });
        }
    }
}
