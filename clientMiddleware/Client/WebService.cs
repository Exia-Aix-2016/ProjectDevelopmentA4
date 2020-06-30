using Middleware.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
		private static readonly Lazy<WebService> _instance = new Lazy<WebService>(() => new WebService());
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

			
			client.ClientCredentials.Windows.ClientCredential.UserName = Properties.Resources.WINUSER;
			client.ClientCredentials.Windows.ClientCredential.Password = Properties.Resources.WINPASSWORD; 
			client.ClientCredentials.Windows.ClientCredential.Domain = Properties.Resources.WINDOMAIN;

		}

		public static WebService Instance
		{
			get => _instance.Value;
		}

		void Notify(Message message)
		{
			if (message.OperationName == "TOKEN")
			{
				LoginResult result = (LoginResult)message.Data;
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
			client.MService(message);
		}

		public void Upload(string fileName, string txt)
		{
			Message message = new Message
			{
				Data = new DecryptMsg
				{
					FileName = fileName,
					CipherText = txt
				},
				OperationName = "DECRYPT",
				TokenApp = APP_TOKEN,
				TokenUser = userToken
			};
			client.MService(message);
		}
	}
}