using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

namespace HRinfoAPI.Controllers
{
    public class TestConnectionController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> TestGet()
        {
            return StatusCode(HttpStatusCode.OK);
        }

        [HttpPost]
        public async Task<IHttpActionResult> TestPost()
        {
            return StatusCode(HttpStatusCode.OK);
        }
    }
}
