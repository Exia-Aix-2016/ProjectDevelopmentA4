

using Middleware.Models;

namespace Client
{
    /// <summary>
    /// Class to get message from the Middleware
    /// </summary>
    public class ClientCallbackHandler : IEndpointCallback
    {
        //Event to Update Controller
        public delegate void NotifCtrl(Message message);
        public static event NotifCtrl Update;
    
        //Will be call when message comes from Middleware
        public void MServiceCallback(Message message)
        {
            Update?.Invoke(message);
        }
    }
}
