using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    public class MessageController : ApiController
    {
        [HttpPost]
        // api Send
        public string Send(string url, string label)
        {
            return "";
        }
    }
}