using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HRinfoAPI.Controllers
{
    public class ApiTestController : ApiController
    {
        [HttpGet]
        public string TestGet()
        {
            return "Test value.";
        }

        [HttpPost]
        public string TestPost()
        {
            return "Test value.";
        }

        [HttpGet]
        public int TestGet(int value)
        {
            return value;
        }

        [HttpPost]
        public int TestPost(int value)
        {
            return value;
        }
    }
}
