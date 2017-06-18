using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Http;
using HRinfoAPI.Models;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Web.Http.Cors; // przestrzeń dla CORS (Cross Domain)

namespace HRinfoAPI.Controllers
{
    // TODO: Rozwiązanie problemu z lokalnym przekazywaniem danych (cross-domain)
    // W celu rozwiązania problemu Cross-Domain informacje znajują się pod hasłem:
    // wab api2 cross domain
    // i są dostępne pod adresme:
    // https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/enabling-cross-origin-requests-in-web-api

    // TODO: uncoment
    //[Authorize]
    //[EnableCors(origins: "http://localhost", headers: "application/json", methods: "GET")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Employee")]
    public class EmployeeController : ApiController
    {
        private HRinfoEntities database = new HRinfoEntities();
        int? workerId;

        private void GetEmployeeId()
        {
            // TODO: ogarnąć
            //var userid = HttpContext.Current.User.Identity.GetUserId();
            //var username = HttpContext.Current.User.Identity.GetUserName();
            //var result = database.AspNetUsers.Where(a => a.Email == username).Single().EmployeeId;
            //var result2 = database.AspNetUsers.Where(a => a.Id == username).Single().EmployeeId;
            //workerId = database.AspNetUsers.Where(a => a.Id == User.Identity.GetUserId()).Select(a => a.EmployeeId).Single();

            //if (result == null)
            //    workerId = result2;
            //else
            //    workerId = result;

            workerId = 1;
        }

        private IEnumerable<AddressEmployee> GetEmployeeAddress(
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
                         select new AddressEmployee()
                         {
                             Id = a.Id,
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

        private EmployeeContactData GetEmployeeContactData(int employeeID)
        {
            var result = from e in database.Employees
                         join p in database.Positions on e.PositionId equals p.Id
                         join d in database.Departments on p.DepartmentId equals d.Id
                         where e.Id == employeeID
                         select new EmployeeContactData()
                         {
                             FirstName = e.FirstName,
                             LastName = e.LastName,
                             PhoneNumber = e.JobPhone,
                             Email = e.JobEmail,
                             Position = p.Name,
                             Department = d.Name
                         };

            return result.Single();
        }

        private Street AddressStreet(string name)
        {
            Street street = new Street();
            if (String.IsNullOrEmpty(name))
                street.Id = 0;
            else
            {
                street = database.Streets.SingleOrDefault(s => s.Name == name);

                if (street == null)
                {
                    street = new Street();
                    street.Name = name;
                    database.Streets.Add(street);
                    database.SaveChanges();
                }
            }

            return street;
        }

        private City AddressCity(string name)
        {
            City city = new City();
            if (String.IsNullOrEmpty(name))
                city.Id = 0;
            else
            {
                city = database.Cities.SingleOrDefault(c => c.Name == name);

                if (city == null)
                {
                    city = new City();
                    city.Name = name;
                    database.Cities.Add(city);
                    database.SaveChanges();
                }
            }

            return city;
        }

        private Country AddressCountry(string name)
        {
            Country country = new Country();
            if (String.IsNullOrEmpty(name))
                country.Id = 0;
            else
            {
                country = database.Countries.SingleOrDefault(c => c.Name == name);

                if (country == null)
                {
                    country = new Country();
                    country.Name = name;
                    database.Countries.Add(country);
                    database.SaveChanges();
                }
            }

            return country;
        }

        [Route("FindEmployee")]
        [HttpGet]
        public EmployeeContactData FindEmployeeContactData(int employeeId)
        {
            return this.GetEmployeeContactData(employeeId);
        }

        [Route("FindEmployee")]
        public IEnumerable<EmployeeContactData> FindByEmployeeWorker(EmployeeWorker employeeWorker)
        {
            var result = from e in database.Employees
                         join p in database.Positions on e.PositionId equals p.Id
                         join d in database.Departments on p.DepartmentId equals d.Id
                         where (e.FirstName == employeeWorker.FirstName || String.IsNullOrEmpty(employeeWorker.FirstName))
                            && (e.LastName == employeeWorker.LastName || String.IsNullOrEmpty(employeeWorker.LastName))
                            && (p.Name == employeeWorker.Position || String.IsNullOrEmpty(employeeWorker.Position))
                            && (d.Name == employeeWorker.Department || String.IsNullOrEmpty(employeeWorker.Department))
                         select new EmployeeContactData()
                         {
                             FirstName = e.FirstName,
                             LastName = e.LastName,
                             PhoneNumber = e.JobPhone,
                             Email = e.JobEmail,
                             Position = p.Name,
                             Department = d.Name
                         };

            return result.ToList();
        }

        [Route("ContactData")]
        public EmployeeContactData CurrentUserContactData()
        {
            this.GetEmployeeId();
            return this.GetEmployeeContactData((int)workerId);
        }

        [Route("Address")]
        public IEnumerable<AddressEmployee> CurrentUserAddresses()
        {
            this.GetEmployeeId();

            return this.GetEmployeeAddress();
        }

        [Route("Address")]
        [HttpGet]
        public AddressEmployee CurrentUserAddress(int addressTypeId)
        {
            this.GetEmployeeId();

            return this.GetEmployeeAddress(null, addressTypeId).Single();
        }

        [Route("Address")]
        [HttpGet]
        public AddressEmployee CurrentUserAddress(string addressTypeName)
        {
            this.GetEmployeeId();

            return this.GetEmployeeAddress(null, null, addressTypeName).Single();
        }

        [Route("Salaries")]
        public IEnumerable<EmployeeSalaries> Salaries()
        {
            this.GetEmployeeId();

            var result = from e in database.Employees
                         join p in database.PayOffs on e.Id equals p.EmployeeId
                         where e.Id == workerId
                         orderby p.Date descending
                         select new EmployeeSalaries { Amount = p.Amount, Date = p.Date };

            return result.ToList();
        }

        [Route("PrivateData")]
        public EmployeeData PrivateData()
        {
            this.GetEmployeeId();

            var result = from e in database.Employees
                         where e.Id == workerId
                         select new EmployeeData()
                         {
                             FirstName = e.FirstName,
                             LastName = e.LastName,
                             PhoneNumber = e.JobPhone,
                             Email = e.JobEmail,
                             SalaryFee = e.SalaryFee,
                             PrivatePhoneNumber = e.PhoneNumber,
                             PrivateEmail = e.Email,
                             HolidayDaysTotal = e.HolidayDaysTotal,
                             UsedHolidayDays = e.UsedHolidayDays
                         };

            return result.Single();
        }

        [Route("PrivateData")]
        [HttpPut]
        public async Task<IHttpActionResult> PutPersonalData(EmployeeData employeeData)
        {
            this.GetEmployeeId();

            Employee employee = database.Employees.Single(e => e.Id == workerId);
            database.Entry(employee).State = EntityState.Modified;
            if (employee.Email != employeeData.PrivateEmail)
                employee.Email = employeeData.PrivateEmail;
            if (employee.PhoneNumber != employeeData.PrivatePhoneNumber)
                employee.PhoneNumber = employeeData.PrivatePhoneNumber;
            if (employee.JobPhone != employeeData.PhoneNumber)
                employee.JobPhone = employeeData.PhoneNumber;
            await database.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("Address")]
        [HttpPut]
        public async Task<IHttpActionResult> PutAddress(AddressEmployee address)
        {
            Street street = this.AddressStreet(address.Street);
            City city = this.AddressCity(address.City);
            Country country = this.AddressCountry(address.Country);
            
            Address emplAddress = database.Addresses.Single(a => a.Id == address.Id);
            database.Entry(emplAddress).State = EntityState.Modified;
            emplAddress.StreetId = street.Id == 0 ? emplAddress.StreetId : street.Id;
            emplAddress.HouseNumber = String.IsNullOrEmpty(address.HouseNumber) ? emplAddress.HouseNumber : address.HouseNumber;
            emplAddress.FlatNumber = String.IsNullOrEmpty(address.FlatNumber) ? emplAddress.FlatNumber : address.FlatNumber;
            emplAddress.PostalCode = String.IsNullOrEmpty(address.PostalCode) ? emplAddress.PostalCode : address.PostalCode;
            emplAddress.CityId = city.Id == 0 ? emplAddress.CityId : city.Id;
            emplAddress.CountryId = country.Id == 0 ? emplAddress.CountryId : country.Id;
            await database.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("Address")]
        [HttpPost]
        public async Task<IHttpActionResult> PostAddress(AddressEmployee addressEmployee)
        {
            this.GetEmployeeId();

            Address address = new Address();
            address.StreetId = this.AddressStreet(addressEmployee.Street).Id;
            address.HouseNumber = addressEmployee.HouseNumber;
            address.FlatNumber = addressEmployee.FlatNumber;
            address.PostalCode = addressEmployee.PostalCode;
            address.CityId = this.AddressCity(addressEmployee.City).Id;
            address.CountryId = this.AddressCity(addressEmployee.Country).Id;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            database.Addresses.Add(address);
            await database.SaveChangesAsync();

            EmployeesAddress employeesAddress = new EmployeesAddress();
            employeesAddress.EmployeeId = (int)workerId;
            employeesAddress.AddressId = address.Id;
            employeesAddress.AddressTypeId = database.AddressTypes.Single(t => t.Name == addressEmployee.AddressType).Id;

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            database.EmployeesAddresses.Add(employeesAddress);
            await database.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("Address")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAddress(EmployeeAddressType employeeAddressType)
        {
            this.GetEmployeeId();

            EmployeesAddress employeesAddress = await database.EmployeesAddresses.FirstAsync(e => e.AddressId == employeeAddressType.AddressId && e.EmployeeId == workerId && e.AddressTypeId == employeeAddressType.AddressTypeId);
            if (employeesAddress == null)
                return NotFound();

            Address address = await database.Addresses.FirstAsync(a => a.Id == employeeAddressType.AddressId);
            if (address == null)
                return NotFound();

            database.EmployeesAddresses.Remove(employeesAddress);
            database.Addresses.Remove(address);
            await database.SaveChangesAsync();
            
            return StatusCode(HttpStatusCode.OK);
        }
    }
}
