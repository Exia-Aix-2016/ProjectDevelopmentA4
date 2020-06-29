using Middleware.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
	public sealed class WebService
	{
		private static readonly WebService _instance = new WebService();
		public delegate void NotifCtrl(Message message);
		public event NotifCtrl Update;
		private ClientCallbackHandler clientCallbackHandler;
		private InstanceContext context;
		private EndpointClient client;
		private string userToken;
		private const string APP_TOKEN = "e2lOmEf7z2YcWNOsMgwxrytjcOftPwpi";



		private WebService()
		{
			ClientCallbackHandler.Update += Notify;
			clientCallbackHandler = new ClientCallbackHandler();
			context = new InstanceContext(clientCallbackHandler);
			client = new EndpointClient(context);
		}

		public static WebService Instance
		{
			get => _instance;
		}
		
		void Notify(Message message)
        {
			if (message.OperationName == "TOKEN")
            {
				LoginResult result = (LoginResult) message.Data;
				userToken = result.TokenUser;
            }
			Update?.Invoke(message);
			
		}

		public void Login(string username, string password)
        {
			Message message = new Message
			{
				Data = new Credential
				{
					Username = username,
					Password = password
				},
				OperationName = "AUTHENTIFICATION",
				TokenApp = APP_TOKEN

			};

			client.MServiceAsync(message);
        }
	}
}
