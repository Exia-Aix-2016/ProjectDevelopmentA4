using Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IComm
    {
        private InstanceContext instanceContext;
        private EndpointClient endpointClient;
        public MainWindow()
        {
            InitializeComponent();

            instanceContext = new InstanceContext(new ClientCallbackHandler(this));
            endpointClient = new EndpointClient(instanceContext);

            
        }

        public void notify(Message message)
        {
            Console.WriteLine(message.OperationName);
        }
    }

}
