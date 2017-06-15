using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using HRinfoAPI.Models;

namespace HRinfoAPI.Controllers
{
    [Authorize]
    public class PayOffsController : ApiController
    {
        private HRinfoEntities db = new HRinfoEntities();

        // GET: api/PayOffs
        public IQueryable<PayOff> GetPayOffs()
        {
            return db.PayOffs;
        }

        // GET: api/PayOffs/5
        [ResponseType(typeof(PayOff))]
        public async Task<IHttpActionResult> GetPayOff(int id)
        {
            PayOff payOff = await db.PayOffs.FindAsync(id);
            if (payOff == null)
            {
                return NotFound();
            }

            return Ok(payOff);
        }

        // PUT: api/PayOffs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPayOff(int id, PayOff payOff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != payOff.Id)
            {
                return BadRequest();
            }

            db.Entry(payOff).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PayOffExists(id))
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

        // POST: api/PayOffs
        [ResponseType(typeof(PayOff))]
        public async Task<IHttpActionResult> PostPayOff(PayOff payOff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PayOffs.Add(payOff);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = payOff.Id }, payOff);
        }

        // DELETE: api/PayOffs/5
        [ResponseType(typeof(PayOff))]
        public async Task<IHttpActionResult> DeletePayOff(int id)
        {
            PayOff payOff = await db.PayOffs.FindAsync(id);
            if (payOff == null)
            {
                return NotFound();
            }

            db.PayOffs.Remove(payOff);
            await db.SaveChangesAsync();

            return Ok(payOff);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PayOffExists(int id)
        {
            return db.PayOffs.Count(e => e.Id == id) > 0;
        }
    }
}