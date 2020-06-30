using System;
using System.Windows;
using Middleware.Models;
using Microsoft.Win32;
using System.IO;

namespace Client
{
    /// <summary>
    /// Logique d'interaction pour appli.xaml
    /// </summary>
    public partial class Appli : Window
    {
        WebService webService = WebService.Instance;
        Processing processing = new Processing();
        OpenFileDialog openFileDialog = new OpenFileDialog();

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

            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                    fileUpload.Items.Add(System.IO.Path.GetFileName(filename));
            }
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            foreach (string filename in openFileDialog.FileNames)
            {
                string text = File.ReadAllText(filename);
                webService.Upload(System.IO.Path.GetFileName(filename), text);
            }
            processing.Show();
            Close();
        }

    }
}
