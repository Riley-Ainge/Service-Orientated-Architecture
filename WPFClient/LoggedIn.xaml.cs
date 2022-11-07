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
using RestSharp;
using Newtonsoft.Json;
using DataTypes;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for LoggedIn.xaml
    /// </summary>
    public partial class LoggedIn : Window
    {
        int token;
        private static RestClient client;
        RestRequest request;
        public LoggedIn(int _token)
        {
            token = _token;
            InitializeComponent();
            client = new RestClient("http://localhost:49755/");
            request = new RestRequest("api/registry/getAll/{token}", Method.Get);
            request.AddUrlSegment("token", token);
            asyncStartup();
            


        }
        public async void asyncStartup()
        {
            Bar.Visibility = Visibility.Visible;
            switchUI();
            Bar.Value = 30;
            await Task.Delay(1500);
            Bar.Value = 60;
            Task<RestResponse> task = new Task<RestResponse>(startTask);
            task.Start();
            RestResponse resp = await task;
            Bar.Visibility = Visibility.Hidden;
            switchUI();
            List<ServiceDescription> results = JsonConvert.DeserializeObject<List<ServiceDescription>>(resp.Content);
            foreach (ServiceDescription result in results)
            {
                Box.Items.Add("Name: " + result.name + "\nDescription: " + result.description + "\nAPI Endpoint: " + result.endpoint + "\nNumber of Operands: " + result.operands + "\nOperand Type: " + result.type);
            }
        }

        private async void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            request = new RestRequest("api/registry/search/{term}/{token}", Method.Post);
            request.AddUrlSegment("term", Search.Text);
            request.AddUrlSegment("token", token);
            Bar.Visibility = Visibility.Visible;
            switchUI();
            Bar.Value = 30;
            await Task.Delay(1500);
            Bar.Value = 60;
            Task<RestResponse> task = new Task<RestResponse>(searchTask);
            task.Start();
            RestResponse resp = await task;
            Bar.Visibility = Visibility.Hidden;
            switchUI();
            List<ServiceDescription> results = JsonConvert.DeserializeObject<List<ServiceDescription>>(resp.Content);
            Box.Items.Clear();
            foreach(ServiceDescription result in results)
            {
                Box.Items.Add("Name: "+result.name+"\nDescription: "+result.description+"\nAPI Endpoint: "+result.endpoint+"\nNumber of Operands: "+result.operands+"\nOperand Type: "+result.type);
            }
        }
        private RestResponse searchTask()
        {
            return client.Post(request);
        }
        private RestResponse startTask()
        {
            return client.Get(request);
        }
        private void switchUI()
        {
            Box.IsEnabled = !Box.IsEnabled;
            SearchBtn.IsEnabled = !SearchBtn.IsEnabled;
        }
        private void Box_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            string[] lines = Box.Items[Box.SelectedIndex].ToString().Split('\n');
            string path = lines[2].Split(':')[1].Trim() + ":" + lines[2].Split(':')[2] + ":" + lines[2].Split(':')[3];
            string operands = lines[3].Split(':')[1].Trim();
            Operation newWindow = new Operation(token, path, int.Parse(operands));
            newWindow.Show();
            newWindow.setName(lines[0]);
        }
    }
}
