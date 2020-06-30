using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Models
{
    [DataContract]
    public class DecryptMsg
    {
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string CipherText { get; set; }

        [DataMember]
        public string PlainText { get; set; }

        [DataMember]
        public int[] Report { get; set; }

        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Secret { get; set; }
    }
}
