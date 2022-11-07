using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Authenticator;
using DataTypes;
using System.ServiceModel;

namespace ServiceProvider.Controllers
{
    [RoutePrefix("api/calculator")]
    public class CalculatorController : ApiController
    {
        [Route("addTwoNumbers/{Number1}/{Number2}/{token}")]
        [Route("addTwoNumbers")]
        [HttpGet]
        public IHttpActionResult addTwoNumbers(int Number1, int Number2, int token)
        {
            if (validateToken(token) != "validated")
            {
                return Ok(new Result("Denied", "Authentication Error"));
            }
            return Ok(new Result("Successful", (Number1 + Number2).ToString()));
        }
        [Route("addThreeNumbers/{Number1}/{Number2}/{Number3}/{token}")]
        [Route("addThreeNumbers")]
        [HttpGet]
        public IHttpActionResult addThreeNumbers(int Number1, int Number2, int Number3, int token)
        {
            if (validateToken(token) != "validated")
            {
                return Ok(new Result("Denied", "Authentication Error"));
            }
            return Ok(new Result("Successful", (Number1 + Number2 + Number3).ToString()));
        }
        [Route("MulTwoNumbers/{Number1}/{Number2}/{token}")]
        [Route("MulTwoNumbers")]
        [HttpGet]
        public IHttpActionResult mulTwoNumbers(int Number1, int  Number2, int token)
        {
            if (validateToken(token) != "validated")
            {
                return Ok(new Result("Denied", "Authentication Error"));
            }
            return Ok(new Result("Successful", (Number1 * Number2).ToString()));
        }
        [Route("mulThreeNumbers/{Number1}/{Number2}/{Number3}/{token}")]
        [Route("mulThreeNumbers")]
        [HttpGet]
        public IHttpActionResult mulThreeNumbers(int Number1, int Number2, int Number3, int token)
        {
            if (validateToken(token) != "validated")
            {
                return Ok(new Result("Denied", "Authentication Error"));
            }
            return Ok(new Result("Successful", (Number1 * Number2 * Number3).ToString()));
        }
        private string validateToken(int token)
        {
            ChannelFactory<AuthenticationServiceInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8200/AuthenticationService";
            foobFactory = new ChannelFactory<AuthenticationServiceInterface>(tcp, URL);
            return foobFactory.CreateChannel().validate(token);
        }
        
    }
}
