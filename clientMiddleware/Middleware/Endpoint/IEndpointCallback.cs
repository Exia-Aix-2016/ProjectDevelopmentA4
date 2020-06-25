using Middleware.Models;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Middleware
{
    public interface IEndpointCallback
    {
        [OperationContract(IsOneWay = true)]
        void MServiceCallback(Message message);
        

    }
}
