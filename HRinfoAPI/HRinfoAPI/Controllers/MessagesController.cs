using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using HRinfoAPI.Models;
using Microsoft.AspNet.Identity;

namespace HRinfoAPI.Controllers
{
    //[Authorize]
    [RoutePrefix("Messages")]
    public class MessagesController : ApiController
    {
        private HRinfoEntities db = new HRinfoEntities();
        int? workerId;

        private void GetEmployeeId()
        {
            workerId = db.AspNetUsers.Where(a => a.Id == User.Identity.GetUserId()).Select(a => a.EmployeeId).SingleOrDefault();
        }

        // GET: api/PrivateMessages
        public IQueryable<PrivateMessage> GetPrivateMessages()
        {
            this.GetEmployeeId();

            var result = (from p in db.PrivateMessages
                          where p.EmployeeId == workerId
                          select p)
                         .Union(from r in db.PrivateMessages
                                join p in db.PrivateMessages on r.RequestId equals p.Id
                                where p.EmployeeId == workerId
                                select r)
                         .Union(from r in db.PrivateMessages
                                join p in db.PrivateMessages on r.ResponseId equals p.Id
                                where p.EmployeeId == workerId
                                select r);

            return result.OrderByDescending(r => r.Id);
        }

        // GET: api/PrivateMessages/5
        [ResponseType(typeof(PrivateMessage))]
        public async Task<IHttpActionResult> GetPrivateMessage(int id)
        {
            PrivateMessage privateMessage = await db.PrivateMessages.FindAsync(id);
            if (privateMessage == null)
            {
                return NotFound();
            }

            return Ok(privateMessage);
        }

        // PUT: api/PrivateMessages/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPrivateMessage(int id, PrivateMessage privateMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != privateMessage.Id)
            {
                return BadRequest();
            }

            db.Entry(privateMessage).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrivateMessageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PrivateMessages
        [ResponseType(typeof(PrivateMessage))]
        public async Task<IHttpActionResult> PostPrivateMessage(PrivateMessage privateMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            this.GetEmployeeId();
            privateMessage.EmployeeId = (int)workerId;
            db.PrivateMessages.Add(privateMessage);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = privateMessage.Id }, privateMessage);
        }

        // DELETE: api/PrivateMessages/5
        [ResponseType(typeof(PrivateMessage))]
        public async Task<IHttpActionResult> DeletePrivateMessage(int id)
        {
            PrivateMessage privateMessage = await db.PrivateMessages.FindAsync(id);
            if (privateMessage == null)
            {
                return NotFound();
            }

            db.PrivateMessages.Remove(privateMessage);
            await db.SaveChangesAsync();

            return Ok(privateMessage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PrivateMessageExists(int id)
        {
            return db.PrivateMessages.Count(e => e.Id == id) > 0;
        }
    }
}