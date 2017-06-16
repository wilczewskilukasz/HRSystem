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
    //[Authorize]
    [RoutePrefix("api/Dictionary")]
    public class DictionaryController : ApiController
    {
        HRinfoEntities db = new HRinfoEntities();

        [Route("Status")]
        [HttpGet]
        public IQueryable<DictionaryStatus> GetStatus()
        {
            return db.Status.Select(s => new DictionaryStatus() { Name = s.Name, Id = s.Id, OrderPosition = s.OrderPosition, EventId = s.EventId });
        }
        
        [Route("Status")]
        [HttpGet]
        public IQueryable<DictionaryStatus> GetStatus(int id)
        {
            return db.Status.Where(s => s.Id == id).Select(s => new DictionaryStatus() { Name = s.Name, Id = s.Id, OrderPosition = s.OrderPosition, EventId = s.EventId });
        }

        [Route("Status")]
        [HttpPost]
        public async Task<IHttpActionResult> PostStatus(DictionaryStatus ds)
        {
            Status status = new Status();
            status.Name = ds.Name;
            status.OrderPosition = ds.OrderPosition;
            status.EventId = ds.EventId;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Status.Add(status);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("Status")]
        [HttpPut]
        public async Task<IHttpActionResult> PutStatus(DictionaryStatus ds)
        {
            Status status = db.Status.Single(s => s.Id == ds.Id);

            if (status == null)
                return BadRequest();

            db.Entry(status).State = EntityState.Modified;
            if (!String.IsNullOrEmpty(ds.Name))
                status.Name = ds.Name;
            if (ds.OrderPosition >= 0)
                status.OrderPosition = ds.OrderPosition;
            if (ds.EventId > 0)
                status.EventId = ds.EventId;

            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("Status")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteStatus(int id)
        {
            Status status = await db.Status.FindAsync(id);

            if (status == null)
                return NotFound();

            db.Status.Remove(status);
            await db.SaveChangesAsync();

            return Ok(status);
        }

        [Route("SolutionBase")]
        [HttpGet]
        public IQueryable<DictionaryBasic> GetSolutionBase()
        {
            return db.SolutionBases.Select(s => new DictionaryBasic() { Name = s.Name, Id = s.Id });
        }

        [Route("SolutionBase")]
        [HttpGet]
        public IQueryable<DictionaryBasic> GetSolutionBase(int id)
        {
            return db.SolutionBases.Where(s => s.Id == id).Select(s => new DictionaryBasic() { Name = s.Name, Id = s.Id });
        }

        [Route("SolutionBase")]
        [HttpPost]
        public async Task<IHttpActionResult> PostSolutionBase(DictionaryBasic ds)
        {
            SolutionBase solutionBase = new SolutionBase();
            solutionBase.Name = ds.Name;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.SolutionBases.Add(solutionBase);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("SolutionBase")]
        [HttpPut]
        public async Task<IHttpActionResult> PutSolutionBase(DictionaryBasic ds)
        {
            SolutionBase solutionBase = db.SolutionBases.Single(s => s.Id == ds.Id);

            if (solutionBase == null)
                return BadRequest();

            db.Entry(solutionBase).State = EntityState.Modified;
            if (!String.IsNullOrEmpty(ds.Name))
                solutionBase.Name = ds.Name;

            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("SolutionBase")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteSolutionBase(int id)
        {
            SolutionBase solutionBase = await db.SolutionBases.FindAsync(id);

            if (solutionBase == null)
                return NotFound();

            db.SolutionBases.Remove(solutionBase);
            await db.SaveChangesAsync();

            return Ok(solutionBase);
        }

        [Route("Event")]
        [HttpGet]
        public IQueryable<DictionaryEvent> GetEvent()
        {
            return db.Events.Select(s => new DictionaryEvent() { Name = s.Name, Id = s.Id, SolutionBaseId = s.SolutionBaseId });
        }

        [Route("Event")]
        [HttpGet]
        public IQueryable<DictionaryEvent> GetEvent(int id)
        {
            return db.Events.Where(s => s.Id == id).Select(s => new DictionaryEvent() { Name = s.Name, Id = s.Id, SolutionBaseId = s.SolutionBaseId });
        }

        [Route("Event")]
        [HttpPost]
        public async Task<IHttpActionResult> PostEvent(DictionaryEvent ds)
        {
            Event mEvent = new Event();
            mEvent.Name = ds.Name;
            mEvent.SolutionBaseId = ds.SolutionBaseId;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Events.Add(mEvent);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("Event")]
        [HttpPut]
        public async Task<IHttpActionResult> PutEvent(DictionaryEvent ds)
        {
            Event mEvent = db.Events.Single(s => s.Id == ds.Id);

            if (mEvent == null)
                return BadRequest();

            db.Entry(mEvent).State = EntityState.Modified;
            if (!String.IsNullOrEmpty(ds.Name))
                mEvent.Name = ds.Name;
            if (ds.SolutionBaseId >= 0)
                mEvent.SolutionBaseId = ds.SolutionBaseId;

            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("Event")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteEvent(int id)
        {
            Event mEvent = await db.Events.FindAsync(id);

            if (mEvent == null)
                return NotFound();

            db.Events.Remove(mEvent);
            await db.SaveChangesAsync();

            return Ok(mEvent);
        }

        [Route("Position")]
        [HttpGet]
        public IQueryable<DictionaryPosition> GetPosition()
        {
            return db.Positions.Select(s => new DictionaryPosition() { Id = s.Id, Name = s.Name, PositionCode = s.PositionCode, DepartmentId = s.DepartmentId, UpPositionId = s.UpPositionId });
        }

        [Route("Position")]
        [HttpGet]
        public IQueryable<DictionaryPosition> GetPosition(int id)
        {
            return db.Positions.Where(s => s.Id == id).Select(s => new DictionaryPosition() { Id = s.Id, Name = s.Name, PositionCode = s.PositionCode, DepartmentId = s.DepartmentId, UpPositionId = s.UpPositionId });
        }

        [Route("Position")]
        [HttpPost]
        public async Task<IHttpActionResult> PostPosition(DictionaryPosition ds)
        {
            Position position = new Position();
            position.Name = ds.Name;
            position.PositionCode = ds.PositionCode;
            position.DepartmentId = ds.DepartmentId;
            position.UpPositionId = ds.UpPositionId;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Positions.Add(position);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("Position")]
        [HttpPut]
        public async Task<IHttpActionResult> PutPosition(DictionaryPosition ds)
        {
            Position position = db.Positions.Single(s => s.Id == ds.Id);

            if (position == null)
                return BadRequest();

            db.Entry(position).State = EntityState.Modified;
            if (!String.IsNullOrEmpty(ds.Name))
                position.Name = ds.Name;
            if (!String.IsNullOrEmpty(ds.PositionCode))
                position.PositionCode = ds.PositionCode;
            if (ds.DepartmentId > 0)
                position.DepartmentId = ds.DepartmentId;
            if (ds.UpPositionId > 0)
                position.UpPositionId = ds.UpPositionId;

            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("Position")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeletePosition(int id)
        {
            Position position = await db.Positions.FindAsync(id);

            if (position == null)
                return NotFound();

            db.Positions.Remove(position);
            await db.SaveChangesAsync();

            return Ok(position);
        }

        [Route("Department")]
        [HttpGet]
        public IQueryable<DictionaryDepartment> GetDepartment()
        {
            return db.Departments.Select(s => new DictionaryDepartment() { Name = s.Name, Id = s.Id, DepartmentCode = s.DepartmentCode, UpDepartmentId = s.UpDepartmentId });
        }

        [Route("Department")]
        [HttpGet]
        public IQueryable<DictionaryDepartment> GetDepartment(int id)
        {
            return db.Departments.Where(s => s.Id == id).Select(s => new DictionaryDepartment() { Name = s.Name, Id = s.Id, DepartmentCode = s.DepartmentCode, UpDepartmentId = s.UpDepartmentId });
        }

        [Route("Department")]
        [HttpPost]
        public async Task<IHttpActionResult> PostDepartment(DictionaryDepartment ds)
        {
            Department department = new Department();
            department.Name = ds.Name;
            department.DepartmentCode = ds.DepartmentCode;
            department.UpDepartmentId = ds.UpDepartmentId;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Departments.Add(department);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("Department")]
        [HttpPut]
        public async Task<IHttpActionResult> PutDepartment(DictionaryDepartment ds)
        {
            Department department = db.Departments.Single(s => s.Id == ds.Id);

            if (department == null)
                return BadRequest();

            db.Entry(department).State = EntityState.Modified;
            if (!String.IsNullOrEmpty(ds.Name))
                department.Name = ds.Name;
            if (!String.IsNullOrEmpty(ds.DepartmentCode))
                department.DepartmentCode = ds.DepartmentCode;
            if (ds.UpDepartmentId > 0)
                department.UpDepartmentId = ds.UpDepartmentId;

            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("Department")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteDepartment(int id)
        {
            Department department = await db.Departments.FindAsync(id);

            if (department == null)
                return NotFound();

            db.Departments.Remove(department);
            await db.SaveChangesAsync();

            return Ok(department);
        }

        [Route("AddressType")]
        [HttpGet]
        public IQueryable<DictionaryBasic> GetAddressType()
        {
            return db.AddressTypes.Select(s => new DictionaryBasic() { Name = s.Name, Id = s.Id });
        }

        [Route("AddressType")]
        [HttpGet]
        public IQueryable<DictionaryBasic> GetAddressType(int id)
        {
            return db.AddressTypes.Where(s => s.Id == id).Select(s => new DictionaryBasic() { Name = s.Name, Id = s.Id });
        }

        [Route("AddressType")]
        [HttpPost]
        public async Task<IHttpActionResult> PostAddressType(DictionaryBasic ds)
        {
            AddressType addressType = new AddressType();
            addressType.Name = ds.Name;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.AddressTypes.Add(addressType);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("Street")]
        [HttpGet]
        public IQueryable<DictionaryBasic> GetStreet()
        {
            return db.Streets.Select(s => new DictionaryBasic() { Name = s.Name, Id = s.Id });
        }

        [Route("Street")]
        [HttpGet]
        public IQueryable<DictionaryBasic> GetStreet(int id)
        {
            return db.Streets.Where(s => s.Id == id).Select(s => new DictionaryBasic() { Name = s.Name, Id = s.Id });
        }

        [Route("Street")]
        [HttpPost]
        public async Task<IHttpActionResult> PostStreet(DictionaryBasic ds)
        {
            Street street = new Street();
            street.Name = ds.Name;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Streets.Add(street);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("City")]
        [HttpGet]
        public IQueryable<DictionaryBasic> GetCity()
        {
            return db.Cities.Select(s => new DictionaryBasic() { Name = s.Name, Id = s.Id });
        }

        [Route("City")]
        [HttpGet]
        public IQueryable<DictionaryBasic> GetCity(int id)
        {
            return db.Cities.Where(s => s.Id == id).Select(s => new DictionaryBasic() { Name = s.Name, Id = s.Id });
        }

        [Route("City")]
        [HttpPost]
        public async Task<IHttpActionResult> PostCity(DictionaryBasic ds)
        {
            City city = new City();
            city.Name = ds.Name;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Cities.Add(city);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("Country")]
        [HttpGet]
        public IQueryable<DictionaryBasic> GetCountry()
        {
            return db.Countries.Select(s => new DictionaryBasic() { Name = s.Name, Id = s.Id });
        }

        [Route("Country")]
        [HttpGet]
        public IQueryable<DictionaryBasic> GetCountry(int id)
        {
            return db.Countries.Where(s => s.Id == id).Select(s => new DictionaryBasic() { Name = s.Name, Id = s.Id });
        }

        [Route("Country")]
        [HttpPost]
        public async Task<IHttpActionResult> PostCountry(DictionaryBasic ds)
        {
            Country country = new Country();
            country.Name = ds.Name;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Countries.Add(country);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }

        [Route("Topic")]
        [HttpGet]
        public IQueryable<DictionaryTopics> GetTopic()
        {
            return db.Topics.Select(s => new DictionaryTopics() { Name = s.Name, Id = s.Id });
        }

        [Route("Topic")]
        [HttpGet]
        public IQueryable<DictionaryTopics> GetTopic(int id)
        {
            return db.Topics.Where(s => s.Id == id).Select(s => new DictionaryTopics() { Name = s.Name, Id = s.Id });
        }

        [Route("Topic")]
        [HttpPost]
        public async Task<IHttpActionResult> PostTopic(DictionaryTopics ds)
        {
            Topic topic = new Topic();
            topic.Name = ds.Name;
            if (ds.DepartmentId > 0)
                topic.DepartmentId = ds.DepartmentId;
            if (ds.PositionId > 0)
                topic.PositionId = ds.PositionId;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Topics.Add(topic);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
        }
    }
}
