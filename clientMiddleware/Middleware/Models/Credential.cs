using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Models
{
    [DataContract]
    public class Credential
    {
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }

    }
}
