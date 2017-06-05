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
        public string Test()
        {
            return "Test value.";
        }

        public int Test(int value)
        {
            return value;
        }
    }
}
