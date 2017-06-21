using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Http;
using HRinfoAPI.Models;
using System.Web.Http.Cors;

namespace HRinfoAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Calendar")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CompanyCalendarController : ApiController
    {
        HRinfoEntities db = new HRinfoEntities();

        [Route("Company")]
        [HttpGet]
        public IEnumerable<CompanyCalendar> GetCalendar()
        {
            var result = from c in db.Calendars
                         where c.IsActive == true
                            && c.EmployeeId == null
                         orderby c.DateFrom,
                            c.TimeFrom,
                            c.DateTo,
                            c.TimeTo
                         select new CompanyCalendar() {
                             Id = c.Id,
                             Name = c.Name,
                             Description = c.Description,
                             DateFrom = c.DateFrom,
                             TimeFrom = c.TimeFrom,
                             DateTo = c.DateTo,
                             TimeTo = c.TimeTo,
                             WorkDaysNumber = c.WorkDaysNumber,
                             StatusId = c.StatusId,
                             StatusName = db.Status.Single(s => s.Id == c.StatusId).Name,
                             EventId = c.EventId,
                             EventName = db.Events.Single(e => e.Id == c.EventId).Name,
                             PositionRestriction = c.PositionRestriction,
                             DepartmentRestriction = c.DepartmentRestriction,
                             IsActive = c.IsActive,
                             ParticipantTotalNumber = c.ParticipantTotalNumber,
                             ParticipantAvailableNumber = c.ParticipantAvailableNumber
                         };

            return result.ToList();
        }

        [Route("Company")]
        [HttpGet]
        public CompanyCalendar GetCalendar(bool? isActive = null, int id = 0, string name = "", int eventId = 0)
        {
            var result = from c in db.Calendars
                         where (isActive == null || c.IsActive == isActive)
                            && (id == 0 || c.Id == id)
                            && (String.IsNullOrEmpty(name) || String.Equals(c.Name, name))
                            && (eventId == 0 || c.EventId == eventId)
                            && c.EmployeeId == null
                         select new CompanyCalendar()
                         {
                             Id = c.Id,
                             Name = c.Name,
                             Description = c.Description,
                             DateFrom = c.DateFrom,
                             TimeFrom = c.TimeFrom,
                             DateTo = c.DateTo,
                             TimeTo = c.TimeTo,
                             WorkDaysNumber = c.WorkDaysNumber,
                             StatusId = c.StatusId,
                             StatusName = db.Status.Single(s => s.Id == c.StatusId).Name,
                             EventId = c.EventId,
                             EventName = db.Events.Single(e => e.Id == c.EventId).Name,
                             PositionRestriction = c.PositionRestriction,
                             DepartmentRestriction = c.DepartmentRestriction,
                             IsActive = c.IsActive,
                             ParticipantTotalNumber = c.ParticipantTotalNumber,
                             ParticipantAvailableNumber = c.ParticipantAvailableNumber
                         };

            return result.First();
        }

        [Route("Company/PositionRestriction")]
        public IEnumerable<dynamic> GetCalendarPositionRestriction(int id)
        {
            return db.Calendars.Where(c => c.Id == id).Select(c => c.Positions).ToList();
        }

        [Route("Company/DepartmentRestriction")]
        public IEnumerable<dynamic> GetCalendarDepartmentRestriction(int id)
        {
            return db.Calendars.Where(c => c.Id == id).Select(c => c.Departments).ToList();
        }

        [Route("Company")]
        [HttpPut]
        public async Task<IHttpActionResult> PutCalendar(CompanyCalendar cc)
        {
            Calendar calendar = db.Calendars.Single(c => c.Id == cc.Id);

            if (calendar == null)
                return NotFound();

            db.Entry(calendar).State = EntityState.Modified;
            if (!String.IsNullOrEmpty(cc.Name))
                calendar.Name = cc.Name;
            if (!String.IsNullOrEmpty(cc.Name))
                calendar.Description = cc.Description;
            if (cc.DateFrom != null)
                calendar.DateFrom = (DateTime)cc.DateFrom;
            if (cc.TimeFrom != null)
                calendar.TimeFrom = cc.TimeFrom;
            if (cc.DateTo != null)
                calendar.DateTo = (DateTime)cc.DateTo;
            if (cc.TimeTo != null)
                calendar.TimeTo = cc.TimeTo;
            if (cc.WorkDaysNumber != null)
                calendar.WorkDaysNumber = cc.WorkDaysNumber;
            calendar.StatusId = cc.StatusId;
            calendar.EventId = cc.EventId;
            calendar.PositionRestriction = cc.PositionRestriction;
            calendar.DepartmentRestriction = cc.DepartmentRestriction;
            calendar.IsActive = cc.IsActive;
            if (cc.ParticipantTotalNumber != null)
                calendar.ParticipantTotalNumber = cc.ParticipantTotalNumber;
            if (cc.ParticipantAvailableNumber != null)
                calendar.ParticipantAvailableNumber = cc.ParticipantAvailableNumber;
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("Company")]
        [HttpPost]
        public async Task<int?> PostCalendar(CompanyCalendar cc)
        {
            Calendar calendar = new Calendar();
            if (!String.IsNullOrEmpty(cc.Name))
                calendar.Name = cc.Name;
            if (!String.IsNullOrEmpty(cc.Name))
                calendar.Description = cc.Description;
            if (cc.DateFrom != null)
                calendar.DateFrom = (DateTime)cc.DateFrom;
            if (cc.TimeFrom != null)
                calendar.TimeFrom = cc.TimeFrom;
            if (cc.DateTo != null)
                calendar.DateTo = (DateTime)cc.DateTo;
            if (cc.TimeTo != null)
                calendar.TimeTo = cc.TimeTo;
            if (cc.WorkDaysNumber != null)
                calendar.WorkDaysNumber = cc.WorkDaysNumber;
            if (db.Events.Single(e => e.Id == cc.EventId).Id > 0)
                calendar.EventId = cc.EventId;
            else
            {
                var eventId = db.Events.Single(e => e.Name == cc.EventName).Id;
                if (eventId > 0)
                    calendar.EventId = eventId;
            }
                
            if (db.Status.Single(s => s.Id == cc.StatusId && s.EventId == calendar.EventId).Id > 0)
                calendar.StatusId = cc.StatusId;
            else
            {
                var statusId = db.Status.Single(s => s.Name == cc.StatusName && s.EventId == calendar.EventId).Id;
                if (statusId > 0)
                    calendar.StatusId = statusId;
            }
            calendar.PositionRestriction = cc.PositionRestriction;
            calendar.DepartmentRestriction = cc.DepartmentRestriction;
            calendar.IsActive = cc.IsActive;
            if (cc.ParticipantTotalNumber != null)
                calendar.ParticipantTotalNumber = cc.ParticipantTotalNumber;
            if (cc.ParticipantAvailableNumber != null)
                calendar.ParticipantAvailableNumber = cc.ParticipantAvailableNumber;

            if (!ModelState.IsValid)
                return null;

            db.Calendars.Add(calendar);
            await db.SaveChangesAsync();

            return calendar.Id;
        }

        [Route("Company")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteCalendar(CompanyCalendar cc)
        {
            Calendar calendar = db.Calendars.Single(c => c.Id == cc.Id);
            
            if (calendar == null)
                return NotFound();

            db.Calendars.Remove(calendar);
            await db.SaveChangesAsync();

            return Ok(calendar);
        }

        [Route("Company")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteCalendar(int id)
        {
            Calendar calendar = db.Calendars.Single(c => c.Id == id);

            if (calendar == null)
                return NotFound();

            db.Calendars.Remove(calendar);
            await db.SaveChangesAsync();

            return Ok(calendar);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
