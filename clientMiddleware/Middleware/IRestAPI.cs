using System.ServiceModel;
using System.ServiceModel.Web;

namespace Middleware
{
    [ServiceContract]
    public interface IRestAPI
    {
        [OperationContract]
        [WebInvoke]
        string MServiceRest(string message);
    }
}