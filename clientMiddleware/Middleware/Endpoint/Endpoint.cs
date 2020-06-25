
using Middleware.Services;
using Middleware.Services.AuthService;
using Models;
using Newtonsoft.Json;
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
        private static readonly ConcurrentDictionary<string, IEndpointCallback> clients = new ConcurrentDictionary<string, IEndpointCallback>();
        private IEndpointCallback callback = null;

        public static readonly IService decryptService = new DecryptService();
        public static readonly IService authService = new AuthService();

        public Endpoint()
        {
            try
            {
                callback = OperationContext.Current.GetCallbackChannel<IEndpointCallback>();
            }
            catch { }//When MServiceRest call.

        }

        public void MService(Message message)
        {
            if (callback != null && message.TokenUser != null && message.TokenUser != string.Empty)
                clients.AddOrUpdate(message.TokenUser, callback, (k, v) => callback);

            if (message.TokenUser == null)
            {
                Message returnMessage = authService.ServiceAction(message);

                callback.MServiceCallback(message);

            }
            else
            {
                // TODO IS AUTH 

                ((IToken)authService).IsValidToken(message.TokenUser);

                switch (message.OperationName)
                {
                    case "DECRYPT":

                        decryptService.ServiceAction(message);

                        break;
                    case "SOLUTION":
                        if (clients.ContainsKey(message.TokenUser))
                        {
                            //Stop the process by users
                            ((IDecryptService)decryptService).StopOperation(message);

                            clients[message.TokenUser].MServiceCallback(message);
                        }
                        else
                        {
                            throw new ClientDisconnectedException();
                        }
                        break;
                    default:
                        throw new InvalidOperationException("The message operation is invalid");
                }
            }
        }

        public void MServiceRest(Message message)
        {
            if(message != null)
            {
               MService(message);
            }
        }
    }
}
