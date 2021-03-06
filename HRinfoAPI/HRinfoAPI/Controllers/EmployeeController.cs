﻿using System;
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
    // Rozwiązanie problemu z lokalnym przekazywaniem danych (cross-domain)
    // W celu rozwiązania problemu Cross-Domain informacje znajują się pod hasłem:
    // wab api2 cross domain
    // i są dostępne pod adresme:
    // https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/enabling-cross-origin-requests-in-web-api
    
    [Authorize]
    //[EnableCors(origins: "http://localhost", headers: "application/json", methods: "GET")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Employee")]
    public class EmployeeController : ApiController
    {
        private HRinfoEntities database = new HRinfoEntities();
        int? workerId;

        private void GetEmployeeId()
        {
            var id = User.Identity.GetUserId();
            workerId = database.AspNetUsers.Where(a => a.Id == id).Select(a => a.EmployeeId).Single();
            
            if (!workerId.HasValue)
            {
                var username = User.Identity.Name;
                workerId = database.AspNetUsers.Where(a => a.Email == username).Select(a => a.EmployeeId).Single();
            }
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
                         where (e.FirstName.Contains(employeeWorker.FirstName) || String.IsNullOrEmpty(employeeWorker.FirstName))
                            && (e.LastName.Contains(employeeWorker.LastName) || String.IsNullOrEmpty(employeeWorker.LastName))
                            && (p.Name.Contains(employeeWorker.Position) || String.IsNullOrEmpty(employeeWorker.Position))
                            && (d.Name.Contains(employeeWorker.Department) || String.IsNullOrEmpty(employeeWorker.Department))
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

        [Route("Salaries")]
        public IEnumerable<EmployeeSalaries> Salaries(int number = 5)
        {
            this.GetEmployeeId();

            var result = (from e in database.Employees
                          join p in database.PayOffs on e.Id equals p.EmployeeId
                          where e.Id == workerId
                          orderby p.Date descending
                          select p).Take(number);

            return result.Select(p => new EmployeeSalaries { Amount = p.Amount, Date = p.Date }).ToList();
        }

        [Route("PositionsHistory")]
        public IEnumerable<HistoryPositions> Positions()
        {
            this.GetEmployeeId();

            var result = from h in database.EmployeePositionsHistories
                         join e in database.Employees on h.EmployeeId equals e.Id
                         join p in database.Positions on h.PositionId equals p.Id
                         orderby h.DateFrom descending, h.DateTo descending
                         select new HistoryPositions
                         {
                             PositionId = p.Id,
                             Position = p.Name,
                             DateFrom = h.DateFrom,
                             DateTo = h.DateTo,
                             Salary = h.Salary
                         };

            return result.ToList();
        }

        [Route("PrivateData")]
        public EmployeeData PrivateData()
        {
            this.GetEmployeeId();

            var result = from e in database.Employees
                         join p in database.Positions on e.PositionId equals p.Id
                         join d in database.Departments on p.DepartmentId equals d.Id
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
                             UsedHolidayDays = e.UsedHolidayDays,
                             Department = d.Name,
                             Position = p.Name
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
            await database.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("Address")]
        [HttpGet]
        public IEnumerable<AddressEmployee> CurrentUserAddresses()
        {
            this.GetEmployeeId();

            return this.GetEmployeeAddress().OrderBy(a => a.AddressType);
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
