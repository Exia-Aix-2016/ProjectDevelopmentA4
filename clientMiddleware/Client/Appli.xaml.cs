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
using Microsoft.Win32;

namespace Client
{
    /// <summary>
    /// Logique d'interaction pour appli.xaml
    /// </summary>
    public partial class Appli : Window
    {
        WebService webService = WebService.Instance;
        Processing processing = new Processing();

        public Appli()
        {
            InitializeComponent();

            this.webService.Update += this.Notify;

            this.Closed += (o, e) => this.webService.Update -= this.Notify;
        }

        public void Notify(Message message)
        {
            Console.WriteLine(message.OperationName);
        }

        private void btnOpenFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                    fileUpload.Items.Add(System.IO.Path.GetFileName(filename));
            }
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            processing.Show();
            Close();
        }

    }
}
