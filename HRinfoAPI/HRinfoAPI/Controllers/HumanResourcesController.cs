using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRinfoAPI.Models;

namespace HRinfoAPI.Controllers
{
    [RoutePrefix("api/HumanResources")]
    public class HumanResourcesController : ApiController
    {
        HRinfoEntities database = new HRinfoEntities();
        
        [Route("Team")]
        public IEnumerable<EmployeeContactData> Get()
        {
            var result = from e in database.Employees
                             join p in database.Positions on e.PositionId equals p.Id
                             join d in database.Departments on p.DepartmentId equals d.Id
                             where d.DepartmentCode == "HR"
                         select new EmployeeContactData()
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
}
