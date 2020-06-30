using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Services.Authentification
{
    public interface IToken
    {
        bool IsValidToken(string token);
        bool IsValidAppToken(string token);
    }
}
