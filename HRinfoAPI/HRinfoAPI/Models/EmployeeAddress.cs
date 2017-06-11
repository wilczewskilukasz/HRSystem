using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRinfoAPI.Models
{
    public class EmployeeAddress
    {
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string FlatNumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string AddressType { get; set; }
    }
}