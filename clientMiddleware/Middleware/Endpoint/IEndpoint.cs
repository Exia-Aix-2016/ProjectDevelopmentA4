using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Middleware
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IEndpointCallback))]
    public interface IEndpoint
    {
        [OperationContract(IsOneWay = true)]
        void MService(Message message);
    }
}
