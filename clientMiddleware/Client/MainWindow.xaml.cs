using System;
using Middleware;
using System.ServiceModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Client
{
    public partial class MainWindow : Window
    {
        private InstanceContext instanceContext;
        private EndpointClient endpointClient;
        Appli appli = new Appli();

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


        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxEmail.Text.Length == 0)
            {
                errormessage.Text = "Mail vide.";
                textBoxEmail.Focus();
            }
            else if (!Regex.IsMatch(textBoxEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                errormessage.Text = "Mail invalide.";
                textBoxEmail.Select(0, textBoxEmail.Text.Length);
                textBoxEmail.Focus();
            }
            else
            {
                string email = textBoxEmail.Text;
                string password = passwordBox1.Password;

                if (passwordBox1.Password.Length == 0)
                {
                    errormessage.Text = "Mot de Passe vide.";
                    passwordBox1.Focus();
                }
                else
                {
                    appli.Show();
                    Close();
                }
            }
        }
    }

}
