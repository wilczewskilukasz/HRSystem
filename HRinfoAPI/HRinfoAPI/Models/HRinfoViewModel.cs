using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRinfoAPI.Models
{
    public class HRinfoViewModel
    {
    }

    public class AddressEmployee
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string FlatNumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string AddressType { get; set; }
    }

    public class EmployeeAddressType
    {
        public int AddressId { get; set; }
        public int AddressTypeId { get; set; }
    }

    public class EmployeeContactData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
    }

    public class EmployeeData : EmployeeContactData
    {
        public decimal? SalaryFee { get; set; }
        public string PrivatePhoneNumber { get; set; }
        public string PrivateEmail { get; set; }
        public short HolidayDaysTotal { get; set; }
        public short UsedHolidayDays { get; set; }
    }

    public class EmployeePutData
    {
        public string PhoneNumber { get; set; }
        public string PrivatePhoneNumber { get; set; }
        public string PrivateEmail { get; set; }
    }

    public class EmployeeSalaries
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }

    public class NewsResults
    {
        public string ResultNews { get; set; }
        public DateTime? ResultDateFrom { get; set; }
        public DateTime? ResultDateTo { get; set; }
    }
}