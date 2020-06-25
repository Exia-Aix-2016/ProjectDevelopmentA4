using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
 
    public class LoginResult
    {
        public string TokenUser { get; set; }

        public bool IsValid { get; set; }
    }
}
