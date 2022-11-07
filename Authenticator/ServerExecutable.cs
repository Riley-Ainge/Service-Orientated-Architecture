using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Authenticator
{
    internal class ServerExecutable
    {
        static void Main(string[] args)
        {
            //This should *definitely* be more descriptive.
            Console.WriteLine("hey so like welcome to my server");
            //This is the actual host service system
            ServiceHost host;
            //This represents a tcp/ip binding in the Windows network stack
            NetTcpBinding tcp = new NetTcpBinding();
            //Bind server to the implementation of DataServer
            host = new ServiceHost(typeof(AuthenticationService));
            //Present the publicly accessible interface to the client. 0.0.0.0 tells .net to accept on any interface. :8100 means this will use port 8100. DataService is a name for the actual service, this can be any string.

            host.AddServiceEndpoint(typeof(AuthenticationServiceInterface), tcp, "net.tcp://0.0.0.0:8200/AuthenticationService");
            //And open the host for business!
            host.Open();
            Console.WriteLine("System Online");
            Console.WriteLine("Clear token time? (ms)");
            try
            {
                AuthenticationService.clearTokens(int.Parse(Console.ReadLine()));
            }
            catch(Exception e)
            {
                Console.WriteLine("Invalid Input");
            }
            Console.ReadLine();
            //Don't forget to close the host after you're done!
            host.Close();
        }
    }
}
