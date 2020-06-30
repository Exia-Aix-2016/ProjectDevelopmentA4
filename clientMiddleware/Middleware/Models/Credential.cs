using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Models
{
    /// <summary>
    /// Model which is used for carring credentials information from client to Middleware
    /// </summary>
    [DataContract]
    public class Credential
    {
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }

    }
}
