using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using HRinfoAPI.Models;
using Microsoft.AspNet.Identity;

namespace HRinfoAPI.Controllers
{
    // TODO: uncoment
    //[Authorize]
    [RoutePrefix("Message")]
    public class MessagesController : ApiController
    {
        private HRinfoEntities db = new HRinfoEntities();
        int? workerId;

        private void GetEmployeeId()
        {
            workerId = db.AspNetUsers.Where(a => a.Id == User.Identity.GetUserId()).Select(a => a.EmployeeId).Single();
        }

        // GET: api/Message
        [HttpGet]
        public IEnumerable<Messages> GetMessages()
        {
            this.GetEmployeeId();

            var result = (from p in db.PrivateMessages
                          where p.EmployeeId == workerId
                          select new Messages() { Id = p.Id, Date = p.DateTime, EmployeeId = p.EmployeeId, Message = p.Message, RequestId = p.RequestId, ResponseEmployeeId = p.ResponseEmployeeId, TopicId = p.TopicId, ResponseId = p.ResponseId })
                         .Union(from p in db.PrivateMessages
                          join r in db.PrivateMessages on p.RequestId equals r.Id
                          where r.EmployeeId == workerId
                          select new Messages() { Id = p.Id, Date = p.DateTime, EmployeeId = p.EmployeeId, Message = p.Message, RequestId = p.RequestId, ResponseEmployeeId = p.ResponseEmployeeId, TopicId = p.TopicId, ResponseId = p.ResponseId })
                         .Union(from p in db.PrivateMessages
                          join r in db.PrivateMessages on p.ResponseId equals r.Id
                          where r.EmployeeId == workerId
                          select new Messages() { Id = p.Id, Date = p.DateTime, EmployeeId = p.EmployeeId, Message = p.Message, RequestId = p.RequestId, ResponseEmployeeId = p.ResponseEmployeeId, TopicId = p.TopicId, ResponseId = p.ResponseId });

            return result.OrderByDescending(p => p.Id);
        }

        // GET: api/Message/5
        [HttpGet]
        public IEnumerable<Messages> GetMessagesById(int messageId)
        {
            var result = db.PrivateMessages.Where(p => p.Id == messageId).Select(p => new Messages() { Id = p.Id, Date = p.DateTime, EmployeeId = p.EmployeeId, Message = p.Message, RequestId = p.RequestId, ResponseEmployeeId = p.ResponseEmployeeId, TopicId = p.TopicId, ResponseId = p.ResponseId });

            return result.OrderByDescending(p => p.Id);
        }

        // GET: api/Message/Employee/5
        [HttpGet]
        [Route("Employee")]
        public IEnumerable<Messages> GetMessagesByEmployee(int employeeId)
        {
            workerId = employeeId;

            var result = (from p in db.PrivateMessages
                          where p.EmployeeId == workerId
                          select new Messages() { Id = p.Id, Date = p.DateTime, EmployeeId = p.EmployeeId, Message = p.Message, RequestId = p.RequestId, ResponseEmployeeId = p.ResponseEmployeeId, TopicId = p.TopicId, ResponseId = p.ResponseId })
                         .Union(from p in db.PrivateMessages
                                join r in db.PrivateMessages on p.RequestId equals r.Id
                                where r.EmployeeId == workerId
                                select new Messages() { Id = p.Id, Date = p.DateTime, EmployeeId = p.EmployeeId, Message = p.Message, RequestId = p.RequestId, ResponseEmployeeId = p.ResponseEmployeeId, TopicId = p.TopicId, ResponseId = p.ResponseId })
                         .Union(from p in db.PrivateMessages
                                join r in db.PrivateMessages on p.ResponseId equals r.Id
                                where r.EmployeeId == workerId
                                select new Messages() { Id = p.Id, Date = p.DateTime, EmployeeId = p.EmployeeId, Message = p.Message, RequestId = p.RequestId, ResponseEmployeeId = p.ResponseEmployeeId, TopicId = p.TopicId, ResponseId = p.ResponseId });

            return result.OrderByDescending(p => p.Id);
        }

        // POST: api/Message
        public async Task<IHttpActionResult> PostMessage(Messages message)
        {
            PrivateMessage privateMessage = new PrivateMessage();
            if (message.EmployeeId > 0)
                privateMessage.EmployeeId = message.EmployeeId;
            else
            {
                this.GetEmployeeId();
                privateMessage.EmployeeId = (int)workerId;
            }
            privateMessage.TopicId = message.TopicId;
            privateMessage.Message = message.Message;
            privateMessage.StatusId = db.Status.Single(s => s.EventId == db.Events.Single(e => e.Name == "Wiadomości").Id).Id;
            if (message.ResponseEmployeeId > 0)
                privateMessage.ResponseEmployeeId = message.ResponseEmployeeId;
            if (message.RequestId > 0)
                privateMessage.RequestId = message.RequestId;
            if (message.ResponseId > 0)
                privateMessage.ResponseId = message.ResponseId;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.PrivateMessages.Add(privateMessage);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.OK);
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