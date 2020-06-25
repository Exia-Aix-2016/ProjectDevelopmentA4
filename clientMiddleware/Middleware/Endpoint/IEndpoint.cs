using Middleware.Models;
using System.ServiceModel;

namespace Middleware
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IEndpointCallback))]
    public interface IEndpoint
    {
        [OperationContract(IsOneWay = true)]
        void MService(Message message);
    }
}
