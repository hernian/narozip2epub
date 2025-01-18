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
        [Option("output_dir", Required = true, HelpText = "Output directory")]
        public string OutputDir { get; set; } = string.Empty;
        [Option("opf_filename", Required = true, HelpText = "OPF Filename (wo extension)")]
        public string OpfFilename { get; set; } = string.Empty;

        [Option("title", Required = true, HelpText = "Title")]
        public string Title { get; set; } = string.Empty;

        [Option("kana_title", Required = true, HelpText = "Kana Title")]
        public string KanaTitle { get; set; } = string.Empty;
        [Option("author", Required = true, HelpText = "Author")]
        public string Author { get; set; } = string.Empty;

        [Option("kana_author", Required = true, HelpText = "Kana Author")]
        public string KanaAuthor {  get; set; } = string.Empty;
        [Option("skip_blank", Required = false, Default = 0, HelpText = "無駄な改行の扱い")]
        public int SkipBlank { get; set; } = 0;
        [Option("epsodes_in_volume", Required = false, Default = 50, HelpText = "分冊時のエピソード数")]
        public int EpisodesInVolume { get; set; } = 30;
        [Value(0, Required = true, MetaName = "SourceZip", HelpText = "Source Zip Filename")]
        public string PathSourceZip {  get; set; } = string.Empty;
    }
}
