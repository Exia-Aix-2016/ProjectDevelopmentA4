using System;
using Middleware;
using System.ServiceModel;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private InstanceContext instanceContext;
        private EndpointClient endpointClient;
        public MainWindow()
        {
            InitializeComponent();

            ClientCallbackHandler.Update += Notify;
            instanceContext = new InstanceContext(new ClientCallbackHandler());
            endpointClient = new EndpointClient(instanceContext);

            endpointClient.MServiceAsync(new Message { TokenUser = "TesT",  OperationName = "BITE!!!!!!!!!!!!!!!!!!!!" });
        }

        public void Notify(Message message)
        {
            Console.WriteLine(message.OperationName);
        }
    }

}
