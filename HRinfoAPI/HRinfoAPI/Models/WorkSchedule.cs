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
    
    public partial class WorkSchedule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WorkSchedule()
        {
            this.WorkScheduleExceptions = new HashSet<WorkScheduleException>();
        }
    
        public int Id { get; set; }
        public int DayId { get; set; }
        public bool WorkDay { get; set; }
        public System.TimeSpan TimeFrom { get; set; }
        public System.TimeSpan TimeTo { get; set; }
        public int StatusId { get; set; }
    
        public virtual Day Day { get; set; }
        public virtual Status Status { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkScheduleException> WorkScheduleExceptions { get; set; }
    }
}
