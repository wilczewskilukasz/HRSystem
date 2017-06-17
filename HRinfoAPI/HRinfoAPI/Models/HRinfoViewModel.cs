using System;

namespace HRinfoAPI.Models
{
    public class HRinfoViewModel
    {
    }

    public class CompanyCalendar
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public TimeSpan? TimeFrom { get; set; }
        public DateTime DateTo { get; set; }
        public TimeSpan? TimeTo { get; set; }
        public byte? WorkDaysNumber { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public bool PositionRestriction { get; set; }
        public bool DepartmentRestriction { get; set; }
        public bool IsActive { get; set; }
        public short? ParticipantTotalNumber { get; set; }
        public short? ParticipantAvailableNumber { get; set; }
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

    public class EmployeeWorker
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
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

    public class DictionaryBasic
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DictionaryStatus : DictionaryBasic
    {
        public byte OrderPosition { get; set; }
        public int EventId { get; set; }
    }

    public class DictionaryEvent : DictionaryBasic
    {
        public int SolutionBaseId { get; set; }
    }

    public class DictionaryPosition : DictionaryBasic
    {
        public string PositionCode { get; set; }
        public int DepartmentId { get; set; }
        public int? UpPositionId { get; set; }
    }

    public class DictionaryDepartment : DictionaryBasic
    {
        public string DepartmentCode { get; set; }
        public int? UpDepartmentId { get; set; }
    }

    public class DictionaryTopics : DictionaryBasic
    {
        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }
    }

    public class Salaries
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }

    public class FullSalaries : Salaries
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class Messages
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Employee { get; set; }
        public int TopicId { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public int? ResponseEmployeeId { get; set; }
        public string ResponseEmployee { get; set; }
        public int? RequestId { get; set; }
        public int? ResponseId { get; set; }
    }
}