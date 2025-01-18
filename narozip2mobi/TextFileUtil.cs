using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace narozip2mobi
{
    internal class TextFileUtil
    {
        private static readonly Encoding ENC = new UTF8Encoding(false);

        public static void WriteText(string pathTextOut,  string strContent)
        {
            using var fs = new FileStream(pathTextOut, FileMode.Create, FileAccess.Write);
            using var sw = new StreamWriter(fs, ENC);
            sw.Write(strContent);
        }
    }
}
