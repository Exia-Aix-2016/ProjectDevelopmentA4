using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Middleware.Models;

namespace Client
{
    public partial class Result : Window
    {
        WebService webService = WebService.Instance;

        public Result()
        {
            InitializeComponent();
            
            this.webService.Update += this.Notify;

            this.Closed += (o, e) => this.webService.Update -= this.Notify;

        }

        public void Notify(Message message)
        {
            Console.WriteLine(message.OperationName);
            if (message.OperationName == "SOLUTION")
            {
                PrintResult(message);
            }
        }

        public void PrintResult(Message message)
        {
            var decrypt = (DecryptMsg)message.Data;

            var content = $"{decrypt.FileName} {decrypt.Key} {decrypt.Secret} \n";

            ResultBox.Text += content;

            if (decrypt.Secret != null)
            {
                File.WriteAllBytes("monpdf.pdf", decrypt.Report.Select(x => (byte)x).ToArray());
                ButtonOuvrir.IsEnabled  = true;
            }
                       
        }

        private void ButtonOuvrir_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("monpdf.pdf");
        }
    }
}
