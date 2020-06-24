using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Middleware
{
    public class Endpoint : IEndpoint
    {
        public void MService(Message message)
        {
            callback.MServiceCallback(message);
        }

        private IEndpointCallback callback { get => OperationContext.Current.GetCallbackChannel<IEndpointCallback>(); }
    }
}
