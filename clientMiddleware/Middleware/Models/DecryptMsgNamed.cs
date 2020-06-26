using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Models
{
    public class DecryptMsgNamed
    {
        public string UserToken { get; set; }
        public DecryptMsg DecryptMsg { get; set; }
    }
}
