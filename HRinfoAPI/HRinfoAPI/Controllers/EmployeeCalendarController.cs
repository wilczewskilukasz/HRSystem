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
using System.Web.Http.Cors;

namespace HRinfoAPI.Controllers
{
    // TODO: uncoment
    //[Authorize]
    [RoutePrefix("api/Calendar")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmployeeCalendarController : ApiController
    {
        HRinfoEntities db = new HRinfoEntities();
        int? employeeId;

        // TODO: ogarnąć
        private void GetEmployeeId()
        {
            //employeeId = db.AspNetUsers.Where(a => a.Id == User.Identity.GetUserId()).Select(a => a.EmployeeId).Single();
            employeeId = 1;
        }

        [Route("Employee")]
        [HttpGet]
        public IEnumerable<CompanyCalendar> GetCalendar()
        {
            this.GetEmployeeId();

            var result = from c in db.Calendars
                         where c.IsActive == true
                            && c.EmployeeId == employeeId
                         orderby c.DateFrom,
                            c.TimeFrom,
                            c.DateTo,
                            c.TimeTo
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
                             IsActive = c.IsActive
                         };

            return result.ToList();
        }

        [Route("Employee")]
        [HttpGet]
        public CompanyCalendar GetCalendar(int id)
        {
            var result = from c in db.Calendars
                         where c.Id == id
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
                             IsActive = c.IsActive
                         };

            return result.First();
        }

        [Route("Employee/Short")]
        [HttpGet]
        public IEnumerable<CompanyCalendar> GetShortCalendar()
        {
            this.GetEmployeeId();

            var result = from c in db.Calendars
                         where c.IsActive == true
                            && c.EmployeeId == employeeId
                         orderby c.DateFrom,
                            c.TimeFrom,
                            c.DateTo,
                            c.TimeTo
                         select new CompanyCalendar()
                         {
                             Id = c.Id,
                             Name = c.Name,
                             Description = c.Description,
                             DateFrom = c.DateFrom,
                             DateTo = c.DateTo,
                             WorkDaysNumber = c.WorkDaysNumber,
                             StatusName = db.Status.Single(s => s.Id == c.StatusId).Name,
                             EventName = db.Events.Single(e => e.Id == c.EventId).Name,
                         };

            return result.ToList();
        }

        [Route("Employee/Short")]
        [HttpGet]
        public CompanyCalendar GetShortCalendar(int id)
        {
            var result = from c in db.Calendars
                         where c.Id == id
                         orderby c.DateFrom,
                            c.TimeFrom,
                            c.DateTo,
                            c.TimeTo
                         select new CompanyCalendar()
                         {
                             Id = c.Id,
                             Name = c.Name,
                             Description = c.Description,
                             DateFrom = c.DateFrom,
                             DateTo = c.DateTo,
                             WorkDaysNumber = c.WorkDaysNumber,
                             StatusName = db.Status.Single(s => s.Id == c.StatusId).Name,
                             EventName = db.Events.Single(e => e.Id == c.EventId).Name,
                         };

            return result.First();
        }

        [Route("Employee")]
        [HttpPost]
        public async Task<IHttpActionResult> PostCalendar(BaseCalendar cc)
        {
            this.GetEmployeeId();

            if (employeeId == null)
                return NotFound();

            Calendar calendar = new Calendar();
            calendar.EmployeeId = employeeId;
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
                calendar.StatusId = db.Status.Single(s => s.EventId == calendar.EventId && s.OrderPosition == 0).Id;
            calendar.PositionRestriction = false;
            calendar.DepartmentRestriction = false;
            calendar.IsActive = true;

            if (!ModelState.IsValid)
                return null;

            db.Calendars.Add(calendar);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("Employee")]
        [HttpPut]
        public async Task<IHttpActionResult> PutCalendar(BaseCalendar cc)
        {
            Calendar calendar = db.Calendars.Single(c => c.Id == cc.Id);
            
            if (calendar == null)
                return NotFound();

            if (cc.StatusId != db.Status.Single(s => s.EventId == cc.EventId && s.OrderPosition == 0).Id)
                return BadRequest();

            this.GetEmployeeId();

            if (calendar.EmployeeId != (int)employeeId)
                return BadRequest();

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
                calendar.StatusId = db.Status.Single(s => s.EventId == calendar.EventId && s.OrderPosition == 0).Id;
            calendar.PositionRestriction = false;
            calendar.DepartmentRestriction = false;
            calendar.IsActive = true;
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("Employee")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteCalendar(BaseCalendar cc)
        {
            Calendar calendar = db.Calendars.Single(c => c.Id == cc.Id);

            if (calendar == null)
                return NotFound();

            db.Calendars.Remove(calendar);
            await db.SaveChangesAsync();

            return Ok(calendar);
        }

        [Route("Employee")]
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
