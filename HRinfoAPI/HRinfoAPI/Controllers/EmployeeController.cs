using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HRinfoAPI.Models;
using Microsoft.AspNet.Identity;

namespace HRinfoAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Employee")]
    public class EmployeeController : ApiController
    {
        HRinfoEntities database = new HRinfoEntities();
        int? workerId;

        private void GetEmployeeId()
        {
            workerId = database.AspNetUsers.Where(a => a.Id == User.Identity.GetUserId()).Select(a => a.EmployeeId).SingleOrDefault();
        }

        private IEnumerable<EmployeeAddress> GetEmployeeAddress(
            int? employeeId = 0,
            int? addressTypeId = 0,
            string addressTypeName = null)
        {
            if (employeeId > 0)
                workerId = employeeId;

            if (!workerId.HasValue)
                return null;

            if (!String.IsNullOrEmpty(addressTypeName))
                addressTypeId = database.AddressTypes.Where(a => a.Name == addressTypeName).Select(a => a.Id).SingleOrDefault();

            if (!addressTypeId.HasValue)
                addressTypeId = 0;

            var result = from e in database.Employees
                         join ea in database.EmployeesAddresses on e.Id equals ea.EmployeeId
                         join at in database.AddressTypes on ea.AddressTypeId equals at.Id
                         join a in database.Addresses on ea.AddressId equals a.Id
                         join s in database.Streets on a.StreetId equals s.Id
                         join c in database.Cities on a.CityId equals c.Id
                         join r in database.Countries on a.CountryId equals r.Id
                         where e.Id == workerId
                            && (addressTypeId == 0 || at.Id == addressTypeId)
                         select new EmployeeAddress()
                         {
                             Street = s.Name,
                             HouseNumber = a.HouseNumber,
                             FlatNumber = a.FlatNumber,
                             PostalCode = a.PostalCode,
                             City = c.Name,
                             Country = r.Name,
                             AddressType = at.Name
                         };

            return result.ToList();
        }

        // TODO: znalezc pracownika po imieniu i nazwisku
        [Route("FindEmployeeContactData")]
        public IEnumerable<EmployeeContactData> FindEmployeeContactData(int employeeId)
        {
            var result = from e in database.Employees
                         join p in database.Positions on e.PositionId equals p.Id
                         where e.Id == employeeId
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
        
        [Route("CurrentUserContactData")]
        public IEnumerable<EmployeeContactData> CurrentUserContactData()
        {
            this.GetEmployeeId();

            var result = from e in database.Employees
                         join p in database.Positions on e.PositionId equals p.Id
                         where e.Id == workerId
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

        [Route("CurrentUserAddress")]
        public IEnumerable<EmployeeAddress> CurrentUserAddresses()
        {
            this.GetEmployeeId();

            return this.GetEmployeeAddress();
        }

        [Route("CurrentUserAddress")]
        public IEnumerable<EmployeeAddress> CurrentUserAddress(int addressTypeId)
        {
            this.GetEmployeeId();

            return this.GetEmployeeAddress(null, addressTypeId);
        }

        [Route("CurrentUserAddress")]
        public IEnumerable<EmployeeAddress> CurrentUserAddress(string addressTypeName)
        {
            this.GetEmployeeId();

            return this.GetEmployeeAddress(null, null, addressTypeName);
        }

        [Route("Salaries")]
        public IEnumerable<EmployeeSalaries> Salaries()
        {
            this.GetEmployeeId();

            var result = from e in database.Employees
                         join p in database.PayOffs on e.Id equals p.EmployeeId
                         where e.Id == workerId
                         select new EmployeeSalaries { Amount = p.Amount, Date = p.Date };

            return result.ToList();
        }
    }
}
