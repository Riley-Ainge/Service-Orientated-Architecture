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
using System.ServiceModel;
using Authenticator;
using DataTypes;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static AuthenticationServiceInterface foob;
        string name; string password;
        public MainWindow()
        {
            InitializeComponent();
            ChannelFactory<AuthenticationServiceInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8200/AuthenticationService";
            foobFactory = new ChannelFactory<AuthenticationServiceInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
            GlobalPath.path = Path.Text;
            //foob.setPath(Path.Text);
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            Bar.Visibility = Visibility.Visible;
            Bar.Value = 30;
            switchUI();
            await Task.Delay(1000);
            Bar.Value = 60;
            Task<int> task = new Task<int>(loginTask);
            task.Start();
            int token = await task;
            switchUI();
            Bar.Visibility = Visibility.Hidden;
            if (token != -1)
            {
                LoggedIn newWindow = new LoggedIn(token);
                newWindow.Show();
                this.Close();
            }
        }
        private int loginTask()
        {
            return foob.Login(name, password);
        }
        private string registerTask()
        {
            return foob.Register(name, password);
        }
        private void switchUI()
        {
            name = Name.Text;
            password = Password.Text;
            Login.IsEnabled = !Login.IsEnabled;
            Register.IsEnabled = !Register.IsEnabled;
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            Bar.Visibility = Visibility.Visible;
            switchUI();
            Bar.Value = 30;
            await Task.Delay(1000);
            Bar.Value = 60;
            Task<string> task = new Task<string>(registerTask);
            task.Start();
            Status.Content = await task;
            switchUI();
            Bar.Visibility = Visibility.Hidden;
        }

        private void Path_TextChanged(object sender, TextChangedEventArgs e)
        {
            GlobalPath.path = Path.Text;
        }
    }
}
