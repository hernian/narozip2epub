using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Xml;

namespace narozip2mobi
{
    public partial class OpfTemplate(EpubVolume vol, string modDate)
    {
        private static string XmlEncode(string text)
        {
            return SecurityElement.Escape(text);
        }

        private readonly EpubVolume _vol = vol;
        private readonly string _modDate = modDate;
    }
}
