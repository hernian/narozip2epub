using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace narozip2mobi
{
    public partial class GenMobiBatTemplate(string oftFilename)
    {
        private readonly string _opfFilename = oftFilename;

        public void Generate(string dirOut)
        {
            var pathBat = Path.Combine(dirOut, "genmobi.bat");
            var strBat = this.TransformText();
            TextFileUtil.WriteText(pathBat, strBat);
        }
    }
}
