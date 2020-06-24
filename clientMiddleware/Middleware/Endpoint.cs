﻿
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
            if (callback != null && message.TokenUser != string.Empty)
                clients.AddOrUpdate(message.TokenUser, callback, (k, v) => callback);

            if (message.TokenUser == null)
            {
                   //TODO LOGIN
            }
            else
            {
                //TODO IS AUTH 
                switch (message.OperationName)
                {
                    case "DECRYPT":
                        break;
                    case "SOLUTION":
                        if (clients.ContainsKey(message.TokenUser))
                        {
                            Console.WriteLine(message.TokenUser);
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
