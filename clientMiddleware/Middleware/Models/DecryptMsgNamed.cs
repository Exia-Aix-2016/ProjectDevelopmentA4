using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Models
{
    /// <summary>
    /// Helper used by DecryptService to link file with an user
    /// </summary>
    public class DecryptMsgNamed
    {
        public string UserToken { get; set; }
        public DecryptMsg DecryptMsg { get; set; }
    }
}
