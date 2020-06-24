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
        //Event to Update Controller
        public delegate void NotifCtrl(Message message);
        public static event NotifCtrl Update;
    
        public ClientCallbackHandler()
        {

        }
        public void MServiceCallback(Message message)
        {
            Update?.Invoke(message);
        }
    }
}
