using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Http.Description;
using HRinfoAPI.Models;
using Microsoft.AspNet.Identity;
using System.Web.Http.Cors;

namespace HRinfoAPI.Controllers
{
    [RoutePrefix("api/Company")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CompanyInfoController : ApiController
    {
        HRinfoEntities database = new HRinfoEntities();
        int? workerId;

        // TODO: ogarnąć
        private void GetEmployeeId()
        {
            //workerId = database.AspNetUsers.Where(a => a.Id == User.Identity.GetUserId()).Select(a => a.EmployeeId).Single();

            workerId = 1;
        }

        [Route("News")]
        public IEnumerable<NewsResults> News()
        {
            var result = from news in database.News
                         where news.IsActive == true
                            && (news.DateFrom == null
                                || news.DateFrom <= DateTime.UtcNow)
                            && (news.DateTo == null
                                || news.DateTo >= DateTime.UtcNow)
                         orderby news.DateFrom ascending
                         select new NewsResults() {
                             ResultNews = news.News1,
                             ResultDateFrom = news.DateFrom,
                             ResultDateTo = news.DateTo };

            return result.ToList();
        }

        /// <summary>
        /// Get top number of records order by id descending.
        /// </summary>
        /// <returns>NewsResults</returns>
        [Route("NewsLastAdd")]
        public IEnumerable<NewsResults> NewsLastAdd(int numberOfRecords)
        {
            var result = database.News.Where(news => news.IsActive == true
                    && (news.DateFrom == null || news.DateFrom <= DateTime.UtcNow)
                    && (news.DateTo == null || news.DateTo >= DateTime.UtcNow))
                .OrderByDescending(news => news.Id)
                .Select(news => new NewsResults()
                {
                    ResultNews = news.News1,
                    ResultDateFrom = news.DateFrom,
                    ResultDateTo = news.DateTo
                })
                .Take(numberOfRecords);

            return result.ToList();
        }

        /// <summary>
        /// Get limited number of records and skip declare number of records
        /// </summary>
        /// <param name="skipNumber">Number of records to skip</param>
        /// <param name="takeNumber">Number of records to take</param>
        /// <returns></returns>
        [Route("NewsByRecords")]
        public IEnumerable<NewsResults> NewsRecords(int skipNumber, int takeNumber)
        {
            var result = database.News.Where(news => news.IsActive == true
                    && (news.DateFrom == null || news.DateFrom <= DateTime.UtcNow)
                    && (news.DateTo == null || news.DateTo >= DateTime.UtcNow))
                .OrderByDescending(news => news.Id)
                .Select(news => new NewsResults()
                {
                    ResultNews = news.News1,
                    ResultDateFrom = news.DateFrom,
                    ResultDateTo = news.DateTo
                })
                .Skip(skipNumber)
                .Take(takeNumber);

            return result.ToList();
        }

        [Authorize]
        [Route("RaiseError")]
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
            privateMessage.TopicId = database.Topics.Single(t => t.Name == "Problemy z aplikacją").Id;
            privateMessage.StatusId = database.Status.Single(s => s.Name == "Wysłane" && s.EventId == database.Events.Single(e => e.Name == "Wiadomości").Id).Id;
            database.PrivateMessages.Add(privateMessage);
            await database.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = privateMessage.Id }, privateMessage);
        }
    }
}
