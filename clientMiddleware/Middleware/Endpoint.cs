using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Middleware
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class Endpoint : IEndpoint, IRestAPI
    {
        private static ConcurrentDictionary<string, IEndpointCallback> clients = new ConcurrentDictionary<string, IEndpointCallback>();
        private IEndpointCallback callback = null;

        public Endpoint()
        {
            try
            {
                callback = OperationContext.Current.GetCallbackChannel<IEndpointCallback>();

                
            }
            catch (Exception err){
            }
        }

        public void MService(Message message)
        {

            if(callback != null && message.TokenUser != string.Empty)
                clients.AddOrUpdate(message.TokenUser, callback, (k,v) => callback);


            if(message.TokenUser != string.Empty && clients.ContainsKey(message.TokenUser))
            {
                Console.WriteLine(message.TokenUser);
                clients[message.TokenUser].MServiceCallback(message);
            }    
        }

        public void MServiceRest(string message)
        {
            MService(new Message { TokenUser = "TesT", OperationName = message });

             Console.WriteLine(message);
        }
        //private IEndpointCallback callback { get => OperationContext.Current.GetCallbackChannel<IEndpointCallback>(); }
    }
}
