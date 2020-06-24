using Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientCallbackHandler : IEndpointCallback
    {
        private IComm controller;
        public ClientCallbackHandler(IComm controller)
        {
            this.controller = controller;
        }
        public void MServiceCallback(Message message)
        {
            controller.notify(message);
        }
    }
}
