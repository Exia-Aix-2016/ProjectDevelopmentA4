using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DecryptMsg
    {
        public string FileName { get; set; }

        public string CipherText { get; set; }

        public string PlainText { get; set; }

        public string Report { get; set; }

        public string Key { get; set; }

        public string Secret { get; set; }
    }
}
