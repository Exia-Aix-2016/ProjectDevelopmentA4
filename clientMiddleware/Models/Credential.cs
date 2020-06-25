using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [DataContract]
    public class Credential
    {
        [DataMember(Name = "Username", EmitDefaultValue = false)]
        public string Username { get; set; }

        [DataMember(Name = "Password", EmitDefaultValue = false)]
        public string Password { get; set; }

    }
}
