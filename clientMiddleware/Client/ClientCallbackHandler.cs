using Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class ClientCallbackHandler : IEndpointCallback
    {
        public void MServiceCallback(Message message)
        {
            //TODO : Notify Controller !
            Console.WriteLine(message.OperationName);
        }
    }
}
