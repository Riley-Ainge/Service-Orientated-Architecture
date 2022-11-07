using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataTypes;
using Registry.Models;
using System.ServiceModel;
using Authenticator;

namespace Registry.Controllers
{
    [RoutePrefix("api/registry")]
    public class RegistryController : ApiController
    {
        [Route("publish/{token}")]
        [Route("publish")]
        [HttpPut]
        public IHttpActionResult Publish(int token, ServiceDescription service)
        {
            if (validateToken(token) != "validated")
            {
                return Ok(new Result("Denied", "Authentication Error"));
            }
            RegistryDataTier.addService(service);
            return Ok(new Result("Successful", ""));
        }
        [Route("search/{term}/{token}")]
        [Route("search")]
        [HttpPost]
        public IHttpActionResult Search(string term, int token)
        {
            if (validateToken(token) != "validated")
            {
                return Ok(new Result("Denied", "Authentication Error"));
            }
            List<ServiceDescription> services = RegistryDataTier.getServices();
            List<ServiceDescription> foundServices = new List<ServiceDescription>();
            foreach(ServiceDescription item in services)
            {
                if(item.description.Contains(term))
                {
                    foundServices.Add(item);
                }
            }
            return Ok(foundServices);
        }
        [Route("getAll/{token}")]
        [Route("getAll")]
        [HttpGet]
        public IHttpActionResult allServices(int token)
        {
            if (validateToken(token) != "validated")
            {
                return Ok(new Result("Denied", "Authentication Error"));
            }
            return Ok(RegistryDataTier.getServices());
        }
        [Route("unpublish/{endpoint}/{token}")]
        [Route("unpublish")]
        [HttpDelete]
        public IHttpActionResult unPublish(string endpoint, int token)
        {
            if (validateToken(token) != "validated")
            {
                return Ok(new Result("Denied", "Authentication Error"));
            }
            List<ServiceDescription> services = RegistryDataTier.getServices();
            for(int i = 0; i < services.Count; i++)
            {
                if (services[i].endpoint.Contains(endpoint))
                {
                    RegistryDataTier.removeAt(i);
                    return Ok(new Result("Successful", "Removed"));
                }
            }
            return Ok(new Result("Unsuccessful", "Endpoint not found"));
        }
        /*[Route("setPath/{e}")]
        [Route("setPath")]
        [HttpPut]
        public void setPath(string path)
        {
            RegistryDataTier.setFilePath(path + "services.txt");
        }*/
        private string validateToken(int token)
        {
            ChannelFactory<AuthenticationServiceInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8200/AuthenticationService";
            foobFactory = new ChannelFactory<AuthenticationServiceInterface>(tcp, URL);
            AuthenticationServiceInterface foob = foobFactory.CreateChannel();
            //foob.setPath("C:\\Users\\gameb\\OneDrive\\Documents\\Assignment1\\");
            return foob.validate(token);
        }
    }
}
