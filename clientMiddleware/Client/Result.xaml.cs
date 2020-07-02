﻿using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
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

                File.WriteAllBytes("monpdf.pdf", decrypt.Report.Select(x => (byte)x).ToArray());
                string result = string.Join("", decrypt.Report);
                Console.WriteLine(result);

                System.Diagnostics.Process.Start("monpdf.pdf");
            }
            else
            {
                FileNameOff.Text = decrypt.FileName;
                KeyNameOff.Text = decrypt.Key;
            }
        }
    }
}
