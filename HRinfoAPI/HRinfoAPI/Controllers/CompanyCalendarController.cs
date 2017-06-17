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

namespace HRinfoAPI.Controllers
{
    // TODO: uncoment
    //[Authorize]
    [RoutePrefix("api/Calendar")]
    public class CompanyCalendarController : ApiController
    {
        HRinfoEntities db = new HRinfoEntities();

        [Route("Company")]
        [HttpGet]
        public IEnumerable<CompanyCalendar> GetCompany()
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
        public CompanyCalendar GetCompany(bool? isActive = null, int id = 0, string name = "", int eventId = 0)
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

        [Route("Company")]
        [HttpPut]
        public async Task<IHttpActionResult> PutCompany(CompanyCalendar cc)
        {
            Calendar calendar = db.Calendars.Single(c => c.Id == cc.Id);

            if (calendar == null)
                return NotFound();

            db.Entry(calendar).State = EntityState.Modified;
            calendar.Name = cc.Name;
            calendar.Description = cc.Description;
            calendar.DateFrom = cc.DateFrom;
            calendar.TimeFrom = cc.TimeFrom;
            calendar.DateTo = cc.DateTo;
            calendar.TimeTo = cc.TimeTo;
            calendar.WorkDaysNumber = cc.WorkDaysNumber;
            calendar.StatusId = cc.StatusId;
            calendar.EventId = cc.EventId;
            calendar.PositionRestriction = cc.PositionRestriction;
            calendar.DepartmentRestriction = cc.DepartmentRestriction;
            calendar.IsActive = cc.IsActive;
            calendar.ParticipantTotalNumber = cc.ParticipantTotalNumber;
            calendar.ParticipantAvailableNumber = cc.ParticipantAvailableNumber;
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("Company")]
        [HttpPost]
        public async Task<IHttpActionResult> PostCompany(CompanyCalendar cc)
        {
            Calendar calendar = new Calendar();
            calendar.Name = cc.Name;
            calendar.Description = cc.Description;
            calendar.DateFrom = cc.DateFrom;
            calendar.TimeFrom = cc.TimeFrom;
            calendar.DateTo = cc.DateTo;
            calendar.TimeTo = cc.TimeTo;
            calendar.WorkDaysNumber = cc.WorkDaysNumber;
            if (db.Events.Single(e => e.Id == cc.EventId).Id > 0)
                calendar.EventId = cc.EventId;
            else
            {
                var statusId = db.Events.Single(e => e.Name == cc.EventName).Id;
                if (statusId > 0)
                    calendar.EventId = statusId;
            }
                
            if (db.Status.Single(s => s.Id == cc.StatusId && s.EventId == calendar.EventId).Id > 0)
                calendar.StatusId = cc.StatusId;
            else
            {
                var eventId = db.Status.Single(s => s.Name == cc.StatusName && s.EventId == calendar.EventId).Id;
                if (eventId > 0)
                    calendar.StatusId = eventId;
            }
            calendar.PositionRestriction = cc.PositionRestriction;
            calendar.DepartmentRestriction = cc.DepartmentRestriction;
            calendar.IsActive = cc.IsActive;
            calendar.ParticipantTotalNumber = cc.ParticipantTotalNumber;
            calendar.ParticipantAvailableNumber = cc.ParticipantAvailableNumber;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Calendars.Add(calendar);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("Company")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteCompany(CompanyCalendar cc)
        {
            Calendar calendar = db.Calendars.Single(c => c.Id == cc.Id);
            
            if (calendar == null)
                return NotFound();

            db.Calendars.Remove(calendar);
            await db.SaveChangesAsync();

            return Ok(calendar);
        }
    }
}
