using Middleware.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Services
{
    public interface IService
    {
        Message ServiceAction(Message message);
        void StopService();

    }
}
