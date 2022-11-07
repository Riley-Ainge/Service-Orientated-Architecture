using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using System.ServiceModel;
using Authenticator;
using DataTypes;
using Registry;
using ServiceProvider;

namespace Publishing
{
    internal class Program
    {
        private static AuthenticationServiceInterface foob;
        private static RestClient client;
        static void Main(string[] args)
        {
            Setup();
            string input = "";
            int token = 0;
            while (input != "3")
            {
                Console.WriteLine("Welcome to SOA application \n 1. Log in \n 2. Register \n 3. Exit");
                input = Console.ReadLine();
                if (input == "1")
                {
                    token = LogIn();
                    if (token != -1)
                    {
                        while (input != "3")
                        {
                            Console.WriteLine("Login Successful \n 1. Publish \n 2. Unpublish \n 3. Exit");
                            input = Console.ReadLine();
                            if (input == "1")
                            {
                                Publish(token);
                            }
                            if (input == "2")
                            {
                                UnPublish(token);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Login");
                    }
                }
                if (input == "2")
                {
                    Registration();
                }
            }
        }
        public static void Setup()
        {
            ChannelFactory<AuthenticationServiceInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8200/AuthenticationService";
            foobFactory = new ChannelFactory<AuthenticationServiceInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
            //foob.setPath(@"C:\Users\gameb\OneDrive\Documents\Assignment1\");
            client = new RestClient("http://localhost:49755/");
            /*RestRequest request = new RestRequest("api/registry/setPath/{e}", Method.Put);
            request.AddUrlSegment("e", 1);
            request.AddJsonBody(new PathType("C:\\Users\\gameb\\OneDrive\\Documents\\Assignment1\\"));
            //request.AddUrlSegment("path", "C:\\Users\\gameb\\OneDrive\\Documents\\Assignment1\\");
            RestResponse responce = client.Put(request);
            RestRequest request = new RestRequest("api/registry/unpublish/{endpoint}/{token}", Method.Delete);
            request.AddUrlSegment("endpoint", "C:");
            request.AddUrlSegment("token", 1385693945);
            RestResponse resp = client.Delete(request);
            Result result = JsonConvert.DeserializeObject<Result>(resp.Content);
            Console.WriteLine(result.status + " " + result.result);*/
        }
        public static void Registration()
        {
            Console.WriteLine("Input username");
            string username = Console.ReadLine();
            Console.WriteLine("Input password");
            string password = Console.ReadLine();
            Console.WriteLine(foob.Register(username, password));
        }
        public static int LogIn()
        {
            Console.WriteLine("Input username");
            string username = Console.ReadLine();
            Console.WriteLine("Input password");
            string password = Console.ReadLine();
            return foob.Login(username, password);
        }
        public static void Publish(int token)
        {
            Console.WriteLine("Input name");
            string name = Console.ReadLine();
            Console.WriteLine("Input description");
            string description = Console.ReadLine();
            Console.WriteLine("Input API endpoint");
            string endpoint = Console.ReadLine();
            Console.WriteLine("Input number of operands");
            string operands = Console.ReadLine();
            Console.WriteLine("Input operand type");
            string type = Console.ReadLine();
            RestRequest request = new RestRequest("api/registry/publish/{token}", Method.Put);
            request.AddUrlSegment("token", token);
            request.AddJsonBody(JsonConvert.SerializeObject(new ServiceDescription(name, description, endpoint, operands, type)));
            RestResponse resp = client.Put(request);
            Result result = JsonConvert.DeserializeObject<Result>(resp.Content);
            Console.WriteLine(result.status + " "  + result.result);
        }
        public static void UnPublish(int token)
        {
            Console.WriteLine("Input API endpoint");
            string endpoint = Console.ReadLine();
            RestRequest request = new RestRequest("api/registry/unpublish/{endpoint}/{token}", Method.Delete);
            request.AddUrlSegment("endpoint", endpoint);
            request.AddUrlSegment("token", token);
            RestResponse resp = client.Delete(request);
            Result result = JsonConvert.DeserializeObject<Result>(resp.Content);
            Console.WriteLine(result.status + " " + result.result);
        }
    }
}
