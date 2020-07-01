using System;
using System.Windows;
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

            if (decrypt.Secret != null)
            {
                FileNameOn.Text = decrypt.FileName;
                KeyNameOn.Text = decrypt.Key;
                SecretNameOn.Text = decrypt.Secret;
            }
            else
            {
                FileNameOff.Text = decrypt.FileName;
                KeyNameOff.Text = decrypt.Key;
            }

        }
    }
}
