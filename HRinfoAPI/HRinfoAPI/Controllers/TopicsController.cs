using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using HRinfoAPI.Models;

namespace HRinfoAPI.Controllers
{
    [Authorize]
    public class TopicsController : ApiController
    {
        private HRinfoEntities db = new HRinfoEntities();

        // GET: api/Topics
        public IQueryable<Topic> GetTopics()
        {
            return db.Topics;
        }

        // GET: api/Topics/5
        [ResponseType(typeof(Topic))]
        public async Task<IHttpActionResult> GetTopic(int id)
        {
            Topic topic = await db.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }

            return Ok(topic);
        }

        // PUT: api/Topics/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTopic(int id, Topic topic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != topic.Id)
            {
                return BadRequest();
            }

            db.Entry(topic).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopicExists(id))
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

        // POST: api/Topics
        [ResponseType(typeof(Topic))]
        public async Task<IHttpActionResult> PostTopic(Topic topic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Topics.Add(topic);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = topic.Id }, topic);
        }

        // DELETE: api/Topics/5
        [ResponseType(typeof(Topic))]
        public async Task<IHttpActionResult> DeleteTopic(int id)
        {
            Topic topic = await db.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }

            db.Topics.Remove(topic);
            await db.SaveChangesAsync();

            return Ok(topic);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TopicExists(int id)
        {
            return db.Topics.Count(e => e.Id == id) > 0;
        }
    }
}