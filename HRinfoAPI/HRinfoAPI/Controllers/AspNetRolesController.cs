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
    public class AspNetRolesController : ApiController
    {
        private HRinfoEntities db = new HRinfoEntities();

        // GET: api/AspNetRoles
        public IQueryable<AspNetRole> GetAspNetRoles()
        {
            return db.AspNetRoles;
        }

        // GET: api/AspNetRoles/5
        [ResponseType(typeof(AspNetRole))]
        public async Task<IHttpActionResult> GetAspNetRole(string id)
        {
            AspNetRole aspNetRole = await db.AspNetRoles.FindAsync(id);
            if (aspNetRole == null)
            {
                return NotFound();
            }

            return Ok(aspNetRole);
        }

        // PUT: api/AspNetRoles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAspNetRole(string id, AspNetRole aspNetRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aspNetRole.Id)
            {
                return BadRequest();
            }

            db.Entry(aspNetRole).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetRoleExists(id))
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

        // POST: api/AspNetRoles
        [ResponseType(typeof(AspNetRole))]
        public async Task<IHttpActionResult> PostAspNetRole(AspNetRole aspNetRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AspNetRoles.Add(aspNetRole);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AspNetRoleExists(aspNetRole.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = aspNetRole.Id }, aspNetRole);
        }

        // DELETE: api/AspNetRoles/5
        [ResponseType(typeof(AspNetRole))]
        public async Task<IHttpActionResult> DeleteAspNetRole(string id)
        {
            AspNetRole aspNetRole = await db.AspNetRoles.FindAsync(id);
            if (aspNetRole == null)
            {
                return NotFound();
            }

            db.AspNetRoles.Remove(aspNetRole);
            await db.SaveChangesAsync();

            return Ok(aspNetRole);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AspNetRoleExists(string id)
        {
            return db.AspNetRoles.Count(e => e.Id == id) > 0;
        }
    }
}