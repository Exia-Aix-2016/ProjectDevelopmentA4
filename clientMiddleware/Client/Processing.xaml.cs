using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Middleware.Models;

namespace Client
{
    public partial class Processing : Window
    {
        WebService webService = WebService.Instance;

        public Processing()
        {
            InitializeComponent();

            this.webService.Update += this.Notify;

            this.Closed += (o, e) => this.webService.Update -= this.Notify;
        }

        public void Notify(Message message)
        {
            Console.WriteLine(message.OperationName);
        }

    }

}
