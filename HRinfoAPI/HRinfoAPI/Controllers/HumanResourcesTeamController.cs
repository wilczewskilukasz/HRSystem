using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HRinfoAPI.Models;

namespace HRinfoAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class HumanResourcesTeamController : ApiController
    {
        HRinfoEntities database = new HRinfoEntities();
        
        [Route("HumanResources")]
        public IEnumerable<HumanResources> Get()
        {
            var result = from e in database.Employees
                             join p in database.Positions on e.PositionId equals p.Id
                             join d in database.Departments on p.DepartmentId equals d.Id
                             where d.DepartmentCode == "HR"
                         select new HumanResources()
                         {
                             FirstName = e.FirstName,
                             LastName = e.LastName,
                             PhoneNumber = e.JobPhone,
                             Email = e.JobEmail,
                             Position = p.Name
                         };

            return result.ToList();
        }
    }

    public class HumanResources
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
    }
}
