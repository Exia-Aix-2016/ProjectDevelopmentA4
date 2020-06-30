﻿
using Middleware.Models;
using Middleware.Services;
using Middleware.Services.AuthService;
using System;
using System.Collections.Concurrent;
using System.ServiceModel;

namespace Middleware
{
    /// <summary>
    /// EndPoint of the Middleware its connected to client via NetTcpBinding and to Backend via WebHttpBehavior
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class Endpoint : IEndpoint, IRestAPI
    {
        private static readonly ConcurrentDictionary<string, IEndpointCallback> clients = new ConcurrentDictionary<string, IEndpointCallback>();
        private IEndpointCallback callback = null;


        public static readonly IService decryptService = new DecryptService(new Uri("http://192.168.20.10:8282/webservice/resources/cipher"));
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
            var authServiceToken = ((IToken)authService);


            if (callback != null && message.TokenUser != null && message.TokenUser != string.Empty)
                clients.AddOrUpdate(message.TokenUser, callback, (k, v) => callback);

            
            //Check if user is not authentified (tokenUser == null && OpName == AUTHENTIFICATION) and Also check AppToken
            if (message.TokenUser == null && authServiceToken.IsValidAppToken(message.TokenApp) && message.OperationName == "AUTHENTIFICATION")
            {
                Message returnMessage = authService.ServiceAction(message);
                callback.MServiceCallback(returnMessage);

            }
            else if(message.TokenUser != null)
            {
                // TODO IS AUTH 
                

                if (!authServiceToken.IsValidToken(message.TokenUser) || !authServiceToken.IsValidAppToken(message.TokenApp))
                {
                    message.OperationName = "DROP";
                    message.Info = "Invalid Tokens (user or app)";
                    clients[message.TokenUser].MServiceCallback(message);
                    return;
                }

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
