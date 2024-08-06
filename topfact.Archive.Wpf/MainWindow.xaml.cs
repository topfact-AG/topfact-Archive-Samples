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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace topfact.Archive.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public topfact.Archive.ApiClient.TfaApiClient ApiClient { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var clientId = txtClientId.Text;
            var clientKey = txtClientKey.Text;

            ApiClient = new ApiClient.TfaApiClient(txtApiUrl.Text);
            var token = ApiClient.LogonApp(clientId, clientKey);


            if (token == null)
            {
                MessageBox.Show("Invalid token.", Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            lblUser.Content = token.Username;
            lblExpired.Content = token.ValidTo;
        }
    }
}
