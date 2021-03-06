//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRinfoAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Calendar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Calendar()
        {
            this.Calendars1 = new HashSet<Calendar>();
            this.WorkTimes = new HashSet<WorkTime>();
            this.Departments = new HashSet<Department>();
            this.Positions = new HashSet<Position>();
        }
    
        public int Id { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<int> CompanyCalendarId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.DateTime DateFrom { get; set; }
        public Nullable<System.TimeSpan> TimeFrom { get; set; }
        public System.DateTime DateTo { get; set; }
        public Nullable<System.TimeSpan> TimeTo { get; set; }
        public Nullable<byte> WorkDaysNumber { get; set; }
        public int StatusId { get; set; }
        public int EventId { get; set; }
        public bool PositionRestriction { get; set; }
        public bool DepartmentRestriction { get; set; }
        public bool IsActive { get; set; }
        public Nullable<short> ParticipantTotalNumber { get; set; }
        public Nullable<short> ParticipantAvailableNumber { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Calendar> Calendars1 { get; set; }
        public virtual Calendar Calendar1 { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Event Event { get; set; }
        public virtual Status Status { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkTime> WorkTimes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Department> Departments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Position> Positions { get; set; }
    }
}
