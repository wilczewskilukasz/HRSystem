using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HRinfoAPI.Models;

namespace HRinfoAPI.Controllers
{
    public class HumanResourcesTeamController : ApiController
    {
        HRinfoEntities database = new HRinfoEntities();

        // TODO: dokończyć
        //public IEnumerable<HumanResources> Get()
        //{
        //    var result = from d in database.Departments
        //                 join p in database.Positions on d.de

        //    return result.ToList();
        //}
    }

    public class HumanResources
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
    }
}
