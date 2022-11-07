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
using Newtonsoft.Json;
using RestSharp;
using DataTypes;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for Operation.xaml
    /// </summary>
    public partial class Operation : Window
    {
        RestRequest request;
        List<TextBox> boxes;
        int token;
        int operands;
        string path;
        private static RestClient client;
        public Operation(int _token, string _path, int _operands)
        {
            InitializeComponent();
            boxes = new List<TextBox>();
            client = new RestClient("http://localhost:49755/");
            token = _token;
            operands = _operands;
            path = _path;
            for (int i = 1; i < operands+1; i++)
            {
                path = path + "/{Number" + i + "}";
                TextBox txt = new TextBox();
                txt.Name = "Input" + i;
                txt.Text = "Input" + i;
                txt.Height = 20;
                txt.Width = 100;
                txt.Margin = new Thickness(50, 50*i, 0, 0);
                txt.HorizontalAlignment = HorizontalAlignment.Left;
                txt.VerticalAlignment = VerticalAlignment.Top;
                boxes.Add(txt);
                Grid1.Children.Add(txt);
            }
            path = path + "/{token}";
        }

        private async void Test_Click(object sender, RoutedEventArgs e)
        {
            request = new RestRequest(path, Method.Get);
            request.AddUrlSegment("token", token);
            for (int i = 1; i < operands + 1; i++)
            {
                try
                {
                    int.Parse(boxes[i - 1].Text);
                }
                catch
                {
                    return;
                }
                request.AddUrlSegment("Number" + i, boxes[i - 1].Text);
            }
            Bar.Visibility = Visibility.Visible;
            switchUI();
            Bar.Value = 30;
            await Task.Delay(1200);
            Bar.Value = 60;
            Task<RestResponse> task = new Task<RestResponse>(startTask);
            task.Start();
            RestResponse resp = await task;
            Bar.Visibility = Visibility.Hidden;
            switchUI();
            Result result = JsonConvert.DeserializeObject<Result>(resp.Content);
            Status.Content = result.status;
            Result.Content = result.result;
        }
        private RestResponse startTask()
        {
            return client.Get(request);
        }
        public void setName(string name)
        {
            Name.Content = name;
        }
        private void switchUI()
        {
            Test.IsEnabled = !Test.IsEnabled;
        }
    }
}
