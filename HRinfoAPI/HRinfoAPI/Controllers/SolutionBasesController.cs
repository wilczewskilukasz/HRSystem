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
    public class SolutionBasesController : ApiController
    {
        private HRinfoEntities db = new HRinfoEntities();

        // GET: api/SolutionBases
        public IQueryable<SolutionBase> GetSolutionBases()
        {
            return db.SolutionBases;
        }

        // GET: api/SolutionBases/5
        [ResponseType(typeof(SolutionBase))]
        public async Task<IHttpActionResult> GetSolutionBase(int id)
        {
            SolutionBase solutionBase = await db.SolutionBases.FindAsync(id);
            if (solutionBase == null)
            {
                return NotFound();
            }

            return Ok(solutionBase);
        }

        // PUT: api/SolutionBases/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSolutionBase(int id, SolutionBase solutionBase)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != solutionBase.Id)
            {
                return BadRequest();
            }

            db.Entry(solutionBase).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolutionBaseExists(id))
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

        // POST: api/SolutionBases
        [ResponseType(typeof(SolutionBase))]
        public async Task<IHttpActionResult> PostSolutionBase(SolutionBase solutionBase)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SolutionBases.Add(solutionBase);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = solutionBase.Id }, solutionBase);
        }

        // DELETE: api/SolutionBases/5
        [ResponseType(typeof(SolutionBase))]
        public async Task<IHttpActionResult> DeleteSolutionBase(int id)
        {
            SolutionBase solutionBase = await db.SolutionBases.FindAsync(id);
            if (solutionBase == null)
            {
                return NotFound();
            }

            db.SolutionBases.Remove(solutionBase);
            await db.SaveChangesAsync();

            return Ok(solutionBase);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SolutionBaseExists(int id)
        {
            return db.SolutionBases.Count(e => e.Id == id) > 0;
        }
    }
}