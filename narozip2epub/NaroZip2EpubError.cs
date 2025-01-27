using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace narozip2epub
{
    internal class NaroZip2EpubError: Exception
    {
        public NaroZip2EpubError(string message)
            : base(message)
        {
        }
    }
}
