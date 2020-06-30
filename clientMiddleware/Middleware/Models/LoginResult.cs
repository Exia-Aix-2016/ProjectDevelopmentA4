using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Models
{
    /// <summary>
    /// Model which is used as response to send token to the client.
    /// </summary>
    [DataContract]
     public class LoginResult
    {
        [DataMember]
        public string TokenUser { get; set; }

        [DataMember]
        public bool IsValid { get; set; }
    }
}
