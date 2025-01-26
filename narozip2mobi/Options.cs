using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace narozip2mobi
{
    internal class Options
    {
        [Option("output-dir", Required = true, HelpText = "Output directory")]
        public string OutputDir { get; set; } = string.Empty;

        [Option("title", Required = true, HelpText = "Title")]
        public string Title { get; set; } = string.Empty;

        [Option("kana-title", Required = true, HelpText = "Kana Title")]
        public string KanaTitle { get; set; } = string.Empty;
        [Option("author", Required = true, HelpText = "Author")]
        public string Author { get; set; } = string.Empty;

        [Option("kana-author", Required = true, HelpText = "Kana Author")]
        public string KanaAuthor {  get; set; } = string.Empty;
        [Option("source-zip", Required = true, HelpText = "Source Zip Filename")]
        public string PathSourceZip {  get; set; } = string.Empty;
    }
}
