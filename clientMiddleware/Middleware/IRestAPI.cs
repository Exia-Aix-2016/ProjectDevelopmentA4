using System.ServiceModel;
using System.ServiceModel.Web;

namespace Middleware
{
    [ServiceContract]
    public interface IRestAPI
    {
        [OperationContract]
        [WebInvoke]
        void MServiceRest(string message);
    }
}