
using System.Windows;
using Microsoft.Win32;

namespace Client
{
    /// <summary>
    /// Logique d'interaction pour appli.xaml
    /// </summary>
    public partial class Appli : Window
    {
        public Appli()
        {
            InitializeComponent();
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
    }
}
