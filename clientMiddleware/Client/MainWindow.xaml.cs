using System;
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
using Middleware.Models;

namespace Client
{
    public partial class MainWindow : Window
    {
        WebService webService = WebService.Instance;
        Appli appli = new Appli();

        public MainWindow()
        {
            InitializeComponent();

            /*textBoxEmail.Text = "quentin@viacesi.fr";*/
            passwordBox1.Password = "root";
            
            this.webService.Update += this.Notify;

            this.Closed += (o, e) => this.webService.Update -= this.Notify;
        }

        public void Notify(Message message)
        {
            Console.WriteLine(message.OperationName);
            if(message.OperationName=="TOKEN")
            {
                appli.Show();
                Close();
            }
            else if(message.OperationName == "DROPMESSAGE")
            {
                Console.WriteLine("mais non");
                errormessage.Text = "Mauvais identifiants";
            }
        }

        public void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= TextBox_GotFocus;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxEmail.Text.Length == 0)
            {
                errormessage.Text = "Username vide.";
                textBoxEmail.Focus();
            }
            else if(passwordBox1.Password.Length ==0)
            {
                errormessage.Text = "Mot de Passe vide.";
                passwordBox1.Focus();
            }
            else
            {
                Console.WriteLine("clic");
                string email = textBoxEmail.Text;
                string password = passwordBox1.Password;

                webService.Login(email, password);
            }
        }

    }

}
