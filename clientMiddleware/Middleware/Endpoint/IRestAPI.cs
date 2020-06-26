using Middleware.Models;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Middleware
{
    [ServiceContract]
    public interface IRestAPI
    {
        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Json, Method = "POST")]
        void MServiceRest(Message message);
    }
}