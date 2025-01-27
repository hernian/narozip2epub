using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace narozip2epub
{
    internal class ZeroFillFormatter
    {
        private readonly string _format;

        public ZeroFillFormatter(int maxValue)
        {
            var sb = new StringBuilder();
            for (int i = maxValue; i > 0; i /= 10)
            {
                sb.Append('0');
            }
            _format = sb.ToString();
        }

        public string Format(int val)
        {
            var strVal = val.ToString(_format);
            return strVal;
        }
    }
}
